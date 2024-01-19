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

    public int maxHealth;                //�����������ֵ
    public int health;

    public float moveTime;               //�����ƶ�ʱ��
    public float moveDistance;           //�����ƶ�����
    public float invincibleTime;         //�����޵�ʱ��
    private float startInvincibleTime;                                     //

    public float roadUp;                 //������·λ��  ע��Ҫ���ƶ�����ͬ������
    public float roadMiddle;
    public float roadDown;

    public float InvincibleCd;//�޵м��ܵ�Cd
    private float InvincibleStartCd;
    public float reBoundCd;//�����ϰ���CD
    private float startReBoundCd;
    public float wandCd;//�������ɷ���cd
    private float startWandCd;
    public int WindNum;//һ�μ������ɷ������
   

    public float skillInvincibleTime;//�޵ж೤ʱ��
    public float reBoundTime;//��������ʱ��

    public Image skillCD;
    public float CDtime;
   
    bool isCD;
    private bool isDamage;
    private bool isSkillInvincible;
    //�ж�����Ƿ����ʹ�ü��ܵ�boolֵ
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
        health = maxHealth;              //��ʼ������ֵ
        playerCollider = GetComponent<BoxCollider2D>();
        isCD = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(canInvincble);
        Move();
        Skill();
        CanInvincble();//�޵к���
        TurnColor();
         invincibleTime -= Time.deltaTime;//�޵�ʱ����٣����޵�ʱ���ж��Ƿ��޵�
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
        //�Ȱѵ��ߺ�����ʼ�����¼�����
        eventsystem.Instance.setUpOrAdd("Invincible", PlayerInvincible);//��string�����͵�������Ӧ����ͬ
        eventsystem.Instance.setUpOrAdd("wand", PlayerWand);
        eventsystem.Instance.setUpOrAdd("ReBound", playerReBound);
        //ʹ����ˢ�º���ǰһ�������Ѿ���ȡ�ĵ�����Ч
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
        if (Input.GetKeyDown(KeyCode.W))//���°�����W��
        {
            if (playerTrans.position.y == roadUp)  //�������·�������ƶ�
            {
                return;
            }
            else if (playerTrans.position.y == roadMiddle || playerTrans.position.y == roadDown) //�����м�or��·���ƶ�
            {
                playerTrans.DOMoveY(moveDistance, moveTime).SetRelative();
                soundManager.jumpingSound();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.S))//���°�����S��
        {
            if (playerTrans.position.y == roadDown) //�������·�������ƶ�
            {
                return;
            }
            else if (playerTrans.position.y == roadMiddle || playerTrans.position.y == roadUp) //�����м�or��·���ƶ�
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
            
            if (skillCD.fillAmount == 0f)        //���ܴ��ھ���״̬
            {
                
                skillCD.fillAmount = 1f;
                isCD = true;
            }
        }
        if (isCD)                                //���ܴ�����ȴ״̬
        {
            skillCD.fillAmount -= 1f / CDtime * Time.deltaTime;
        }
    }

    public void TakeDamage (int damage)  //��ɫ����
    {
        if (invincibleTime<=0)                //�ܵ��˺�
        {
              health -= damage;
            isDamage = true;
            eventsystem.Instance.EventInvoke("playerTakeDamage");//���ļ���
            Debug.Log("playerTakeDamage");
            if (PlayerRender.color != Color.green)
            {
                PlayerRender.color = Color.red;//��ɺ�ɫ�������ã�
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
    public void CanInvincble()//��k�����޵еĺ���
    {
        InvincibleCd -= Time.deltaTime;
        if (canInvincble && InvincibleCd<=0)//�ж��Ƿ���Խ����޵�
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                invincibleTime = skillInvincibleTime;//�޵�״̬����   
                InvincibleCd = InvincibleStartCd;
                isSkillInvincible = true;
            }
        }
    }
  public void ReBound()//�����ϰ���ĺ���
    {
        reBoundCd -= Time.deltaTime;
        if (CanReBound && reBoundCd <= 0)//�ж��Ƿ���Խ����޵�
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                print("�ɹ���������");
                reBoundColli.enabled = true;
                StartCoroutine("deleteColli");
                reBoundCd = startReBoundCd;
            }
        }
    }
    public void wend()//���ɷ���ĺ���
    {
        wandCd -= Time.deltaTime;
        if (haveWand && wandCd <= 0)//�ж��Ƿ���Խ����޵�
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                print("�ɹ�����wind����");
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
    //����д��������boolֵ�Ľ���
    void PlayerInvincible()//����޵�
    {
       canInvincble = true;
    }
    void PlayerWand()//��ҵķ���
    {
        haveWand = true;
    }
    void playerReBound()//��ҷ�������
    {
        CanReBound = true;
    }
}
