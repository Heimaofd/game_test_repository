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
    public BoxCollider2D shortBladeColli;
    public SpriteRenderer LightTingsprite;
    public boss boss;

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
    public float shortBladeCd;//短刃冷却时间
    private float shortBladeStartCd;
    public float finalJudgementCd;//制裁冷却时间
    private float startFinalJudgement;
    public int WindNum;//一次技能生成法球个数
    private float RollBackTime;//后摇时间
    public float wandRollBackTIme;//法杖后摇时间
    public float shortBladeRollBackTime;
    public float ReBoundInvincibTime;//护盾的无敌时间
    public float JudementRollBackTime;//终阎制裁后摇

    public float skillInvincibleTime;//无敌多长时间
    public float reBoundTime;//反弹持续时间
    public float ShortBladeTime;//短刃效果持续时间

    public Image skillCD;
    public float CDtime;

    public int judgementDamage;//制裁技能伤害

    bool isCD;
    private bool isDamage;
    private bool isSkillInvincible;
    //判断玩家是否可以使用技能的bool值
    public bool haveShield;
    public bool haveWand;
    public bool CanReBound;
    public bool haveShortBlade;
    public bool haveFinalJudgement;
    // Start is called before the first frame update
    void Start()
    {
        InvincibleStartCd = InvincibleCd;
        InvincibleCd = 0;
       startWandCd = wandCd ;
        wandCd = 0;
         startReBoundCd=reBoundCd;
        reBoundCd = 0;
        invincibleTime = 0;
        shortBladeStartCd = shortBladeCd;
        shortBladeCd = 0; 
        startFinalJudgement = finalJudgementCd;
        finalJudgementCd = 0;
        PlayerRender = GetComponent<SpriteRenderer>();
        health = maxHealth;              //初始化生命值
        playerCollider = GetComponent<BoxCollider2D>();
        isCD = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Skill();
        CanInvincble();//无敌函数
        TurnColor();
         invincibleTime -= Time.deltaTime;//无敌时间减少，由无敌时间判断是否无敌
       RollBackTime -= Time.deltaTime;
        ReBound();
        wend();
        shorBlade();
        FinalJudgement();
    }
    void TurnColor()
    {
        if (invincibleTime > 0&& PlayerRender.color != Color.red && isSkillInvincible)
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
        eventsystem.Instance.setUpOrAdd("shield", PlayerShield);//此string变量和道具名字应该相同
        eventsystem.Instance.setUpOrAdd("wand", PlayerWand);
        eventsystem.Instance.setUpOrAdd("ReBound", playerReBound);
        eventsystem.Instance.setUpOrAdd("shortBlade", PlayerShortBlade);
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
            else if ((playerTrans.position.y == roadMiddle || playerTrans.position.y == roadDown )&&RollBackTime<=0) //处于中间or下路，移动
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
            else if ((playerTrans.position.y == roadMiddle || playerTrans.position.y == roadUp) && RollBackTime <= 0) //处于中间or上路，移动
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
    public void CanInvincble()//按k键就无敌的函数(盾牌)
    {
        InvincibleCd -= Time.deltaTime;
        if (haveShield && InvincibleCd<=0)//判断是否可以解锁无敌
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                invincibleTime = skillInvincibleTime;//无敌状态开启   
                InvincibleCd = InvincibleStartCd;
                isSkillInvincible = true;
            }
        }
    }
  public void ReBound()//反弹护盾的函数
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
                invincibleTime = ReBoundInvincibTime;
                isSkillInvincible = true;
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
                RollBackTime = wandRollBackTIme;
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
        isSkillInvincible = false;
    }
    public void shorBlade()//短刃的函数
    {
        shortBladeCd-= Time.deltaTime;
        if (haveShortBlade && shortBladeCd <= 0)//判断是否可以解锁无敌
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                print("成功开启shortBlade技能");
                shortBladeColli.enabled = true;
                StartCoroutine("deleteShortBladeColli");//消除短刃的碰撞体
                shortBladeCd = shortBladeStartCd;
                RollBackTime = shortBladeRollBackTime;
                
            }
        }
    }
    IEnumerator deleteShortBladeColli()//消除短刃的函数
    {
        yield return new WaitForSeconds(ShortBladeTime);
        shortBladeColli.enabled = false;
        
    }
    public void FinalJudgement()//终阎制裁
    {
        finalJudgementCd-= Time.deltaTime;
        if (haveFinalJudgement && finalJudgementCd<= 0)//判断是否可以解锁无敌
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                print("成功开启FinalJudgement技能");
                
                StartCoroutine("generateLighting");
                finalJudgementCd = startFinalJudgement;
                RollBackTime = JudementRollBackTime;
                
            }
        }
    }
    IEnumerator generateLighting()
    {
        yield return new WaitForSeconds(0.5f);
        LightTingsprite.enabled = true;
        print("制裁");
        boss.BossHealth -=judgementDamage;
        StartCoroutine("deleteLighting");
    }
    IEnumerator deleteLighting()
    {
        yield return new WaitForSeconds(0.4f);
        LightTingsprite.enabled = false;
    }
    //下面写各个道具bool值的解锁
    void PlayerShield()//玩家无敌
    {
       haveShield = true;
    }
    void PlayerWand()//玩家的法杖
    {
        haveWand = true;
    }
    void playerReBound()//玩家反弹道具
    {
        CanReBound = true;
    }
    void PlayerShortBlade()
    {
        haveShortBlade = true;
    }
    void PlayerFinalJudgement()
    {
        haveFinalJudgement = true;
    }
}
