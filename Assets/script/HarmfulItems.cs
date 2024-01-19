using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public abstract class HarmfulItems : MonoBehaviour
{

    public int damage;

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    { 
        //检测触碰
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerIncombat>().TakeDamage(damage);//对角色造成伤害
            Destroy(gameObject);                            //摧毁障碍物(如果有持续伤害性物体此处要做出更改，如：激光)
        }
    }
}
