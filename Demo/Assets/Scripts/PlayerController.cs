using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;  //场景处理

public class PlayerController : MonoBehaviour
{
    //此处对象在实例化后建议立刻在unity中挂载上相应的组件以防遗漏
    //或者如animator = GetComponent<Animator>()即可自动挂载
    public Rigidbody2D rb;
    public float speed = 400;
    public float jumpForce;
    public Animator animator;   //动画参数控制
    public LayerMask ground;    //地面图层
    public Collider2D coll;     //碰撞体
    public Collider2D head;
    public Transform ceilingCheck;
    public int cherryNum = 0; //cherry拾取计数
    public int gemNum = 0; //gem拾取计数
    public int jumpTime = 2;

    public Text CherryNum;
    public Text GemNum;

    private float horizontalInput;
    private float faceDirection;
    private bool isHurt = false;

    public AudioSource bgm;
    public AudioSource jumpAudio;
    public AudioSource hurtAudio;
    public AudioSource cherryAudio;
    public AudioSource gemAudio;
    public AudioSource dieAudio;


    public Joystick joystick;   //手机操纵杆
    public Button jumpButton;

    [Header("Dash参数")]

    public float dashTime;  //冲刺时长
    private float dashTimeLeft;
    public float dashCoolDowm;      //冷却时间
    public float dashSpeed;
    private float lastDash=-10f;     //上一次冲刺的时间点

    private bool isDashing;

    [Header("CD的UI组件")]
    public Image cdImage;
    
    // Update is called once per frame
    void FixedUpdate()  //固定50帧
    {
        Dash();
        if (isDashing)
        {
            return;     //如果在冲刺过程中，则屏蔽移动指令
        }

        if (!isHurt)        //未受伤时，正常移动；若受伤即isHurt为true则在下面的代码中进行反弹而不调用move
        {
            Move();
        }
        
        
        
    }

    void Update()
    {
        SwitchAnimation();

        //跳跃(写在FixedUpdate会不流畅)

        Jump();   //手机端通过button调用jump，所以这里注释掉

        Crouch();

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time >= (lastDash + dashCoolDowm))
            {
                ReadyToDash();
                cdImage.fillAmount = 1.0f;
            }
        }

        cdImage.fillAmount -= 1.0f / dashCoolDowm * Time.deltaTime;
        
    }




    void Move()
    {
        //手机端操控
        //horizontalInput = joystick.Horizontal;      
        //if (horizontalInput > 0f)
        //{
        //    transform.localScale = new Vector3(1, 1, 1);
        //    rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
        //    animator.SetFloat("running", 1f);
        //}
        //else if(horizontalInput < 0f)
        //{
        //    transform.localScale = new Vector3(-1, 1, 1);
        //    rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, rb.velocity.y);
        //    animator.SetFloat("running", 1f);
        //}
        //else if (horizontalInput == 0f)
        //{
        //    rb.velocity = new Vector2(0f, rb.velocity.y);
        //    animator.SetFloat("running", 0f);
        //}







        //pc端操控 
        horizontalInput = Input.GetAxis("Horizontal");
        faceDirection = Input.GetAxisRaw("Horizontal"); //获取-1或1
        //左右移动
        if (horizontalInput != 0)
        {
            rb.velocity = new Vector2(horizontalInput * speed * Time.fixedDeltaTime, rb.velocity.y);   //velicity指矢量的速度;y轴不变
            //rb.velocity = new Vector2(horizontalInput * speed , rb.velocity.y);

            animator.SetFloat("running", Mathf.Abs(horizontalInput));
        }

        //转身
        if (faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);    //改变scale的值，-1就是将1的贴图反转，达到转身的效果
        }


    }


    public void Jump()
    {
        //pc端
        if (Input.GetButtonDown("Jump") && jumpTime > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTime--;
            animator.SetBool("jumping", true);
            animator.SetBool("falling", false);
            jumpAudio.Play();
        }




        //手机端
        //if (jumpTime > 0)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //    jumpTime--;
        //    animator.SetBool("jumping", true);
        //    animator.SetBool("falling", false);
        //    jumpAudio.Play();
        //}
    }

    //下蹲
    void Crouch()
    {
        speed = 400;    //恢复速度
        //*********此处在障碍物下松开下蹲后，走出障碍物不能恢复站立，待修复**************   
        if (!Physics2D.OverlapCircle(ceilingCheck.position, 0.2f, ground))     //如果头顶附件没有物体就进行下蹲与起立
        {


            if (Input.GetButton("Crouch"))    //pc
            //if (joystick.Vertical<-0.5f)
            {
                head.enabled = false;       //下蹲时删除头部的collider
                animator.SetBool("crouching", true);
                //下蹲时速度减半
                speed = speed/2;
            }else  
            { 
                animator.SetBool("crouching", false);
                head.enabled = true;
            }
            
        }
        
    }


    //动画切换
    void SwitchAnimation()
    {
        animator.SetBool("idle", false);

        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", true);
            
        }

        //若处于跳跃状态
        if (animator.GetBool("jumping"))
        {
            //检测y轴变化，小于0时即开始下落
            if (rb.velocity.y < 0)
            {
                animator.SetBool("falling", true);
                animator.SetBool("jumping", false);
            }

        }else if (coll.IsTouchingLayers(ground))    //**检测碰撞到地面layer
        {
            jumpTime = 2;
            animator.SetBool("falling", false);
            animator.SetBool("jumping", false);
            animator.SetBool("idle", true);
        }
        if (isHurt)     //受伤
        {
            animator.SetBool("isHurt", true);
            animator.SetFloat("running", 0f);       //解决受伤反弹后不能回到idle状态的问题

            //受伤后处于受伤状态被反弹后，速度降至0.5以下时恢复正常状态
            if (Mathf.Abs(rb.velocity.x) < 0.2f)
            {
                isHurt = false;
                animator.SetBool("isHurt", false);
                animator.SetBool("idle", true);
            }
        }
    }

    
    //碰撞触发
    private void OnTriggerEnter2D(Collider2D collision)     //(先给要拾取的物体加上tag)输入的是物体的collider,在碰撞时自动调用和输入
    {
        //拾起物体
        if (collision.tag=="CollectionCherry")     //若碰撞到tag为Collection的物体，则销毁之
        {
            cherryAudio.Play();     //放音频

            //*****检测两个碰撞体都有碰撞导致的计数错误已解决*****
            //Destroy(collision.gameObject);
            //cherryNum++;
            collision.GetComponent<Animator>().Play("feedback");    //运行收集动画，在动画结束时调用计数

        }else if(collision.tag == "CollectionGem")
        {
            gemAudio.Play();      //放音频
            //Destroy(collision.gameObject);
            //gemNum++;
            //GemNum.text = gemNum.ToString();      //同步UI
            collision.GetComponent<Animator>().Play("feedback");
        }



        if (collision.tag == "DeadLine")
        {
            bgm.Pause();
            dieAudio.Play();
            //延迟调用Restart()重开
            Invoke("Restart",0.5f);
        }
    }


    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)      //当碰撞发生时调用
    {
        if (collision.gameObject.tag == "Anemy")    
        {
            //实例化enemy
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            
            //如果处于下落状态才能消灭敌人
            if (animator.GetBool("falling"))
            {
                enemy.JumpOn();  //调用销毁函数

                //消灭后自动跳一下
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.8f);
                animator.SetBool("jumping", true);
                animator.SetBool("falling", false);
            }else if (transform.position.x < collision.gameObject.transform.position.x)     //受伤
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);      //受伤反弹
                isHurt = true;
                hurtAudio.Play();    //放音频
            }
            else if(transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                isHurt = true;
                hurtAudio.Play();    //放音频
            }

        }
        
    }


    void ReadyToDash()
    {
        isDashing = true;

        dashTimeLeft = dashTime;

        lastDash = Time.time;
    }



    void Dash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                rb.velocity = new Vector2(dashSpeed * transform.localScale.x, rb.velocity.y);

                dashTimeLeft -= Time.deltaTime;

                ShadowPool.instance.GetFromPool();
            }
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
            }
        }
    }




    




    //下面的函数供其他对象调用

    void Restart()
    {
        //死亡重置场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    //收集物品计数
    public void CherryCount()
    {
        cherryNum++;
        CherryNum.text = cherryNum.ToString();      //同步UI
    }

    public void GemCount()
    {
        gemNum++;
        GemNum.text = gemNum.ToString();
    }
}
