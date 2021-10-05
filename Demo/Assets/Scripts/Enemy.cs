using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected AudioSource deathAudio;
   
    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    public void Death()    //死亡，先放爆炸动画再销毁
    {
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        deathAudio.Play();
        animator.SetTrigger("death");
    }
}
