using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;       //player位置
    private SpriteRenderer thisSprite;  //当前残影
    private SpriteRenderer playerSprite;    //用于获取player当前动作图片

    private Color color;

    public float activeTime;    //残影显示的时间
    public float activeStart;   //开始显示的时间点

    private float alpha;    //透明度
    public float alphaSet;  //初始值
    public float alphaMultiplier;   //淡化速度


    //被启用时执行
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;
    }


    // Update is called once per frame
    void Update()
    {
        alpha *= alphaMultiplier;

        color = new Color(0.5f, 0.5f, alpha);

        thisSprite.color = color;

        if (Time.time >= activeStart + activeTime)
        {
            //返回对象池
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}
