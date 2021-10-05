using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryFeedback : MonoBehaviour
{
    public void Death()
    {
        //寻找并调用player的函数
        FindObjectOfType<PlayerController>().CherryCount();
        
        Destroy(gameObject);
    }

}
