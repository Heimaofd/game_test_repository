using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reBound : MonoBehaviour
{
    public GameObject reBoundObstacle;
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
        if (collision.gameObject.CompareTag("obstacle"))
        {
            collision.GetComponent<obstacle>().isReBound = true;
        }
    }
}
