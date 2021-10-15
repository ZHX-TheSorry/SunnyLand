using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;

    public GameObject shadowPrefab;     //����һ����Ӱ����Ԥ����

    public int shadowCount; //������������

    private Queue<GameObject> availableObjects = new Queue<GameObject>();   //�����������
    private void Awake()
    {
        instance = this;

        FillPool();
    }


    //��ʼ�������
    public void FillPool()
    {
        for(int i = 0; i < 10; i++)
        {
            //����Ԥ����
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);   //����shadowPool֮��

            //ȡ�����ã�ȡ������״̬������������
            ReturnPool(newShadow);

        }
    }


    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        availableObjects.Enqueue(gameObject);   //���
    }


       public GameObject GetFromPool()
    {
        if (availableObjects.Count == 0 )
        {
            FillPool();     //�����ж����������꣬�����������
        }
        var outShadow = availableObjects.Dequeue();   //����
        outShadow.SetActive(true);  //����

        return outShadow;
    }
}
