using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPool : MonoBehaviour
{
    public static ShadowPool instance;

    public GameObject shadowPrefab;     //创建一个残影对象预制体

    public int shadowCount; //对象数量控制

    private Queue<GameObject> availableObjects = new Queue<GameObject>();   //创建对象队列
    private void Awake()
    {
        instance = this;

        FillPool();
    }


    //初始化对象池
    public void FillPool()
    {
        for(int i = 0; i < 10; i++)
        {
            //生成预制体
            var newShadow = Instantiate(shadowPrefab);
            newShadow.transform.SetParent(transform);   //放入shadowPool之下

            //取消启用（取消激活状态），放入对象池
            ReturnPool(newShadow);

        }
    }


    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        availableObjects.Enqueue(gameObject);   //入队
    }


       public GameObject GetFromPool()
    {
        if (availableObjects.Count == 0 )
        {
            FillPool();     //若已有对象数量用完，则再填充对象池
        }
        var outShadow = availableObjects.Dequeue();   //出队
        outShadow.SetActive(true);  //激活

        return outShadow;
    }
}
