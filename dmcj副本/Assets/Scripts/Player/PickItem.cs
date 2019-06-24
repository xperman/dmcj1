using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//只控制拾取物品

public class PickItem : MonoBehaviour
{
    #region private
    private Vector3 rayPos;
    //绷带数量
    private int bandageAmount;
    //饮料数量
    private int drinkAmount;
    //急救包数量
    private int firstAidAmount;

    #endregion
    #region public
    public Camera myCamera;
    //头盔
    public GameObject helmet;
    //防弹衣
    public GameObject armor;
    //背包
    public GameObject backpack;

    public GameObject[] guns;

    private PhotonView pv;
    //所有的物品，包括枪
    public GameObject[] items;
    public Transform handgun1;
    public Transform handgun2;
    public Transform handAndGuns;
    private bool gun1State; //判断1号位是否有枪
    private bool gun2State; //判断1号位是否有枪
    public Animator hands;
    //远程玩家的枪
    public GameObject[] gunRemove;
    public GameObject handsMelle;
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
            SwitchGuns();
            this.GetComponent<UIManager>().bandageAmountText.text = "绷带 x " + bandageAmount.ToString();
            this.GetComponent<UIManager>().drinkAmountText.text = "饮料 x " + drinkAmount.ToString();
            rayPos = myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            if (Input.GetKeyDown(KeyCode.F))
            {
                Ray ray = new Ray(rayPos, myCamera.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, 3, 1 << 12))
                {
                    PickItems(hit.collider.gameObject.tag);
                    hit.collider.gameObject.GetComponent<CanPickObject>().DestoryThisObject();
                }
            }
        }
    }
    /// <summary>
    /// 拾取物品
    /// </summary>
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
                case "FirstAid":
                    this.GetComponent<UIManager>().itemText.text = "拾取急救包";
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        pv.RPC("RemovePlayerPickItem", RpcTarget.AllBuffered, hit.collider.gameObject.tag);
                        hit.collider.gameObject.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.AllBuffered);
                        firstAidAmount++;
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
            }
        }
        else
        {
            this.GetComponent<UIManager>().itemText.text = null;
            this.GetComponent<UIManager>().itemText.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 拾取枪
    /// </summary>
    /// <param name="itemName"></param>
    private void PickItems(string itemName)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].tag == itemName)
            {
                if (gun1State == false)
                {
                    //把捡到的枪放在手中一号枪位
                    items[i].transform.SetParent(handgun1);
                    //开启与拾取到的东西相同的物品
                    items[i].SetActive(true);
                    gun1State = true; //此时1号位有一把枪
                    GunImageControl(itemName);
                    pv.RPC("RemovePlayerPick", RpcTarget.AllBuffered, itemName);
                    this.GetComponent<UIManager>().isAuto.gameObject.SetActive(true);
                    handsMelle.SetActive(false); //关闭手
                    this.GetComponent<UIManager>().bulletsControl.SetActive(true);
                }
                else if (gun1State == true && gun2State == false)
                {
                    if (handgun1.GetChild(0).gameObject.tag == itemName)
                    {
                        Debug.Log("有一把相同的枪");
                    }
                    else
                    {
                        //把捡到的枪放在手中一号枪位
                        items[i].transform.SetParent(handgun2);
                        //关闭与拾取到的东西相同的物品
                        items[i].SetActive(false);
                        gun2State = true;
                    }
                }
                else if (gun1State == true && gun2State == true)
                {
                    if (items[i].tag == handgun1.GetChild(0).gameObject.tag)
                    {
                        Debug.Log("相同的枪");
                    }
                    else if (items[i].tag == handgun1.GetChild(0).gameObject.tag && items[i].tag == handgun2.GetChild(0).gameObject.tag)
                    {
                        handgun1.GetChild(0).gameObject.SetActive(false);
                        handgun2.GetChild(0).gameObject.SetActive(true);
                    }
                    else if (items[i].tag != handgun1.GetChild(0).gameObject.tag && items[i].tag != handgun2.GetChild(0).gameObject.tag)
                    {
                        if (handgun2.GetChild(0).gameObject.activeInHierarchy == true)
                        {
                            handgun1.GetChild(0).gameObject.SetActive(false);
                            handgun1.GetChild(0).gameObject.transform.SetParent(handAndGuns);
                            //把捡到的枪放在手中一号枪位
                            items[i].transform.SetParent(handgun1);
                            //关闭与拾取到的东西相同的物品
                            items[i].SetActive(false);
                        }
                        else if (handgun1.GetChild(0).gameObject.activeInHierarchy == true)
                        {
                            handgun2.GetChild(0).gameObject.SetActive(false);
                            handgun2.GetChild(0).gameObject.transform.SetParent(handAndGuns);
                            //把捡到的枪放在手中一号枪位
                            items[i].transform.SetParent(handgun2);
                            //关闭与拾取到的东西相同的物品
                            items[i].SetActive(false);
                        }
                    }
                }
            }
        }
    }
    /// <summary>
    /// 切换枪
    /// </summary>
    private void SwitchGuns()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (handgun1.childCount > 0 && handgun2.childCount > 0)
            {
                handgun1.GetChild(0).gameObject.SetActive(true);
                handgun2.GetChild(0).gameObject.SetActive(false);
                GunImageControl(handgun1.GetChild(0).gameObject.tag);
                handsMelle.SetActive(false);
                pv.RPC("RemovePlayerPick", RpcTarget.AllBuffered, handgun1.GetChild(0).gameObject.tag);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (handgun1.childCount > 0 && handgun2.childCount > 0)
            {
                handgun1.GetChild(0).gameObject.SetActive(false);
                handgun2.GetChild(0).gameObject.SetActive(true);
                GunImageControl(handgun2.GetChild(0).gameObject.tag);
                handsMelle.SetActive(false);
                pv.RPC("RemovePlayerPick", RpcTarget.AllBuffered, handgun2.GetChild(0).gameObject.tag);
            }
        }
    }
    /// <summary>
    /// 枪械图标显示
    /// </summary>
    private void GunImageControl()
    {
        this.GetComponent<UIManager>().gunImage[0].gameObject.SetActive(false);
        this.GetComponent<UIManager>().gunImage[1].gameObject.SetActive(false);
        this.GetComponent<UIManager>().gunImage[2].gameObject.SetActive(false);
        this.GetComponent<UIManager>().gunImage[3].gameObject.SetActive(false);
        this.GetComponent<UIManager>().gunImage[4].gameObject.SetActive(false);
    }
    /// <summary>
    /// 枪械图标控制
    /// </summary>
    /// <param name="itemName"></param>
    private void GunImageControl(string itemName)
    {
        switch (itemName)
        {
            case "AK47":
                GunImageControl();
                this.GetComponent<UIManager>().gunImage[1].gameObject.SetActive(true);
                break;
            case "Barrett":
                GunImageControl();
                this.GetComponent<UIManager>().gunImage[0].gameObject.SetActive(true);
                break;
            case "ShotGun":
                GunImageControl();
                this.GetComponent<UIManager>().gunImage[4].gameObject.SetActive(true);
                break;
            case "M4A1":
                GunImageControl();
                this.GetComponent<UIManager>().gunImage[2].gameObject.SetActive(true);
                break;
            case "SMG":
                GunImageControl();
                this.GetComponent<UIManager>().gunImage[3].gameObject.SetActive(true);
                break;
            case "Pistol":
                GunImageControl();
                this.GetComponent<UIManager>().gunImage[2].gameObject.SetActive(true);
                break;
            case "SandEagle":
                GunImageControl();
                this.GetComponent<UIManager>().gunImage[2].gameObject.SetActive(true);
                break;
        }
    }
    /// <summary>
    /// 远程玩家拾取头盔
    /// </summary>
    /// <param name="item"></param>
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
    /// <summary>
    /// 远程玩家拾取枪
    /// </summary>
    /// <param name="name"></param>
    [PunRPC]
    private void RemovePlayerPick(string name)
    {
        switch (name)
        {
            case "AK47":
                {
                    gunRemove[2].SetActive(true);
                    gunRemove[0].SetActive(false);
                    gunRemove[1].SetActive(false);
                    gunRemove[3].SetActive(false);
                    gunRemove[4].SetActive(false);
                }
                break;
            case "M4A1":
                {
                    gunRemove[1].SetActive(true);
                    gunRemove[0].SetActive(false);
                    gunRemove[2].SetActive(false);
                    gunRemove[3].SetActive(false);
                    gunRemove[4].SetActive(false);
                }
                break;
            case "Barrett":
                {
                    gunRemove[0].SetActive(true);
                    gunRemove[1].SetActive(false);
                    gunRemove[2].SetActive(false);
                    gunRemove[3].SetActive(false);
                    gunRemove[4].SetActive(false);
                }
                break;
            case "SMG":
                {
                    gunRemove[3].SetActive(true);
                    gunRemove[0].SetActive(false);
                    gunRemove[1].SetActive(false);
                    gunRemove[2].SetActive(false);
                    gunRemove[4].SetActive(false);
                }
                break;
            case "Shotgun":
                {
                    gunRemove[4].SetActive(true);
                    gunRemove[0].SetActive(false);
                    gunRemove[1].SetActive(false);
                    gunRemove[2].SetActive(false);
                    gunRemove[3].SetActive(false);
                }
                break;
        }
    }
}
