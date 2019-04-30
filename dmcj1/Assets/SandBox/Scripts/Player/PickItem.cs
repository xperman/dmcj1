using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PickItem : MonoBehaviour
{
    #region private
    private Vector3 rayPos;
    //绷带数量
    private int bandageAmount;
    //饮料数量
    private int drinkAmount;
    #endregion
    #region public
    public Camera myCamera;
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
        bandageAmount = 0;
        drinkAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.pv.IsMine)
        {
            AroundWeapons();
            UIManager.Instance.bandageAmountText.text = bandageAmount.ToString();
            UIManager.Instance.drinkAmountText.text = drinkAmount.ToString();
        }
    }

    public void AroundWeapons()
    {
        rayPos = myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Ray ray = new Ray(rayPos, myCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3, 1 << 12))
        {
            UIManager.Instance.itemText.gameObject.SetActive(true);
            switch (hit.collider.gameObject.tag)
            {
                case "helmet":
                    UIManager.Instance.itemText.text = "拾取头盔";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        //仅远程玩家可以看到头盔
                        PlayerManager.pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                    }
                    break;
                case "armor":
                    UIManager.Instance.itemText.text = "拾取防弹衣";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        PlayerManager.pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                    }
                    break;
                case "Bandage":
                    UIManager.Instance.itemText.text = "拾取绷带";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        PlayerManager.pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                        Debug.Log("已经拾取了绷带");
                        //背包绷带数加一
                        bandageAmount++;
                    }
                    break;
                case "Backpack":
                    UIManager.Instance.itemText.text = "拾取背包";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        PlayerManager.pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                    }
                    break;
                case "Drink":
                    UIManager.Instance.itemText.text = "拾取饮料";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        PlayerManager.pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                        drinkAmount++;
                    }
                    break;
            }
        }
        else
        {
            UIManager.Instance.itemText.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    private void RemovePlayerPickItem(string item)
    {
        if (item == "helmet")
        {
            helmet.SetActive(true);
        }
        else if (item == "Backpack")
        {
            backpack.SetActive(true);
        }
        else if (item == "armor")
        {
            armor.SetActive(true);
        }
    }
}
