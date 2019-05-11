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
    private PhotonView pv;

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
            this.GetComponent<UIManager>().bandageAmountText.text = "绷带 x " + bandageAmount.ToString();
            this.GetComponent<UIManager>().drinkAmountText.text = "饮料 x " + drinkAmount.ToString();
        }
    }

    public void AroundWeapons()
    {
        rayPos = myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Ray ray = new Ray(rayPos, myCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3, 1 << 12))
        {
            this.GetComponent<UIManager>().itemText.gameObject.SetActive(true);
            switch (hit.collider.gameObject.tag)
            {
                case "helmet":
                    this.GetComponent<UIManager>().itemText.text = "拾取头盔";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        //仅远程玩家可以看到头盔
                        pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                    }
                    break;
                case "armor":
                    this.GetComponent<UIManager>().itemText.text = "拾取防弹衣";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                    }
                    break;
                case "Bandage":
                    this.GetComponent<UIManager>().itemText.text = "拾取绷带";
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
                    this.GetComponent<UIManager>().itemText.text = "拾取背包";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                    }
                    break;
                case "Drink":
                    this.GetComponent<UIManager>().itemText.text = "拾取饮料";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                        drinkAmount++;
                    }
                    break;
                case "Barrett":
                    this.GetComponent<UIManager>().itemText.text = "拾取Barrett";
                    break;
                case "AK47":
                    this.GetComponent<UIManager>().itemText.text = "拾取AK47";
                    break;
                case "SMG":
                    this.GetComponent<UIManager>().itemText.text = "拾取SMG";
                    break;
                case "M4A1":
                    this.GetComponent<UIManager>().itemText.text = "拾取M4A1";
                    break;
                case "Shotgun":
                    this.GetComponent<UIManager>().itemText.text = "拾取Shotgun";
                    break;

                case "556":
                    this.GetComponent<UIManager>().itemText.text = "拾取5.56子弹";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        guns[0].GetComponentInChildren<Akm>().backupBullets = 60;
                        guns[1].GetComponentInChildren<Sinper>().backupBullets = 30;
                        guns[2].GetComponentInChildren<Scar>().backupBullets = 60;
                        guns[3].GetComponentInChildren<Lever>().backupBullets = 60;
                        guns[4].GetComponentInChildren<Smg>().backupBullets = 60;
                    }

                    break;
                case "762":

                    break;
                case "999":

                    break;
            }
        }
        else
        {
            this.GetComponent<UIManager>().itemText.text = null;
            this.GetComponent<UIManager>().itemText.gameObject.SetActive(false);
        }
    }

    public GameObject[] guns;

    [PunRPC]
    private void RemovePlayerPickItem(string item)
    {
        if (item == "helmet")
        {
            helmet.SetActive(true);
            this.GetComponent<UIManager>().helemet.gameObject.SetActive(true);
        }
        else if (item == "Backpack")
        {
            backpack.SetActive(true);
            this.GetComponent<UIManager>().backpage.gameObject.SetActive(true);
        }
        else if (item == "armor")
        {
            armor.SetActive(true);
            this.GetComponent<UIManager>().aromor.gameObject.SetActive(true);
        }
    }
}
