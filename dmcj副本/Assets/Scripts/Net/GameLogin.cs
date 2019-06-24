using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameLogin : MonoBehaviourPunCallbacks
{
    string gameVersion = "1.0";
    [Header("大厅面板")]
    [SerializeField] private GameObject nextPanel;
    [Header("登录面板")]
    [SerializeField] private GameObject[] initalState;
    [Header("沙盒场景")]
    [SerializeField] private Button sandBox;
    [Header("战场场景")]
    [SerializeField] private Button battleground;
    [Header("加入房间Button")]
    [SerializeField] private Button joinRoom;
    [Header("选择地图面板")]
    [SerializeField] private GameObject seletePanel;
    [Header("当前房间内玩家数量Text")]
    [SerializeField] private Text currentPlayers;
    [Header("退出游戏button")]
    [SerializeField] private Button quitGame;
    //房间的属性
    RoomOptions roomOptions = new RoomOptions();

    //现在是否准备可以加入一个房间
    private bool joinRoomIs;

    private bool roomIs;

    private int mapNums = 0;

    private void Start()
    {
        Debug.Log("开始");
        //所有玩家加载级别一样
        PhotonNetwork.AutomaticallySyncScene = true;
        //在游戏的一开始连接服务器
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = gameVersion;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        joinRoomIs = false;
        roomIs = false;
        //退出游戏
        quitGame.onClick.AddListener(() => { PhotonNetwork.Disconnect(); Application.Quit(); });
        //添加匹配事件
        battleground.onClick.AddListener(CreatRoom);
        //添加加入房间事件
        joinRoom.onClick.AddListener(JoinARoom);
    }

    private void Update()
    {
        if (roomIs == true)
        {
            currentPlayers.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + 20 + "当前的在线人数："
+ PhotonNetwork.CountOfPlayers;
        }
    }

    private void JoinARoom()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.LogError("存在的房间 ：" + PhotonNetwork.CountOfRooms);
    }

    private void CreatRoom()
    {
        if (joinRoomIs == true && PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.CreateRoom(null, roomOptions, null, null);
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 10 }, null, null);
            seletePanel.SetActive(true);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("成功创建一个房间");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        Debug.Log("加入一个房间失败");
    }


    public override void OnJoinedRoom()
    {
        roomIs = true;
        Debug.Log("成功加入了一个房间");
      //  PhotonNetwork.LoadLevel(1);
    }

    /// <summary>
    /// 当玩家连上服务器时调用
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("成功连接到服务器");
        //当玩家成功连接到服务器后可以加入房间
        joinRoomIs = true;
        //跳转大厅面板
        nextPanel.SetActive(true);
        //隐藏登录界面
        initalState[0].SetActive(false);
        //initalState[1].SetActive(false);
    }

    /// <summary>
    /// 当玩家进入房间时调用
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}
