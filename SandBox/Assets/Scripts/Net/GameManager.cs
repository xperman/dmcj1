using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace SandBox
{
    public class GameManager : MonoBehaviour
    {
        //角色预制体
        public GameObject playerPrefabs;

        void Start()
        {
            //进入房间就会生成一个角色
            PhotonNetwork.Instantiate(playerPrefabs.name, Vector3.zero, Quaternion.identity, 0);
        }
    }
}
