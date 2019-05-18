using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{
    //角色在漫游的场景
    public GameObject playerInWaitScene;
    //角色在漫游场景的出生位置
    public Transform[] playerSpawnPos;
    //玩家的数量
    private int playerNums;
    //是否开始倒计时
    private bool timeGo;
    //游戏开始倒计时时间
    private float timeStart;
    //用来提示游戏开始的倒计时文字
    public Text gameStartText;
    //加载进度条
    public Slider loadSlider;

    private void Start()
    {
        //开始生成一个角色
        Instantiate(playerInWaitScene, playerSpawnPos[Random.Range(0, 5)].position, Quaternion.identity);
        timeStart = 10f;
        gameStartText.gameObject.SetActive(false);
        StartCoroutine("LoadGame");
    }

    private void Update()
    {
        playerNums = PhotonNetwork.CurrentRoom.PlayerCount;
        //如果此时场景的玩家数量大于2则开始游戏倒计时
        if (playerNums >= 0)
        {
            timeGo = true;
            if (timeGo == true)
            {               
                gameStartText.gameObject.SetActive(true);
                timeStart -= Time.deltaTime;
                gameStartText.text = "游戏开始时间：" + ((int)timeStart).ToString();
            }
        }
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(10f);
        PhotonNetwork.LoadLevel(2);
    }
}
