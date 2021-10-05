using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    // Update is called once per frame
    void Update()
    {
        transform.position=new Vector3(player.position.x,player.position.y/3,-10);
    }
}
