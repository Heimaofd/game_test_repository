using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class bagManager : MonoBehaviour
{
    public GameObject bagPenel;
    public PlayerIncombat PlayerIncombat;//！！！拖入
    public static bagManager instance;
    public bag playerBag;
    private void Start()
    {
       
       
    }
    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(this);
        }
        instance = this;
    }
     void OnEnable()//根据道具bool值判断是否要调用事件中心对应道具的函数
    {

    }  
public static void refreshItem()
    {
        //先删除
        for(int i=0;i<instance.bagPenel.transform.childCount;i++)
        {
            if (instance.bagPenel.transform.childCount == 0)
                break;
            Destroy(instance.bagPenel.transform.GetChild(i).gameObject);
        }
        //后更新
        for(int i=0;i<instance.playerBag.items.Count;i++)
        {
            //更新每个bag里道具的数据到UI里（数目，描述，）
        }
    }

}
