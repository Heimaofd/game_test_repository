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
    public item thisItem;//!!!!在外部拖入
    public void interaction()
    {
        if (!playerBag.items.Contains(thisItem))
        {
            Debug.Log("打开背包成功");
            playerBag.items.Add(thisItem);//添加到背包
            thisItem.isGet = true;
            eventsystem.Instance.EventInvoke("Invincible");
            sceneManager.Instance.nextScene();
            //ItemManager.creatNewItem(thisItem);
        }
        else
        {
            thisItem.numItem++;
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
