using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : Enemy
{
    private Rigidbody2D rb;
    //private Animator animator;
    private Collider2D coll;
    public LayerMask ground;    //�ǵ���unity�ﹴѡһ��ground

    public Transform leftPoint, rightPoint;
    public bool facingLeft = true;
    public float speed = 5;
    public float jumpForce;

    private float leftX, rightX;
    // Start is called before the first frame update
    protected override void Start()     //��д���෽��
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        transform.DetachChildren();
        leftX = leftPoint.position.x;
        rightX = rightPoint.position.x;
        //��ȡ�������ƶ��ı߽������ٶ���
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnimation();
    }

    
    //�ڶ����¼���ÿ��������idle���������
    void Move()
    {   

        //***���Ż�����ת������Ծ����һ�µ�����

        if (facingLeft)//������
        {

            animator.SetBool("jumping", true);
            rb.velocity = new Vector2(-speed, jumpForce);
            if (transform.position.x < leftX && coll.IsTouchingLayers(ground))      //�ڵ�������Ծ,������Χ��ת��
            {
                transform.localScale = new Vector3(-1, 1, 1);
                facingLeft = false;

                animator.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);
            }
        }else//����
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
        if (animator.GetBool("jumping"))    //����ʼ����
        {
            if (rb.velocity.y < 0.1f)
            {
                animator.SetBool("falling", true);
                animator.SetBool("jumping", false);
            }
        }
        if (coll.IsTouchingLayers(ground) && animator.GetBool("falling"))   //����������
        {
            animator.SetBool("falling", false);
        }
    }


    
}
