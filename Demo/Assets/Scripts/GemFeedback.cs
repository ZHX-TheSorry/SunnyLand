using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemFeedback : MonoBehaviour
{
    public void Death()
    {
        //Ѱ�Ҳ�����player�ĺ���
        FindObjectOfType<PlayerController>().GemCount();

        Destroy(gameObject);
    }
}
