using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    public float leftSpeed;
    public float rightSpeed;
    public bool isReBound;
    public int damageToBoss;
    // Start is called before the first frame update
    // Update is called once per frame
    private void Start()
    {
        isReBound = false;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (!isReBound)
        {
            rb.velocity = new Vector2(-leftSpeed, rb.velocity.y);//向左移动
        }
       else
        {
            rb.velocity = new Vector2(rightSpeed, rb.velocity.y);//向左移动
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ObstacleRemover")|| collision.gameObject.CompareTag("boss"))//在摄像机外去除障碍
        {
            Destroy(this.gameObject);
        }
    }
}
