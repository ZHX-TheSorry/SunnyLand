using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossumController : Enemy
{
    private Rigidbody2D rb;
    public bool facingLeft = true;
    public float speed;
    public Transform leftPoint, rightPoint;
    private float leftX, rightX;
    //public Animator animator;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);

        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }



    private void Move()
    {
        if (facingLeft)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            if (transform.position.x < leftX)      //在地面则跳跃,超出范围则转向
            {
                transform.localScale = new Vector3(-1, 1, 1);
                facingLeft = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x > rightX)      //在地面则跳跃,超出范围则转向
            {
                transform.localScale = new Vector3(1, 1, 1);
                facingLeft = true;
            }
        }

        //if (animator.GetBool("Death"))
        //{
        //    return;
        //}
    }
}
