﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CanPickObject : MonoBehaviour, IPunObservable
{
    //是否被销毁
    private bool isHide;

    private void Start()
    {
        //否
        isHide = false;
    }
    private void Update()
    {
        //如果是被销毁，再场景中销毁
        if(isHide==true)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    [PunRPC]
    public void DestoryThisObject()
    {
        isHide = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //同步状态
        if (stream.IsWriting)
        {
            stream.SendNext(isHide);
        }
        else
        {
            this.isHide = (bool)stream.ReceiveNext();
        }
    }
}