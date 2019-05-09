using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    #region private
    public PhotonView pv;
    #endregion
    #region public
    //The scripts that yourself can't patch
    public GameObject[] myself;
    //The scripts that others can't patch
    public GameObject[] others;
    public Camera myCamera;
    //The scripts that you can control
    public Behaviour[] scriptsController;
    // View camera
    public Camera firstPersonCamera;
    #endregion
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        this.GetComponent<UIManager>().myName.text = PhotonNetwork.NickName;
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
