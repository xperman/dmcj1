using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlaySpawn : MonoBehaviour
{
    private PhotonView pv;
    //飞机
    public GameObject plane;
    //用于飞机的摄像机
    public GameObject planeCamera;
    //飞机的速度
    public float planeSpeed;
    //角色是否在座位上
    private bool onSeat;
    //角色
    public GameObject player;

    public GameObject[] s;
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        onSeat = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if(!pv.IsMine)
        {
            for(int i=0;i<s.Length;i++)
            {
                s[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        if (pv.IsMine == true)
        {
            if (onSeat == true)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    onSeat = false;
                    Instantiate(player, plane.transform.position, Quaternion.identity);                   
                }
            }
        }
    }

    public void SpawnPlayer()
    {
        Instantiate(player, plane.transform.position, Quaternion.identity);
    }
}
