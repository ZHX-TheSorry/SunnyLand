using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform Camera;
    public float moveRate;  //移动速率差
    private float startPointX;
    private float startPointY;

    public bool lockY;


    // Start is called before the first frame update
    void Start()
    {
        startPointX = transform.position.x;  //获取当前（初始）位置
    }

    // Update is called once per frame
    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(startPointX + Camera.position.x * moveRate, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(startPointX + Camera.position.x * moveRate, startPointY + Camera.position.y * moveRate);
        }
        
    }
}
