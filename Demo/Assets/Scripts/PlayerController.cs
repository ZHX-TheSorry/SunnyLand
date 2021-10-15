using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;  //��������

public class PlayerController : MonoBehaviour
{
    //�˴�������ʵ��������������unity�й�������Ӧ������Է���©
    //������animator = GetComponent<Animator>()�����Զ�����
    public Rigidbody2D rb;
    public float speed = 400;
    public float jumpForce;
    public Animator animator;   //������������
    public LayerMask ground;    //����ͼ��
    public Collider2D coll;     //��ײ��
    public Collider2D head;
    public Transform ceilingCheck;
    public int cherryNum = 0; //cherryʰȡ����
    public int gemNum = 0; //gemʰȡ����
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


    public Joystick joystick;   //�ֻ����ݸ�
    public Button jumpButton;

    [Header("Dash����")]

    public float dashTime;  //���ʱ��
    private float dashTimeLeft;
    public float dashCoolDowm;      //��ȴʱ��
    public float dashSpeed;
    private float lastDash=-10f;     //��һ�γ�̵�ʱ���

    private bool isDashing;

    [Header("CD��UI���")]
    public Image cdImage;
    
    // Update is called once per frame
    void FixedUpdate()  //�̶�50֡
    {
        Dash();
        if (isDashing)
        {
            return;     //����ڳ�̹����У��������ƶ�ָ��
        }

        if (!isHurt)        //δ����ʱ�������ƶ��������˼�isHurtΪtrue��������Ĵ����н��з�����������move
        {
            Move();
        }
        
        
        
    }

    void Update()
    {
        SwitchAnimation();

        //��Ծ(д��FixedUpdate�᲻����)

        Jump();   //�ֻ���ͨ��button����jump����������ע�͵�

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
        //�ֻ��˲ٿ�
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







        //pc�˲ٿ� 
        horizontalInput = Input.GetAxis("Horizontal");
        faceDirection = Input.GetAxisRaw("Horizontal"); //��ȡ-1��1
        //�����ƶ�
        if (horizontalInput != 0)
        {
            rb.velocity = new Vector2(horizontalInput * speed * Time.fixedDeltaTime, rb.velocity.y);   //velicityָʸ�����ٶ�;y�᲻��
            //rb.velocity = new Vector2(horizontalInput * speed , rb.velocity.y);

            animator.SetFloat("running", Mathf.Abs(horizontalInput));
        }

        //ת��
        if (faceDirection != 0)
        {
            transform.localScale = new Vector3(faceDirection, 1, 1);    //�ı�scale��ֵ��-1���ǽ�1����ͼ��ת���ﵽת���Ч��
        }


    }


    public void Jump()
    {
        //pc��
        if (Input.GetButtonDown("Jump") && jumpTime > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTime--;
            animator.SetBool("jumping", true);
            animator.SetBool("falling", false);
            jumpAudio.Play();
        }




        //�ֻ���
        //if (jumpTime > 0)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //    jumpTime--;
        //    animator.SetBool("jumping", true);
        //    animator.SetBool("falling", false);
        //    jumpAudio.Play();
        //}
    }

    //�¶�
    void Crouch()
    {
        speed = 400;    //�ָ��ٶ�
        //*********�˴����ϰ������ɿ��¶׺��߳��ϰ��ﲻ�ָܻ�վ�������޸�**************   
        if (!Physics2D.OverlapCircle(ceilingCheck.position, 0.2f, ground))     //���ͷ������û������ͽ����¶�������
        {


            if (Input.GetButton("Crouch"))    //pc
            //if (joystick.Vertical<-0.5f)
            {
                head.enabled = false;       //�¶�ʱɾ��ͷ����collider
                animator.SetBool("crouching", true);
                //�¶�ʱ�ٶȼ���
                speed = speed/2;
            }else  
            { 
                animator.SetBool("crouching", false);
                head.enabled = true;
            }
            
        }
        
    }


    //�����л�
    void SwitchAnimation()
    {
        animator.SetBool("idle", false);

        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(ground))
        {
            animator.SetBool("falling", true);
            
        }

        //��������Ծ״̬
        if (animator.GetBool("jumping"))
        {
            //���y��仯��С��0ʱ����ʼ����
            if (rb.velocity.y < 0)
            {
                animator.SetBool("falling", true);
                animator.SetBool("jumping", false);
            }

        }else if (coll.IsTouchingLayers(ground))    //**�����ײ������layer
        {
            jumpTime = 2;
            animator.SetBool("falling", false);
            animator.SetBool("jumping", false);
            animator.SetBool("idle", true);
        }
        if (isHurt)     //����
        {
            animator.SetBool("isHurt", true);
            animator.SetFloat("running", 0f);       //������˷������ܻص�idle״̬������

            //���˺�������״̬���������ٶȽ���0.5����ʱ�ָ�����״̬
            if (Mathf.Abs(rb.velocity.x) < 0.2f)
            {
                isHurt = false;
                animator.SetBool("isHurt", false);
                animator.SetBool("idle", true);
            }
        }
    }

    
    //��ײ����
    private void OnTriggerEnter2D(Collider2D collision)     //(�ȸ�Ҫʰȡ���������tag)������������collider,����ײʱ�Զ����ú�����
    {
        //ʰ������
        if (collision.tag=="CollectionCherry")     //����ײ��tagΪCollection�����壬������֮
        {
            cherryAudio.Play();     //����Ƶ

            //*****���������ײ�嶼����ײ���µļ��������ѽ��*****
            //Destroy(collision.gameObject);
            //cherryNum++;
            collision.GetComponent<Animator>().Play("feedback");    //�����ռ��������ڶ�������ʱ���ü���

        }else if(collision.tag == "CollectionGem")
        {
            gemAudio.Play();      //����Ƶ
            //Destroy(collision.gameObject);
            //gemNum++;
            //GemNum.text = gemNum.ToString();      //ͬ��UI
            collision.GetComponent<Animator>().Play("feedback");
        }



        if (collision.tag == "DeadLine")
        {
            bgm.Pause();
            dieAudio.Play();
            //�ӳٵ���Restart()�ؿ�
            Invoke("Restart",0.5f);
        }
    }


    //�������
    private void OnCollisionEnter2D(Collision2D collision)      //����ײ����ʱ����
    {
        if (collision.gameObject.tag == "Anemy")    
        {
            //ʵ����enemy
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            
            //�����������״̬�����������
            if (animator.GetBool("falling"))
            {
                enemy.JumpOn();  //�������ٺ���

                //������Զ���һ��
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 0.8f);
                animator.SetBool("jumping", true);
                animator.SetBool("falling", false);
            }else if (transform.position.x < collision.gameObject.transform.position.x)     //����
            {
                rb.velocity = new Vector2(-10, rb.velocity.y);      //���˷���
                isHurt = true;
                hurtAudio.Play();    //����Ƶ
            }
            else if(transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(10, rb.velocity.y);
                isHurt = true;
                hurtAudio.Play();    //����Ƶ
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




    




    //����ĺ����������������

    void Restart()
    {
        //�������ó���
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    //�ռ���Ʒ����
    public void CherryCount()
    {
        cherryNum++;
        CherryNum.text = cherryNum.ToString();      //ͬ��UI
    }

    public void GemCount()
    {
        gemNum++;
        GemNum.text = gemNum.ToString();
    }
}
