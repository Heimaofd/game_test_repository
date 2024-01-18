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

    public int maxHealth;                //设置最大生命值
    int health;

    public float moveTime;               //设置移动时间
    public float moveDistance;           //设置移动距离
    public float invincibleTime;         //设置无敌时间
    bool isInvincible;            //无敌状态

    public float roadUp;                 //设置三路位置  注意要与移动距离同步调整
    public float roadMiddle;
    public float roadDown;
    

    public Image skillCD;
    public float CDtime;
    bool isCD;

    //判断玩家是否可以使用技能的bool值
    public bool canInvincble;
    public bool haveWand;
    public bool CanReBound;
    // Start is called before the first frame update
    void Start()
    {
        PlayerRender = GetComponent<SpriteRenderer>();
        health = maxHealth;              //初始化生命值
        playerCollider = GetComponent<BoxCollider2D>();
        isInvincible = false;
        isCD = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(canInvincble);
        Move();
        Skill();
        CanInvincble();//无敌函数
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
        if (isInvincible)                //若处于无敌状态，免除伤害
        {
            return;
        }
        health -= damage;
        PlayerRender.color = Color.red;//变成红色（测试用）
        StartCoroutine(turnWaite());
        if (health <= 0)
        {
            Debug.Log("die");
        }
        isInvincible = true;             //无敌状态开启
        StartCoroutine(WaitInvincible());//开启协程处理无敌时间
    }

    IEnumerator WaitInvincible()
    {
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
    IEnumerator turnWaite()
    {
        yield return new WaitForSeconds(0.8f);
        PlayerRender.color = Color.white;
    }
    public void CanInvincble()//按k键就无敌的函数
    {
        if (canInvincble)//判断是否可以解锁无敌
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                isInvincible = true;             //无敌状态开启
                StartCoroutine(WaitInvincible());//开启协程处理无敌时间
            }
        }
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
