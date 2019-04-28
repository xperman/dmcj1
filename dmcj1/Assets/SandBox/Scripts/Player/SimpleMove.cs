﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;


public class SimpleMove : MonoBehaviourPun
{
    #region private
    private PhotonView pv;
    #endregion
    #region public
    //远程玩家眼里的完整模型
    public Animator anmRemove;

    public Animator[] anmGuns;
    //最大跳跃力
    public float maxJumpForce;
    //角色最大行进速度
    public float walkMaxSpeed;
    #endregion

    void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }
    public void Update()
    {
        if (pv.IsMine)
        {
            RoleJump();
            RoleCharacterControl();
        }
    }

    private void RoleCharacterControl()
    {
        anmRemove.SetFloat("Velocity", Input.GetAxis("Vertical")); //这个只会在远程玩家眼里看到      

        if (Input.GetKeyDown(KeyCode.Z))
        {
            anmRemove.SetInteger("BodyState", 2); //趴下
            //playerLocal.transform.position = standState.position;
        }

        if (Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.Z))
        {
            anmRemove.SetInteger("BodyState", 0); //站起
                                                  // playerLocal.transform.position = standState.position;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            anmRemove.SetInteger("BodyState", 1);//蹲下
                                                 // playerLocal.transform.position = squatState.position;
        }

        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            anmRemove.SetFloat("Velocity", 4); //行走
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            anmRemove.SetFloat("SideVelocity", Input.GetAxis("Horizontal")); //行走  
        }

        if (Input.GetAxis("Horizontal") != 0 && Input.GetKeyDown(KeyCode.LeftShift))
        {
            anmRemove.SetFloat("Velocity", 7);
        }
    }
    /// <summary>
    /// 控制角色跳跃
    /// </summary>
    private void RoleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //currentGun.SetTrigger("Jump");
            anmRemove.SetTrigger("Jump");
        }
    }


}