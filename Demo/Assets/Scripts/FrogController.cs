using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : Enemy
{
    private Rigidbody2D rb;
    //private Animator animator;
    private Collider2D coll;
    public LayerMask ground;    //记得在unity里勾选一下ground

    public Transform leftPoint, rightPoint;
    public bool facingLeft = true;
    public float speed = 5;
    public float jumpForce;

    private float leftX, rightX;
    // Start is called before the first frame update
    protected override void Start()     //重写父类方法
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        transform.DetachChildren();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        //获取到左右移动的边界后就销毁对象
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnimation();
    }

    
    //在动画事件中每次运行完idle动画后调用
    void Move()
    {   

        //***已优化青蛙转向与跳跃方向不一致的问题

        if (facingLeft)//面向左
        {

            animator.SetBool("jumping", true);
            rb.velocity = new Vector2(-speed, jumpForce);
            if (transform.position.x < leftX && coll.IsTouchingLayers(ground))      //在地面则跳跃,超出范围则转向
            {
                transform.localScale = new Vector3(-1, 1, 1);
                facingLeft = false;

                animator.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
            }
        }else//向右
        {
            animator.SetBool("jumping", true);
            rb.velocity = new Vector2(speed, jumpForce);
            if (transform.position.x > rightX && coll.IsTouchingLayers(ground))      
            {
                transform.localScale = new Vector3(1, 1, 1);
                facingLeft = true;

                animator.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            
        }
    }


    void SwitchAnimation()
    {
        if (animator.GetBool("jumping"))    //跳起开始下落
        {
            if (rb.velocity.y < 0.1f)
            {
                animator.SetBool("falling", true);
                animator.SetBool("jumping", false);
            }
        }
        if (coll.IsTouchingLayers(ground) && animator.GetBool("falling"))   //下落至地面
        {
            animator.SetBool("falling", false);
        }
    }


    
}
