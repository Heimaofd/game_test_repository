using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class bagManager : MonoBehaviour
{
    public GameObject bagPenel;
    public player player;//����������
    public static bagManager instance;
    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(this);
        }
        instance = this;
    }
     void OnEnable()//����
    {
        
    }
    static void refreshItem()
    {
        //��ɾ��
        for(int i=0;i<instance.bagPenel.transform.childCount;i++)
        {
            if (instance.bagPenel.transform.childCount == 0)
                break;
            Destroy(instance.bagPenel.transform.GetChild(i).gameObject);
        }
        //
    }


}
