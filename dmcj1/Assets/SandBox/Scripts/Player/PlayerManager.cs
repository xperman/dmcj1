using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    #region private
    private static PhotonView pv;
    #endregion
    #region public
    //自己看不见的对象
    public GameObject[] myself;
    //别人看不见的对象
    public GameObject[] others;
    public Camera myCamera;
    //自己可以控制的脚本
    public Behaviour[] scriptsController;
    #endregion
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            for (int i = 0; i < myself.Length; i++)
            {
                myself[i].SetActive(false);
            }
        }
        if (!pv.IsMine)
        {
            myCamera.enabled = false;
            for (int i = 0; i < scriptsController.Length; i++)
            {
                scriptsController[i].enabled = false;
            }
            for (int i = 0; i < others.Length; i++)
            {
                others[i].SetActive(false);
            }
        }
    }
}
