using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class PlayerIncombat : MonoBehaviour
{
    public Transform playerTrans;
    private BoxCollider2D playerCollider;
    private SpriteRenderer PlayerRender;
    public bag playerBag;
    public BoxCollider2D reBoundColli;
    public GameObject MagicBal;
    public Transform MagicBallTrans;

    public int maxHealth;                //设置最大生命值
    public int health;

    public float moveTime;               //设置移动时间
    public float moveDistance;           //设置移动距离
    public float invincibleTime;         //设置无敌时间
    private float startInvincibleTime;                                     //

    public float roadUp;                 //设置三路位置  注意要与移动距离同步调整
    public float roadMiddle;
    public float roadDown;

    public float InvincibleCd;//无敌技能的Cd
    private float InvincibleStartCd;
    public float reBoundCd;//反弹障碍物CD
    private float startReBoundCd;
    public float wandCd;//法杖生成法球cd
    private float startWandCd;
    public int WindNum;//一次技能生成法球个数
   

    public float skillInvincibleTime;//无敌多长时间
    public float reBoundTime;//反弹持续时间

    public Image skillCD;
    public float CDtime;
   
    bool isCD;
    private bool isDamage;
    private bool isSkillInvincible;
    //判断玩家是否可以使用技能的bool值
    public bool canInvincble;
    public bool haveWand;
    public bool CanReBound;

    // Start is called before the first frame update
    void Start()
    {
        startWandCd= wandCd ;
        wandCd = 0;
         startReBoundCd=reBoundCd;
        startInvincibleTime = invincibleTime;
        invincibleTime = 0;
        PlayerRender = GetComponent<SpriteRenderer>();
        health = maxHealth;              //初始化生命值
        playerCollider = GetComponent<BoxCollider2D>();
        isCD = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(canInvincble);
        Move();
        Skill();
        CanInvincble();//无敌函数
        TurnColor();
         invincibleTime -= Time.deltaTime;//无敌时间减少，由无敌时间判断是否无敌
        ReBound();
        wend();
    }
    void TurnColor()
    {
        if (invincibleTime > 0&& PlayerRender.color != Color.red&& isSkillInvincible)
        {
            PlayerRender.color = Color.green;
        }
        if (invincibleTime <= 0)
        {
            isSkillInvincible = false;
        }
        if (!isDamage && invincibleTime <= 0)
        {

            PlayerRender.color = Color.white;
        }
    }
    private void OnEnable()
    {
        //先把道具函数初始化到事件中心
        eventsystem.Instance.setUpOrAdd("Invincible", PlayerInvincible);//此string变量和道具名字应该相同
        eventsystem.Instance.setUpOrAdd("wand", PlayerWand);
        eventsystem.Instance.setUpOrAdd("ReBound", playerReBound);
        //使场景刷新后在前一个场景已经获取的道具生效
        for (int i = 0; i < playerBag.items.Count; i++)
        {
            if (playerBag.items[i].isGet)
            {
                eventsystem.Instance.EventInvoke(playerBag.items[i].Name);
            }
        }
    }
    private void Awake()
    {
        InvincibleStartCd = InvincibleCd;
    }
    void Move()
    {
        if (Input.GetKeyDown(KeyCode.W))//按下按键“W”
        {
            if (playerTrans.position.y == roadUp)  //处于最高路，不再移动
            {
                return;
            }
            else if (playerTrans.position.y == roadMiddle || playerTrans.position.y == roadDown) //处于中间or下路，移动
            {
                playerTrans.DOMoveY(moveDistance, moveTime).SetRelative();
                soundManager.jumpingSound();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.S))//按下按键“S”
        {
            if (playerTrans.position.y == roadDown) //处于最低路，不再移动
            {
                return;
            }
            else if (playerTrans.position.y == roadMiddle || playerTrans.position.y == roadUp) //处于中间or上路，移动
            {
                playerTrans.DOMoveY(-moveDistance, moveTime).SetRelative();
                soundManager.jumpingSound();
            }
        }
    }

    void Skill()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            
            if (skillCD.fillAmount == 0f)        //技能处于就绪状态
            {
                
                skillCD.fillAmount = 1f;
                isCD = true;
            }
        }
        if (isCD)                                //技能处于冷却状态
        {
            skillCD.fillAmount -= 1f / CDtime * Time.deltaTime;
        }
    }

    public void TakeDamage (int damage)  //角色受伤
    {
        if (invincibleTime<=0)                //受到伤害
        {
              health -= damage;
            isDamage = true;
            eventsystem.Instance.EventInvoke("playerTakeDamage");//爱心减少
            Debug.Log("playerTakeDamage");
            if (PlayerRender.color != Color.green)
            {
                PlayerRender.color = Color.red;//变成红色（测试用）
                StartCoroutine("turnWhit");
            }
            if (health <= 0)
            {
                Debug.Log("die");
            }
            invincibleTime = startInvincibleTime;
        }                 
    }
   IEnumerator turnWhit()
    {
        yield return new WaitForSeconds(0.4f);
        PlayerRender.color = Color.white;
        isDamage = false;
    }
    public void CanInvincble()//按k键就无敌的函数
    {
        InvincibleCd -= Time.deltaTime;
        if (canInvincble && InvincibleCd<=0)//判断是否可以解锁无敌
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                invincibleTime = skillInvincibleTime;//无敌状态开启   
                InvincibleCd = InvincibleStartCd;
                isSkillInvincible = true;
            }
        }
    }
  public void ReBound()//反弹障碍物的函数
    {
        reBoundCd -= Time.deltaTime;
        if (CanReBound && reBoundCd <= 0)//判断是否可以解锁无敌
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                print("成功开启技能");
                reBoundColli.enabled = true;
                StartCoroutine("deleteColli");
                reBoundCd = startReBoundCd;
            }
        }
    }
    public void wend()//生成发球的函数
    {
        wandCd -= Time.deltaTime;
        if (haveWand && wandCd <= 0)//判断是否可以解锁无敌
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                print("成功开启wind技能");
                wandCd = startWandCd;
                StartCoroutine("lunch");
            }
        }
    }
    IEnumerator lunch()
    {
        for (int i = 0; i < WindNum; i++)
        {
            yield return new WaitForSeconds(0.2f);
            Instantiate(MagicBal,MagicBallTrans.position, Quaternion.identity);
        }
    }
    IEnumerator deleteColli()
    {
        yield return new WaitForSeconds(reBoundTime);
        reBoundColli.enabled = false;
    }
    //下面写各个道具bool值的解锁
    void PlayerInvincible()//玩家无敌
    {
       canInvincble = true;
    }
    void PlayerWand()//玩家的法杖
    {
        haveWand = true;
    }
    void playerReBound()//玩家反弹道具
    {
        CanReBound = true;
    }
}
