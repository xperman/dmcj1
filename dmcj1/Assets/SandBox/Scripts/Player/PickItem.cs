using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PickItem : MonoBehaviour
{
    #region private
    private Vector3 rayPos;
    private PhotonView pv;
    //绷带数量
    private int bandageAmount;
    //饮料数量
    private int drinkAmount;
    #endregion
    #region public
    //绷带的数量在背包中
    public Text bandageText;
    //饮料的数量在背包中
    public Text drinkText;
    public Camera myCamera;
    public Text itemText;
    //头盔
    public GameObject helmet;
    //防弹衣
    public GameObject armor;
    //背包
    public GameObject backpack;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        bandageAmount = 0;
        drinkAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            AroundWeapons();
            bandageText.text = bandageAmount.ToString();
            drinkText.text = drinkAmount.ToString();
        }
    }

    public void AroundWeapons()
    {
        rayPos = myCamera.ViewportToWorldPoint(new Vector3(0f, 5f, 0f));
        Ray ray = new Ray(rayPos, myCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3, 1 << 12))
        {
            itemText.gameObject.SetActive(true);
            switch (hit.collider.gameObject.tag)
            {
                case "helmet":
                    itemText.text = " F 拾取头盔";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        //仅远程玩家可以看到头盔
                        pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                    }
                    break;
                case "armor":
                    itemText.text = " F 拾取防弹衣";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                    }
                    break;
                case "Bandage":
                    itemText.text = " F 拾取绷带";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                        Debug.Log("已经拾取了绷带");
                        //背包绷带数加一
                        bandageAmount++;
                    }
                    break;
                case "Backpack":
                    itemText.text = " F 拾取背包";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                    }
                    break;
                case "Drink":
                    itemText.text = " F 拾取饮料";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                        drinkAmount++;
                    }
                    break;
            }
        }
        else
        {
            itemText.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    private void RemovePlayerPickItem(string item)
    {
        if (item == "helmet")
        {
            helmet.SetActive(true);
        }  
        else if(item=="Backpack")
        {
            backpack.SetActive(true);
        }
        else if(item== "armor")
        {
            armor.SetActive(true);
        }
    }
}
