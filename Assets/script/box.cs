using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour,Imysigninterfence
{
    public Sprite closeSprite;//拖入sprite
    public Sprite openSprite;

    private bool isOpen;//加载场景后判断宝箱是否打开
    private SpriteRenderer boxSprite;
    public bag playerBag;
    public item InvincibleItem;//!!!!在外部拖入
    public item wandItem;
    public item reBoundItem;
    public void interaction()
    {
        if (!playerBag.items.Contains(InvincibleItem))//无敌
        {
            Debug.Log("打开背包成功");
            playerBag.items.Add(InvincibleItem);//添加到背包
            InvincibleItem.isGet = true;
            eventsystem.Instance.EventInvoke("Invincible");
            sceneManager.Instance.nextScene();
            InvincibleItem.numItem++;
            //ItemManager.creatNewItem(thisItem);
        }
        else
        {
            InvincibleItem.numItem++;
            InvincibleItem.isGet = true;
        }
        if (!playerBag.items.Contains(wandItem))//法杖
        {
            Debug.Log("打开背包成功");
            playerBag.items.Add(wandItem);//添加到背包
            wandItem.isGet = true;
            eventsystem.Instance.EventInvoke("wand");
            sceneManager.Instance.nextScene();
            wandItem.numItem++;
            //ItemManager.creatNewItem(thisItem);
        }
        else
        {
            wandItem.numItem++;
            wandItem.isGet = true;
        }
        if (!playerBag.items.Contains(reBoundItem))//反弹
        {
            Debug.Log("打开背包成功");
            playerBag.items.Add(reBoundItem);//添加到背包
            reBoundItem.isGet = true;
            eventsystem.Instance.EventInvoke("reBoundItem");
            sceneManager.Instance.nextScene();
            reBoundItem.numItem++;
            //ItemManager.creatNewItem(thisItem);
        }
        else
        {
            reBoundItem.numItem++;
            reBoundItem.isGet = true;
        }
        bagManager.refreshItem();//把bag里的数据更新到UI里
        gameObject.tag = "Untagged";
        isOpen = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        boxSprite = GetComponent<SpriteRenderer>();
        //记录是否打开可以用文件等
    }
    void OnEnable()    // Update is called once per frame
    {
        //更新宝箱图标状态
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
