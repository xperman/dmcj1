﻿using System.Collections;
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
    public Material[] playerSkins;
    //玩家出生点随机
    public Transform[] spawnPositions;
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
    public Button startPos8;
    public Button startPos9;

    public Button hideMap;
    public Button displayMap;

    public Transform[] posA;
    public Transform[] posB;
    public Transform[] posC;
    public Transform[] posD;
    public Transform[] posE;
    public Transform[] posF;
    public Transform[] posG;
    public Transform[] posH;
    public Transform[] posI;

    public Button gameStart;
    private int areaNum;
    private Vector3 applyPos;
    public Text gameTimeDown;
    private float timeLast = 3;
    public Animator mapAnimator;

    public Text enterGameText;

    void Start()
    {
        gameStart.enabled = false;
        startPos1.onClick.AddListener(() =>
        {
            applyPos = posA[Random.Range(0, 5)].position;
            areaNum = 1;
            startPos1.GetComponentInChildren<Text>().text = "已选定城市区";
        });
        startPos2.onClick.AddListener(() =>
        {
            applyPos = posB[Random.Range(0, 5)].position;
            areaNum = 2;

            startPos2.GetComponentInChildren<Text>().text = "已选定南部绿地";
        });
        startPos3.onClick.AddListener(() =>
        {
            applyPos = posC[Random.Range(0, 5)].position;
            areaNum = 3;

            startPos3.GetComponentInChildren<Text>().text = "已选定军火商";
        });
        startPos4.onClick.AddListener(() =>
        {
            applyPos = posD[Random.Range(0, 5)].position;
            areaNum = 4;

            startPos4.GetComponentInChildren<Text>().text = "已选定工业地区";
        });
        startPos5.onClick.AddListener(() =>
        {
            applyPos = posE[Random.Range(0, 5)].position;
            areaNum = 5;

            startPos5.GetComponentInChildren<Text>().text = "已选定南部无人区";
        });
        startPos6.onClick.AddListener(() =>
        {
            applyPos = posF[Random.Range(0, 5)].position;
            areaNum = 6;

            startPos6.GetComponentInChildren<Text>().text = "已选定极北荒原";
        });
        startPos7.onClick.AddListener(() =>
        {
            applyPos = posG[Random.Range(0, 5)].position;
            areaNum = 7;

            startPos7.GetComponentInChildren<Text>().text = "已选定农业区";
        });
        startPos8.onClick.AddListener(() =>
        {
            applyPos = posH[Random.Range(0, 5)].position;
            areaNum = 8;

            startPos7.GetComponentInChildren<Text>().text = "已选定暴乱之地";
        });
        startPos9.onClick.AddListener(() =>
        {
            applyPos = posI[Random.Range(0, 5)].position;
            areaNum = 9;
            startPos7.GetComponentInChildren<Text>().text = "已选定北部无人区";
        });
        hideMap.onClick.AddListener(() =>
        {
            mapAnimator.SetBool("Hide", true);
            hideMap.gameObject.SetActive(false);
            displayMap.gameObject.SetActive(true);
        });
        displayMap.onClick.AddListener(() =>
        {
            mapAnimator.SetBool("Hide", false); displayMap.gameObject.SetActive(false);
            hideMap.gameObject.SetActive(true);
        });
    }
    public void InstantiatePlayers()
    {
        switch (areaNum)
        {
            case 1:
                GameObject tempPlayer = PhotonNetwork.Instantiate(playerPrefab.name, applyPos, Quaternion.identity, 0);
                tempPlayer.GetComponentInChildren<SkinnedMeshRenderer>().material = playerSkins[Random.Range(0, 4)];
                enterGameText.text = "城市区";
                break;
            case 2:
                GameObject tempPlayer2 = PhotonNetwork.Instantiate(playerPrefab.name, applyPos, Quaternion.identity, 0);
                tempPlayer2.GetComponentInChildren<SkinnedMeshRenderer>().material = playerSkins[Random.Range(0, 4)];
                enterGameText.text = "南部绿地";
                break;
            case 3:
                GameObject tempPlayer3 = PhotonNetwork.Instantiate(playerPrefab.name, applyPos, Quaternion.identity, 0);
                tempPlayer3.GetComponentInChildren<SkinnedMeshRenderer>().material = playerSkins[Random.Range(0, 4)];
                enterGameText.text = "军火商";
                break;
            case 4:
                GameObject tempPlayer4 = PhotonNetwork.Instantiate(playerPrefab.name, applyPos, Quaternion.identity, 0);
                tempPlayer4.GetComponentInChildren<SkinnedMeshRenderer>().material = playerSkins[Random.Range(0, 4)];
                enterGameText.text = "工业地区";
                break;
            case 5:
                GameObject tempPlayer5 = PhotonNetwork.Instantiate(playerPrefab.name, applyPos, Quaternion.identity, 0);
                tempPlayer5.GetComponentInChildren<SkinnedMeshRenderer>().material = playerSkins[Random.Range(0, 4)];
                enterGameText.text = "南部无人区";
                break;
            case 6:
                GameObject tempPlayer6 = PhotonNetwork.Instantiate(playerPrefab.name, applyPos, Quaternion.identity, 0);
                tempPlayer6.GetComponentInChildren<SkinnedMeshRenderer>().material = playerSkins[Random.Range(0, 4)];
                enterGameText.text = "极北荒原";
                break;
            case 7:
                GameObject tempPlayer7 = PhotonNetwork.Instantiate(playerPrefab.name, applyPos, Quaternion.identity, 0);
                tempPlayer7.GetComponentInChildren<SkinnedMeshRenderer>().material = playerSkins[Random.Range(0, 4)];
                enterGameText.text = "农业区";
                break;
            case 8:
                GameObject tempPlayer8 = PhotonNetwork.Instantiate(playerPrefab.name, applyPos, Quaternion.identity, 0);
                tempPlayer8.GetComponentInChildren<SkinnedMeshRenderer>().material = playerSkins[Random.Range(0, 4)];
                enterGameText.text = "暴乱之地";
                break;
            case 9:
                GameObject tempPlayer9 = PhotonNetwork.Instantiate(playerPrefab.name, applyPos, Quaternion.identity, 0);
                tempPlayer9.GetComponentInChildren<SkinnedMeshRenderer>().material = playerSkins[Random.Range(0, 4)];
                enterGameText.text = "北部无人区";
                break;
        }
        gameStart.gameObject.SetActive(false);
        StartCoroutine("hideEnterGameText");
    }

    private void Update()
    {
        timeLast -= Time.deltaTime;
        gameTimeDown.text = "游戏开始:" + (int)timeLast;
        while (timeLast <= 0)
        {
            gameStart.enabled = true;
            gameTimeDown.text = "进入游戏";
            gameTimeDown.GetComponent<Animator>().SetBool("enter", true);
            break;
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

    IEnumerator hideEnterGameText()
    {
        yield return new WaitForSeconds(2f);
        enterGameText.gameObject.SetActive(false);
    }
}