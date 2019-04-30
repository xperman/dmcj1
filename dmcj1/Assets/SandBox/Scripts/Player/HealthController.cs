using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    //private:
    //100滴血
    private int bloodVolume = 100;
    //复活时间
    private float resurrectionTime = 5;
    //判断死亡
    private bool dead;
    //public:
    //血粒子特效
    public GameObject blood;
    //游戏结束后禁止玩家移动和射击
    public Behaviour[] stopBehaviour;
    //随机的复活点
    public Vector3 resurrectionPositions;
    //射击的脚本
    public Behaviour attackScript;
    //视角的脚本
    public Behaviour viewScript;
    //用于渲染准星的摄像机
    public Camera aimCamera;
    //死亡音效
    public AudioClip diedAudio;
    public AudioSource humanAudio;

    private void Start()
    {
        //监听返回大厅
        UIManager.Instance.backLobby.onClick.AddListener(() =>
        {
            Cursor.lockState = CursorLockMode.None;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(0);
        });
        //监听返回游戏
        UIManager.Instance.backGame.onClick.AddListener(() =>
        {
            attackScript.enabled = true;
            viewScript.enabled = true;
            UIManager.Instance.backPanel.gameObject.SetActive(false);
            UIManager.Instance.aimImage.gameObject.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        });

        if (!PlayerManager.pv.IsMine)
        {
            //关掉除了自己以外的所有玩家的UI画布
            for (int i = 0; i < UIManager.Instance.otherPlayerCanvas.Length; i++)
            {
                UIManager.Instance.otherPlayerCanvas[i].gameObject.SetActive(false);
                UIManager.Instance.otherPlayerCanvas[i].enabled = false;
                UIManager.Instance.aimImage.gameObject.SetActive(false);
                UIManager.Instance.aimImage.enabled = false;
            }
        }
        dead = false;
        //记录玩家的出生位置
        resurrectionPositions = this.gameObject.transform.position;
    }

    void Update()
    {
        if (PlayerManager.pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //禁止射击
                attackScript.enabled = false;
                //禁止改变视角
                viewScript.enabled = false;
                //打开返回操作的面板
                UIManager.Instance.backPanel.gameObject.SetActive(true);
                //关闭准星
                UIManager.Instance.aimImage.gameObject.SetActive(false);
                //开启鼠标指针
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (dead == true)
            {
                UIManager.Instance.resurrectionText.gameObject.SetActive(true);
                resurrectionTime -= Time.deltaTime;
                UIManager.Instance.resurrectionText.text = ((int)resurrectionTime).ToString();
            }

            if (resurrectionTime <= 0 && dead == true)
            {
                UIManager.Instance.resurrectionText.gameObject.SetActive(false);
                dead = false;
                //将玩家送回出生点
                this.gameObject.transform.position = resurrectionPositions;
                //关闭死亡提示面板
                UIManager.Instance.deadPanel.gameObject.SetActive(false);
                //重生重新计时
                resurrectionTime = 5f;
                //恢复生命值
                PlayerManager.pv.RPC("AddHealth", RpcTarget.AllBuffered, 100);
                //解除玩家的操作
                for (int i = 0; i < stopBehaviour.Length; i++)
                {
                    stopBehaviour[i].enabled = true;
                }
            }
        }
    }

    /// <summary>
    /// 自身受到伤害
    /// </summary>
    [PunRPC]
    public void DamageGet(int dmg, Vector3 hitPoint)
    {
        bloodVolume -= dmg;
        UIManager.Instance.healthSlider.value = bloodVolume;
        UIManager.Instance.damagePanel.SetActive(true);
        Instantiate(blood, hitPoint, Quaternion.identity);
        StartCoroutine("hideDamage");
        if (UIManager.Instance.healthSlider.value <= 0)
        {
            PlayerManager.pv.RPC("Died", RpcTarget.AllBuffered);
            PlayerManager.pv.RPC("DiedAnimator", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void AddHealth(int amt)
    {
        bloodVolume += amt;
        UIManager.Instance.healthSlider.value = bloodVolume;
    }

    [PunRPC]
    public void Died()
    {
        UIManager.Instance.deadPanel.gameObject.SetActive(true);
        //玩家死亡
        dead = true;
        //禁止玩家的一些操作
        for (int i = 0; i < stopBehaviour.Length; i++)
        {
            stopBehaviour[i].enabled = false;
        }
    }

    [PunRPC]
    public void DiedAnimator()
    {

        humanAudio.clip = diedAudio;
        humanAudio.Play();
    }

    IEnumerator hideDamage()
    {
        yield return new WaitForSeconds(0.2f);
        UIManager.Instance.damagePanel.SetActive(false);
    }
}
