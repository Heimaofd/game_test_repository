using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public int BallDamage;
    private Rigidbody2D rigidbody;
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = new Vector2(Speed, rigidbody.velocity.y);
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ObstacleRemover") || collision.gameObject.CompareTag("boss"))//在摄像机外去除障碍
        {
            Destroy(this.gameObject);
        }
    }
}
