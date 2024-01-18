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
    int health;

    public float moveTime;               //�����ƶ�ʱ��
    public float moveDistance;           //�����ƶ�����
    public float invincibleTime;         //�����޵�ʱ��
    bool isInvincible;            //�޵�״̬

    public float roadUp;                 //������·λ��  ע��Ҫ���ƶ�����ͬ������
    public float roadMiddle;
    public float roadDown;
    

    public Image skillCD;
    public float CDtime;
    bool isCD;

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
        if (isInvincible)                //�������޵�״̬������˺�
        {
            return;
        }
        health -= damage;
        PlayerRender.color = Color.red;//��ɺ�ɫ�������ã�
        StartCoroutine(turnWaite());
        if (health <= 0)
        {
            Debug.Log("die");
        }
        isInvincible = true;             //�޵�״̬����
        StartCoroutine(WaitInvincible());//����Э�̴����޵�ʱ��
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
    public void CanInvincble()//��k�����޵еĺ���
    {
        if (canInvincble)//�ж��Ƿ���Խ����޵�
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                isInvincible = true;             //�޵�״̬����
                StartCoroutine(WaitInvincible());//����Э�̴����޵�ʱ��
            }
        }
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
