using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetWorkManager : MonoBehaviourPunCallbacks
{
    //创建和加入房间的面板
    public GameObject creatOrJoinPanel;
    //创建房间
    public Button creatButton;
    //房间列表
    public Button roomListButton;
    //退出游戏
    public Button quitGameButton;
    //连接信息
    public Text debugLog;
    //大厅面板
    public GameObject lobbyPanel;
    //房间信息
    private RoomOptions roomOptions = new RoomOptions();
    //最大人数
    private byte maxPlayers = 6;
    //是否允许创建房间
    private bool isAllowCreatARoom;

    //called once on game begin
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        //需要等待客户端连接到服务器才可以创建房间
        isAllowCreatARoom = false;
        //创建一个房间
        creatButton.onClick.AddListener(() =>
        {
            if (isAllowCreatARoom == true)
            {
                PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayers }, null, null);
            }
        });

        //退出游戏
        quitGameButton.onClick.AddListener(() =>
        {
            //断开连接
            PhotonNetwork.Disconnect();
            //退出游戏
            Application.Quit();
        });

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //加入一个随机的房间
    public void JoinARoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    //绑定登陆的按钮
    public void StartMatching()
    {
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    //加入随机房间时回调
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        debugLog.text += "加入随机房间失败 : " + message;
        Debug.Log("加入随机房间失败 : " + message);
    }

    //成功创建房间时调用
    public override void OnCreatedRoom()
    {
        debugLog.text = "创建房间成功";
        Debug.Log("创建房间成功");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        debugLog.text += "创建房间失败";
        Debug.Log("创建房间失败");
    }

    public override void OnConnectedToMaster()
    {
        //如果此客户端连接到了服务器才可以进行创建房间
        isAllowCreatARoom = true;
        lobbyPanel.SetActive(true);
        Debug.Log("IsMasterClient   +   " + PhotonNetwork.IsMasterClient);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("加入房间");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int number = PhotonNetwork.PlayerList.Length;
        Debug.Log("此房间内的玩家人数  ： " + number);
        debugLog.text += "此房间内的玩家人数  ： " + number;
    }
}
