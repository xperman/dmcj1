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
    //死亡音效
    public AudioClip diedAudio;
    public AudioSource humanAudio;
    private PhotonView pv;
    public Animator removePlayerAnimator;

    private void Awake()
    {
        //记录玩家的出生位置
        resurrectionPositions = this.gameObject.transform.position;
    }

    private void Start()
    {
        pv = this.GetComponent<PhotonView>();
        //监听返回大厅
        this.GetComponent<UIManager>().backLobby.onClick.AddListener(() =>
        {
            Cursor.lockState = CursorLockMode.None;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(0);
        });
        //监听返回游戏
        this.GetComponent<UIManager>().backGame.onClick.AddListener(() =>
        {
            attackScript.enabled = true;
            viewScript.enabled = true;
            this.GetComponent<UIManager>().backPanel.gameObject.SetActive(false);
            this.GetComponent<UIManager>().aimImage.gameObject.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        });

        if (!pv.IsMine)
        {
            //关掉除了自己以外的所有玩家的UI画布
            for (int i = 0; i < this.GetComponent<UIManager>().otherPlayerCanvas.Length; i++)
            {
                this.GetComponent<UIManager>().otherPlayerCanvas[i].gameObject.SetActive(false);
                this.GetComponent<UIManager>().otherPlayerCanvas[i].enabled = false;
                this.GetComponent<UIManager>().aimImage.gameObject.SetActive(false);
                this.GetComponent<UIManager>().aimImage.enabled = false;
            }
        }
        dead = false;

    }

    void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //禁止射击
                attackScript.enabled = false;
                //禁止改变视角
                viewScript.enabled = false;
                //打开返回操作的面板
                this.GetComponent<UIManager>().backPanel.gameObject.SetActive(true);
                //关闭准星
                this.GetComponent<UIManager>().aimImage.gameObject.SetActive(false);
                //开启鼠标指针
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            if (dead == true)
            {
                this.GetComponent<UIManager>().resurrectionText.gameObject.SetActive(true);
                resurrectionTime -= Time.deltaTime;
                this.GetComponent<UIManager>().resurrectionText.text = ((int)resurrectionTime).ToString();
            }

            if (resurrectionTime <= 0 && dead == true)
            {
                this.GetComponent<UIManager>().resurrectionText.gameObject.SetActive(false);
                dead = false;
                //将玩家送回出生点
                int t = Random.Range(1, 4);
                if (t == 1)
                {
                    this.gameObject.transform.position = pos1;
                }
                else if (t == 2)
                {
                    this.gameObject.transform.position = pos2;
                }
                else if (t == 3)
                {
                    this.gameObject.transform.position = pos3;
                }
                else if (t == 4)
                {
                    this.gameObject.transform.position = pos4;
                }
                //关闭死亡提示面板
                this.GetComponent<UIManager>().deadPanel.gameObject.SetActive(false);
                //重生重新计时
                resurrectionTime = 6f;
                //恢复生命值
                pv.RPC("AddHealth", RpcTarget.AllBuffered, 100);
                removePlayerAnimator.SetBool("Died", false);
                //解除玩家的操作
                for (int i = 0; i < stopBehaviour.Length; i++)
                {
                    stopBehaviour[i].enabled = true;
                }
            }
        }
    }

    private Vector3 pos1 = new Vector3(1030.177f, 91.99748f, 749.9025f);
    private Vector3 pos2 = new Vector3(809.2809f, 30.37775f, 598.5375f);
    private Vector3 pos3 = new Vector3(610.782f, 30.18041f, 1013.272f);
    private Vector3 pos4 = new Vector3(798.7405f, 33.40702f, 1432.018f);
    /// <summary>
    /// 自身受到伤害
    /// </summary>
    [PunRPC]
    public void DamageGet(int dmg, Vector3 hitPoint)
    {
        bloodVolume -= dmg;
        this.GetComponent<UIManager>().healthSlider.value = bloodVolume;
        this.GetComponent<UIManager>().damagePanel.SetActive(true);
        Instantiate(blood, hitPoint, Quaternion.identity);
        StartCoroutine("hideDamage");
        if (this.GetComponent<UIManager>().healthSlider.value <= 0)
        {
            pv.RPC("Died", RpcTarget.AllBuffered);
            pv.RPC("DiedAnimator", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void AddHealth(int amt)
    {
        bloodVolume += amt;
        this.GetComponent<UIManager>().healthSlider.value = bloodVolume;
    }

    [PunRPC]
    public void Died()
    {
        this.GetComponent<UIManager>().deadPanel.gameObject.SetActive(true);

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
        removePlayerAnimator.SetBool("Died", true);
        humanAudio.clip = diedAudio;
        humanAudio.Play();
    }

    IEnumerator hideDamage()
    {
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<UIManager>().damagePanel.SetActive(false);
    }
}
