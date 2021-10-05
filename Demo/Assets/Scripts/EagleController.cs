using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : Enemy
{
    private Rigidbody2D rb;
    public float flyForce;
    public Transform highPoint, lowPoint;
    private float highY,lowY;
    private bool isUp;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();   //调用父类的方法
        rb = GetComponent<Rigidbody2D>();
        highY = highPoint.position.y;
        lowY = lowPoint.position.y;
        Destroy(highPoint.gameObject);
        Destroy(lowPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //ChangeFacing();
    }

    void Fly()  //扑棱
    {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.position.x, flyForce);
        }
        else
        {
            rb.velocity = new Vector2(rb.position.x, flyForce / 3);
        }
        if (rb.position.y < lowY)
        {
            isUp = true;
            rb.velocity = new Vector2(rb.position.x, flyForce);
        }
        else if(rb.position.y > highY)
        {
            isUp = false;
            rb.velocity = new Vector2(rb.position.x, flyForce / 2);
        }
        
    }



    //用于改变Eagle朝向，保持始终对着玩家
    //void ChangeFacing()
    //{

    //}

}
