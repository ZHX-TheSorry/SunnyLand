using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryFeedback : MonoBehaviour
{
    public void Death()
    {
        //Ѱ�Ҳ�����player�ĺ���
        FindObjectOfType<PlayerController>().CherryCount();
        
        Destroy(gameObject);
    }

}
