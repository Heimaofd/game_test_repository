using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour,Imysigninterfence
{
    public Sprite closeSprite;//����sprite
    public Sprite openSprite;

    private bool isOpen;//���س������жϱ����Ƿ��
    private SpriteRenderer boxSprite;
    public bag playerBag;
    public item thisItem;//!!!!���ⲿ����
    public void interaction()
    {
        if (!playerBag.items.Contains(thisItem))
        {
            Debug.Log("�򿪱����ɹ�");
            playerBag.items.Add(thisItem);//��ӵ�����
            thisItem.isGet = true;
            eventsystem.Instance.EventInvoke("Invincible");
            sceneManager.Instance.nextScene();
            //ItemManager.creatNewItem(thisItem);
        }
        else
        {
            thisItem.numItem++;
        }
        bagManager.refreshItem();//��bag������ݸ��µ�UI��
        gameObject.tag = "Untagged";
        isOpen = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        boxSprite = GetComponent<SpriteRenderer>();
        //��¼�Ƿ�򿪿������ļ���
    }
    void OnEnable()    // Update is called once per frame
    {
        //���±���ͼ��״̬
        //boxSprite.sprite = isOpen ? openSprite : closeSprite;
        if(isOpen)
        {
            gameObject.tag = "Untagged";
        }
    }
    void Update()
    {
        
    }
}
