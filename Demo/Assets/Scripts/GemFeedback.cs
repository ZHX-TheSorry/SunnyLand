using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemFeedback : MonoBehaviour
{
    public void Death()
    {
        //寻找并调用player的函数
        FindObjectOfType<PlayerController>().GemCount();

        Destroy(gameObject);
    }
}
