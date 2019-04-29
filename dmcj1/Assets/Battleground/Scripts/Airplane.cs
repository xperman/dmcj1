using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    [Header("Airplane")]
    //飞行距离
    public float FlyDistance = 1000;
    //喷射距离
    public float EjectDistance = 500;
    //飞行速度
    public float Speed = 20;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.Translate(Vector3.forward *Speed * Time.deltaTime);
    }
}
