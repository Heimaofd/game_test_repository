using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //һЩ���߶�Ӧ�Ĺ����Ƿ����ʹ�õ�boolֵ
   public bool unbeatable;
    private Rigidbody2D playerRig;
    private float horizontalInput;
    private float verticalInput;
    public float playerSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);//��Ʒֱ���
        playerRig = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        playerRig.velocity = new Vector2(horizontalInput* playerSpeed, playerRig.velocity.y);
        playerRig.velocity = new Vector2(playerRig.velocity.x, verticalInput* playerSpeed);
    }
}
