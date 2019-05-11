using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class UseItem : MonoBehaviour
{
    ////背包面板
    //public GameObject backpackPanel;
    ////绷带按钮
    //public Button bandageButton;
    ////饮料按钮
    //public Button drinkButton;
    ////使用道具时间显示
    //public Text useItemTime;
    //private float useTimeScale=3f;
    //private bool isUseItem;
    ////加血动画
    //public Animator removePlayer;
    ////加血时间显示的图片
    //public GameObject useItemImage;
    //public AnimatorStateInfo myStateInfo;
    //private PhotonView pv;


    //// Start is called before the first frame update
    //void Start()
    //{
    //    pv = this.GetComponent<PhotonView>();
    //    //使用绷带
    //    bandageButton.onClick.AddListener(() =>
    //    {
    //        //加血持续时间为3秒
    //        StartCoroutine("AddHealthTime");
    //    });
    //    //使用饮料
    //    drinkButton.onClick.AddListener(() =>
    //    {
    //        //加血持续时间为3秒
    //        StartCoroutine("useDrinkTime");
    //    });
    //}

    //void Update()
    //{
    //    if (pv.IsMine)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Tab))
    //        {
    //            backpackPanel.SetActive(true);
    //        }
    //        if (Input.GetKeyUp(KeyCode.Tab))
    //        {
    //            backpackPanel.SetActive(false);
    //        }
    //        if (Input.GetKeyDown(KeyCode.Alpha0))
    //        {
    //            removePlayer.SetTrigger("UseBandage");
    //            isUseItem = true;
    //            //加血持续时间为3秒
    //            StartCoroutine("AddHealthTime");
    //        }          
    //        if(isUseItem==true)
    //        {
    //            useItemImage.SetActive(true);
    //            useTimeScale -= Time.deltaTime;
    //            useItemTime.text = ((int)useTimeScale).ToString();
    //            if(useTimeScale<=0)
    //            {
    //                isUseItem = false;
    //                useItemImage.SetActive(false);
    //            }
    //        }
    //        else
    //        {
    //            useTimeScale = 3;
    //            useItemTime.text = 3.ToString();
    //        }
    //    }
        
    //}

    //IEnumerator AddHealthTime()
    //{
    //    yield return new WaitForSeconds(3f);
    //    this.GetComponent<PhotonView>().RPC("AddHealth", RpcTarget.AllBuffered, 10);
    //}

    //IEnumerator useDrinkTime()
    //{
    //    yield return new WaitForSeconds(2f);
    //    this.GetComponent<PhotonView>().RPC("AddHealth", RpcTarget.AllBuffered, 5);
    //}
}
