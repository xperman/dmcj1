using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NetWorkManager : MonoBehaviourPunCallbacks
{
    //创建和加入房间的面板
    public GameObject creatOrJoinPanel;
    //创建房间
    public Button creatButton;
    //退出游戏
    public Button quitGameButton;
    //连接信息
    public Text debugLog;
    //大厅面板
    public GameObject lobbyPanel;
    //房间信息
    private RoomOptions roomOptions = new RoomOptions();
    //最大人数
    private byte maxPlayers = 10;
    //是否允许创建房间
    private bool isAllowCreatARoom;

    public Animator matchInformation;

    public AudioSource click;

    //玩家头像
    public Image[] headPortrait;

    public GameObject portraitPanel;

    public InputField yourName;
    public Button enterYourName;

    //called once on game begin
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = "无名之辈";
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
        enterYourName.onClick.AddListener(() => { PhotonNetwork.NickName = yourName.text; });
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
    private void Update()
    {
        int number = PhotonNetwork.PlayerList.Length;
        for (int i = 0; i < number; i++)
        {
            headPortrait[i].gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 加入一个随机房间
    /// </summary>
    public void JoinARoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    /// <summary>
    /// 绑定登录按钮
    /// </summary>
    public void StartMatching()
    {
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// 加入随机房间时回调
    /// </summary>
    /// <param name="returnCode">失败代号</param>
    /// <param name="message">失败信息</param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        debugLog.text += "加入随机房间失败 : " + message;
        Debug.Log("加入随机房间失败 : " + message);
        matchInformation.SetBool("display", true);
        StartCoroutine("HideMatchInfromation");
        click.Play();
    }

    /// <summary>
    /// 成功创建房间时调用
    /// </summary>
    public override void OnCreatedRoom()
    {
        debugLog.text = "创建房间成功";
        Debug.Log("创建房间成功");
    }

    /// <summary>
    /// 创建房间失败时
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        debugLog.text += "创建房间失败";
        Debug.Log("创建房间失败");
    }

    /// <summary>
    /// 连接服务器时调用
    /// </summary>
    public override void OnConnectedToMaster()
    {
        //如果此客户端连接到了服务器才可以进行创建房间
        isAllowCreatARoom = true;
        lobbyPanel.SetActive(true);
        Debug.Log("IsMasterClient   +   " + PhotonNetwork.IsMasterClient);
    }

    /// <summary>
    /// 加入房间是调用
    /// </summary>
    public override void OnJoinedRoom()
    {
        portraitPanel.SetActive(true);
        PhotonNetwork.LoadLevel(1);
    }
    /// <summary>
    /// 进入房间时调用
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int number = PhotonNetwork.PlayerList.Length;
        portraitPanel.SetActive(true);
        //if (number >= 0)
        //{
        //    PhotonNetwork.LoadLevel(1);
        //}
        debugLog.text += "此房间内的玩家人数  ： " + number;
    }

    /// <summary>
    /// 隐藏弹出的信息
    /// </summary>
    /// <returns></returns>
    IEnumerator HideMatchInfromation()
    {
        yield return new WaitForSeconds(2f);
        matchInformation.SetBool("display", false);
    }
}
