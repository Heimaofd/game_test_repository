using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class itemOnworld : MonoBehaviour
{
    public bag playerBag;
    public item playerItem;//!!!!���ⲿ����
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
<<<<<<< HEAD
            AddItem();
            //���õ���ӵ�е����ú���
            eventsystem.Instance.EventInvoke(playerItem.name);
            playerItem.isGet = true;
=======
       
            AddItem();
>>>>>>> 程序黑猫的泪痣
            Destroy(this.gameObject);
        }
    }
   void AddItem()
    {    
        //���Ƿ��漰�������
            //�жϵ����Ƿ��Ѿ���ȡ��һ�Σ�
        if(playerBag.items.Contains(playerItem))
        {
            playerItem.numItem++;
        }
        else
        {
            playerBag.items.Add(playerItem);
        }
    }
}
