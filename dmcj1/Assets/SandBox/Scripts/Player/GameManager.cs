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
    //飞机
    public GameObject airplane;
    //飞机进场位置
    public Transform airplaneStartPos;
    //地图
    public GameObject map;

    public Button startPos1;
    public Button startPos2;
    public Button startPos3;
    public Button startPos4;
    public Button startPos5;
    public Button startPos6;
    public Button startPos7;

    public Transform[] posA;
    public Transform[] posB;
    public Transform[] posC;
    public Transform[] posD;
    public Transform[] posE;
    public Transform[] posF;
    public Transform[] posG;

    void Start()
    {
        startPos1.onClick.AddListener(() =>
        {
            PhotonNetwork.Instantiate(playerPrefab.name, posA[Random.Range(0, 5)].position, Quaternion.identity, 0);
            startPos1.GetComponentInChildren<Text>().text = "已选定A区域";
        });
        startPos2.onClick.AddListener(() =>
        {
            PhotonNetwork.Instantiate(playerPrefab.name, posB[Random.Range(0, 5)].position, Quaternion.identity, 0);
            startPos2.GetComponentInChildren<Text>().text = "已选定B区域";
        });
        startPos3.onClick.AddListener(() =>
        {
            PhotonNetwork.Instantiate(playerPrefab.name, posC[Random.Range(0, 5)].position, Quaternion.identity, 0);
            startPos3.GetComponentInChildren<Text>().text = "已选定C区域";
        });
        startPos4.onClick.AddListener(() =>
        {
            PhotonNetwork.Instantiate(playerPrefab.name, posD[Random.Range(0, 5)].position, Quaternion.identity, 0);
            startPos4.GetComponentInChildren<Text>().text = "已选定D区域";
        });
        startPos5.onClick.AddListener(() =>
        {
            PhotonNetwork.Instantiate(playerPrefab.name, posE[Random.Range(0, 5)].position, Quaternion.identity, 0);
            startPos5.GetComponentInChildren<Text>().text = "已选定E区域";
        });
        startPos6.onClick.AddListener(() =>
        {
            PhotonNetwork.Instantiate(playerPrefab.name, posF[Random.Range(0, 5)].position, Quaternion.identity, 0);
            startPos6.GetComponentInChildren<Text>().text = "已选定F区域";
        });
        startPos7.onClick.AddListener(() =>
        {
            PhotonNetwork.Instantiate(playerPrefab.name, posG[Random.Range(0, 5)].position, Quaternion.identity, 0);
            startPos7.GetComponentInChildren<Text>().text = "已选定G区域";
        });
    }
    public void InstantiatePlayers()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, airplane.transform.position, Quaternion.identity, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            map.SetActive(true);
        }

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
    /// 当玩家离开房间时调用
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("有一位玩家离开房间" + otherPlayer.NickName);
    }
}
