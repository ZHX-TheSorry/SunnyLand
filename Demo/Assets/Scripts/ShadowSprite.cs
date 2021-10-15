using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;       //playerλ��
    private SpriteRenderer thisSprite;  //��ǰ��Ӱ
    private SpriteRenderer playerSprite;    //���ڻ�ȡplayer��ǰ����ͼƬ

    private Color color;

    public float activeTime;    //��Ӱ��ʾ��ʱ��
    public float activeStart;   //��ʼ��ʾ��ʱ���

    private float alpha;    //͸����
    public float alphaSet;  //��ʼֵ
    public float alphaMultiplier;   //�����ٶ�


    //������ʱִ��
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
            //���ض����
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}
