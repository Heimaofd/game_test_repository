using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    public GameObject Obstacle;
    public float refreshTime;//间隔多久刷新障碍物
    private float startRefreashTime;
    private int randomNum;
    //private int lastRandomNum;
    // Start is called before the first frame update
    void Start()
    {
        startRefreashTime = refreshTime;
    } 

    // Update is called once per frame
    void Update()
    {
        ObstacRefresh();
        print(randomNum);
        
        
    }
    void ObstacRefresh()
    {
        randomNum = Random.Range(1, 8);//生成1，2，3，4，5，6，7
        refreshTime -= Time.deltaTime;
        if (refreshTime <= 0 )
        {
            if(randomNum<=2&&randomNum>=1)//第一行
            {
               Instantiate(Obstacle, transform.GetChild(0).transform.position, Quaternion.identity);
            }
            
            if (randomNum <= 5 && randomNum >= 3)//第二行
            {
               Instantiate(Obstacle, transform.GetChild(1).transform.position, Quaternion.identity);
            }
             if (randomNum >= 6 && randomNum <= 7)//第三行  
            {
                Instantiate(Obstacle, transform.GetChild(2).transform.position, Quaternion.identity);
            }
            refreshTime = startRefreashTime;
        }
    }
}
