using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SandBox
{
    public class GameBegin : MonoBehaviourPunCallbacks
    {
        //登陆Button
        public Button Login;
        //创建房间Button
        public Button CreatRoom;
        //加入房间Button
        public Button JoinRoom;
        //是否我们准备好加入一个房间
        private bool joinRoomIs;
        //登陆成功后的面板
        public GameObject nextPanel;

        private void Start()
        {
            //保持让所有的玩家的加载场景为同一个级别
            PhotonNetwork.AutomaticallySyncScene = true;

            //如过连不上，就用Pun自带的默认连接配置文件来连接 
            Login.onClick.AddListener(() => { PhotonNetwork.ConnectUsingSettings(); });
            //创建一个房间
            CreatRoom.onClick.AddListener(() => { PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 10 }, null, null); });
            //加入一个随机的房间
            JoinRoom.onClick.AddListener(() => { PhotonNetwork.JoinRandomRoom(); });
        }

        /// <summary>
        /// 在成功连接服务器后调用
        /// </summary>
        public override void OnConnectedToMaster()
        {
            joinRoomIs = true;
            nextPanel.SetActive(true);
        }

        /// <summary>
        /// 成功加入房间后调用
        /// </summary>
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("SandBox");
        }

        /// <summary>
        /// 在加入房间失败后调用
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("加入房间失败");
        }

        /// <summary>
        /// 在成功创建房间后调用
        /// </summary>
        public override void OnCreatedRoom()
        {
            Debug.Log("创建房间成功");
        }
    }
}
