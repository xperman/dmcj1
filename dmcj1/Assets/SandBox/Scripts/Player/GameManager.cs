using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    //玩家预设。
    public GameObject playerPrefab;
    //玩家出生点随机
    public Transform[] spawnPositions;

    void Start()
    {
        InstantiatePlayers();
    }
    public void InstantiatePlayers()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPositions[Random.Range(1, 6)].position, Quaternion.identity, 0);
    }
    /// <summary>
    /// 玩家离开房间时调用
    /// </summary>
    public override void OnLeftRoom()
    {
        //如果玩家退出房间，则返回到登陆界面。
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// 当玩家进入房间时调用
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //统计玩家的数量
        Debug.Log(newPlayer.IsLocal);
        Debug.Log(newPlayer.NickName);
    }
    /// <summary>
    /// 当加载一个新场景时调用
    /// </summary>
    /// <param name="level"></param>
    public void OnLevelWasLoaded(int level)
    {

    }

    /// <summary>
    /// 当玩家离开房间时调用
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("有一位玩家离开房间" + otherPlayer.NickName);
    }
}
