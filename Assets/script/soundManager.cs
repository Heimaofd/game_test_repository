using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static soundManager Instance;
    private AudioSource audioManager;
    private AudioClip jumpSound;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }
    private void Start()
    {
        audioManager = GetComponent<AudioSource>();
        //在下面从Resources加载各种AudioClip
        jumpSound = Resources.Load<AudioClip>("sound/Saint5");
        print(jumpSound);
    }
    //在下面写不同音效的不同函数
   static public void jumpingSound()
    {
       Instance.audioManager.PlayOneShot(Instance.jumpSound);
    }
}
