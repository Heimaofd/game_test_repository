using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    public Sprite heartFull;
    public Sprite heartEmpty;
    public PlayerIncombat player;
    // Start is called before the first frame update
    void Start()
    {
        eventsystem.Instance.setUpOrAdd("playerTakeDamage", reduceHeart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void reduceHeart()
    {
        if (player.health >= 0)
        {
            for (int i = 0; i <= player.health - 1; i++)
            {
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = heartFull;
            }
            for (int i = player.health; i <= player.maxHealth - 1; i++)
            {
                transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = heartEmpty;
            }
        }  
    }
}
