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
    private PhotonView pv;
    //复活时间
    private float resurrectionTime = 5;
    //判断死亡
    private bool dead;
    //public:
    //血条
    public Slider bloodSlider;
    //远程玩家的UI画布
    public Canvas canvas;
    public Canvas aimCanvas;
    //血粒子特效
    public GameObject blood;
    //伤害提示
    public GameObject damageDisplay;
    //死亡面板
    public GameObject diedView;
    //游戏结束后禁止玩家移动和射击
    public Behaviour[] stopBehaviour;
    //返回大厅按钮
    public Button backLobby;
    //返回游戏按钮
    public Button backGame;
    //返回操作的面板
    public GameObject backPanel;
    //复活时间显示文字
    public Text resurrectionText;
    //随机的复活点
    public Vector3 resurrectionPositions;
    //射击的脚本
    public Behaviour attackScript;
    //视角的脚本
    public Behaviour viewScript;
    //准星图片
    public Image aimImage;
    //用于渲染准星的摄像机
    public Camera aimCamera;
    //死亡音效
    public AudioClip diedAudio;
    public AudioSource humanAudio;

    private void Start()
    {
        pv = this.GetComponent<PhotonView>();
        aimCanvas.worldCamera = aimCamera;
        backLobby.onClick.AddListener(() =>
        {
            Cursor.lockState = CursorLockMode.None;
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(0);
        });
        backGame.onClick.AddListener(() =>
        {
            attackScript.enabled = true;
            viewScript.enabled = true;
            backPanel.SetActive(false);
            aimImage.gameObject.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        });
        if (!pv.IsMine)
        {
            //关掉除了自己以外的所有玩家的UI画布
            canvas.gameObject.SetActive(false);
            canvas.enabled = false;
            aimImage.gameObject.SetActive(false);
            aimImage.enabled = false;
        }
        dead = false;
        //记录玩家的出生位置
        resurrectionPositions = this.gameObject.transform.position;
    }

    void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //打开返回操作的面板
                backPanel.SetActive(true);              
                //禁止射击
                attackScript.enabled = false;
                //禁止改变视角
                viewScript.enabled = false;
                //关闭准星
                aimImage.gameObject.SetActive(false);
                //开启鼠标指针
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                
            }

            if (dead == true)
            {
                resurrectionText.gameObject.SetActive(true);
                resurrectionTime -= Time.deltaTime;
                resurrectionText.text = ((int)resurrectionTime).ToString();
            }

            if (resurrectionTime <= 0 && dead == true)
            {
                resurrectionText.gameObject.SetActive(false);
                dead = false;
                //将玩家送回出生点
                this.gameObject.transform.position = resurrectionPositions;
                //关闭死亡提示面板
                diedView.SetActive(false);
                //重生重新计时
                resurrectionTime = 5f;
                //恢复生命值
                pv.RPC("AddHealth", RpcTarget.AllBuffered, 100);
                //解除玩家的操作
                for (int i = 0; i < stopBehaviour.Length; i++)
                {
                    stopBehaviour[i].enabled =true;
                }
            }
        }
    }

    string killer = "world";
    /// <summary>
    /// 自身受到伤害
    /// </summary>
    [PunRPC]
    public void DamageGet(int dmg, string name, Vector3 hitPoint)
    {
        killer = name;
        bloodVolume -= dmg;
        bloodSlider.value = bloodVolume;
        Instantiate(blood, hitPoint, Quaternion.identity);
        Debug.Log("小萝卜");
        damageDisplay.SetActive(true);
        StartCoroutine("hideDamage");
        if (bloodSlider.value <= 0)
        {
            Debug.Log("you died");
            pv.RPC("Died", RpcTarget.AllBuffered);
            pv.RPC("DiedAnimator", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void AddHealth(int amt)
    {
        bloodVolume += amt;
        bloodSlider.value = bloodVolume;
    }

    [PunRPC]
    public void Died()
    {
        diedView.SetActive(true);
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
        damageDisplay.SetActive(false);
    }
}
