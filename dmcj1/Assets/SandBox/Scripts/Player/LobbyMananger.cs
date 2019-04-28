using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyMananger : MonoBehaviourPunCallbacks
{
    private byte maxPlayers = 4;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMatch()
    {
        //创建房间按钮
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayers }, null, null);
        }
    }

    public override void OnJoinedRoom()
    {
        //加载游戏场景
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }
}
