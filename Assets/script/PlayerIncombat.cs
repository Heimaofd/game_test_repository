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

    public int maxHealth;                //�����������ֵ
    public int health;

    public float moveTime;               //�����ƶ�ʱ��
    public float moveDistance;           //�����ƶ�����
    public float invincibleTime;         //�����޵�ʱ��
    bool isInvincible;            //�޵�״̬

    public float roadUp;                 //������·λ��  ע��Ҫ���ƶ�����ͬ������
    public float roadMiddle;
    public float roadDown;

    public float InvincibleCd;//�޵м��ܵ�Cd
    private float InvincibleStartCd;
    public float skillInvincibleTime;//�޵ж೤ʱ��
    public Image skillCD;
    public float CDtime;
   
    bool isCD;
    private bool isSkillInvincble;
    //�ж�����Ƿ����ʹ�ü��ܵ�boolֵ
    public bool canInvincble;
    public bool haveWand;
    public bool CanReBound;
    // Start is called before the first frame update
    void Start()
    {

        PlayerRender = GetComponent<SpriteRenderer>();
        health = maxHealth;              //��ʼ������ֵ
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
        CanInvincble();//�޵к���
        
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
        if (isInvincible||isSkillInvincble)                //�������޵�״̬������˺�
        {
            return;
        }
        if (!isInvincible && !isSkillInvincble)
        {
            health -= damage;
            eventsystem.Instance.EventInvoke("playerTakeDamage");//���ļ���
            Debug.Log("playerTakeDamage");
            if (PlayerRender.color != Color.green)
            {
                PlayerRender.color = Color.red;//��ɺ�ɫ�������ã�
            }
            if (health <= 0)
            {
                Debug.Log("die");
            }
            isInvincible = true;             //�޵�״̬����
            StartCoroutine(WaitInvincible());//����Э�̴����޵�ʱ��
        }
       
    }

    IEnumerator WaitInvincible()//�����޵�
    {
        yield return new WaitForSeconds(invincibleTime);
        PlayerRender.color = Color.white;
        isInvincible = false;
    }
  
    public void CanInvincble()//��k�����޵еĺ���
    {
        InvincibleCd -= Time.deltaTime;
        if (canInvincble && InvincibleCd<=0)//�ж��Ƿ���Խ����޵�
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                PlayerRender.color = Color.green;
                isSkillInvincble = true;             //�޵�״̬����
                StartCoroutine("WaitSkillInvincible");//���������޵�ʱ��
                InvincibleCd = InvincibleStartCd;
            }
        }
    }
    IEnumerator WaitSkillInvincible()//�����޵�
    {
        yield return new WaitForSeconds(skillInvincibleTime);
        PlayerRender.color = Color.white;
        isSkillInvincble = false;
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
