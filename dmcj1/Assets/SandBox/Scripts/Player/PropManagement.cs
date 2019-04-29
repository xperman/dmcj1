using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PropManagement : MonoBehaviour
{
    #region private
    //射线发射位置
    private Vector3 rayPos;
    #endregion
    #region public
    //拾取用的摄像机
    public Camera myCamera;
    //可拾取枪的集合
    public GameObject[] pickGuns;
    //丢弃的枪
    public GameObject[] throwGuns;
    //枪械的出生位置
    public Transform handGun1;
    //备用的枪的位置
    public Transform handGun2;
    //手中的枪
    public GameObject handGun = null;
    //备用的枪
    public GameObject backGun = null;
    #endregion
    void Start()
    {

    }

    private void Update()
    {
        if (PlayerManager.pv.IsMine)
        {
            AroundWeapons();
            if (Input.GetKeyDown(KeyCode.G))
            {
                //扔掉本地玩家的枪
                if (handGun1.GetChild(0).gameObject.activeInHierarchy == true)
                {
                    Destroy(handGun1.GetChild(0).gameObject);
                    ThrowGuns(handGun1.GetChild(0).gameObject.tag);
                }
                else if (handGun2.GetChild(0).gameObject.activeInHierarchy == true)
                {
                    Destroy(handGun2.GetChild(0).gameObject);
                    ThrowGuns(handGun2.GetChild(0).gameObject.tag);
                }

            }
        }
    }

    private void ThrowGuns(string name)
    {
        switch (name)
        {
            case "sinper":
                Instantiate(throwGuns[0], transform.position, transform.rotation);
                break;
            case "akm":
                Instantiate(throwGuns[2], transform.position, transform.rotation);
                break;
            case "scar":
                Instantiate(throwGuns[1], transform.position, transform.rotation);
                break;
            case "smg":
                Instantiate(throwGuns[4], transform.position, transform.rotation);
                break;
            case "lever":
                Instantiate(throwGuns[3], transform.position, transform.rotation);
                break;
        }
    }
    /// <summary>
    /// 拾取武器
    /// </summary>
    public void AroundWeapons()
    {
        rayPos = myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Ray ray = new Ray(rayPos, myCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3, 1 << 13))
        {
            UIManager.Instance.itemText.gameObject.SetActive(true);
            switch (hit.collider.gameObject.tag)
            {
                case "sinper":
                    UIManager.Instance.itemText.text = "F 拾取" + hit.collider.gameObject.tag;
                    PickWeaspon(hit.collider.gameObject.name, pickGuns[0], hit.collider.gameObject);
                    break;
                case "scar":
                    UIManager.Instance.itemText.text = "F 拾取" + hit.collider.gameObject.tag;
                    PickWeaspon(hit.collider.gameObject.name, pickGuns[1], hit.collider.gameObject);
                    break;
                case "akm":
                    UIManager.Instance.itemText.text = "F 拾取" + hit.collider.gameObject.tag;
                    PickWeaspon(hit.collider.gameObject.name, pickGuns[2], hit.collider.gameObject);
                    break;
                case "smg":
                    UIManager.Instance.itemText.text = "F 拾取" + hit.collider.gameObject.tag;
                    PickWeaspon(hit.collider.gameObject.name, pickGuns[3], hit.collider.gameObject);
                    break;
                case "lever":
                    UIManager.Instance.itemText.text = "F 拾取" + hit.collider.gameObject.tag;
                    PickWeaspon(hit.collider.gameObject.name, pickGuns[4], hit.collider.gameObject);
                    break;
            }
        }
        else
        {
            UIManager.Instance.itemText.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 拾取地上的枪
    /// </summary>
    /// <param name="gunName"></param>
    /// <param name="hitObject"></param>
    private void PickWeaspon(string gunName, GameObject hitObject, GameObject hit)
    {
        for (int i = 0; i < pickGuns.Length; i++)
        {
            if (pickGuns[i] == hitObject)
            {
                //如果手中和后背都没枪
                if (handGun == null && backGun == null)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        GameObject tempGun = Instantiate(pickGuns[i], handGun1.position, handGun1.rotation);
                        hit.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.All);
                        tempGun.transform.SetParent(handGun1.transform);
                        tempGun.SetActive(true);
                        handGun = tempGun;
                        PlayerManager.pv.RPC("gunHand", RpcTarget.AllBuffered, tempGun.tag);
                    }
                }//如果手中和后背都有枪
                else if (handGun != null && backGun != null)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        UIManager.Instance.itemText.text = "枪械已满，丢弃多余的枪可以捡起它";
                    }
                }//如果手中有枪且后背没枪
                else if (handGun != null && backGun == null)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        GameObject tempGun = Instantiate(pickGuns[i], handGun2.position, handGun2.rotation);
                        hit.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.All);
                        tempGun.transform.SetParent(handGun2.transform);
                        tempGun.SetActive(false);
                        backGun = tempGun;
                        //在远程玩家的后背生成一把额外的枪    
                        PlayerManager.pv.RPC("gunBack", RpcTarget.AllBuffered, tempGun.tag);
                    }
                }
                else if (handGun == null && backGun != null)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        GameObject tempGun = Instantiate(pickGuns[i], handGun1.position, handGun1.rotation);
                        hit.GetComponent<PhotonView>().RPC("DestoryThisObject", RpcTarget.All);
                        tempGun.transform.SetParent(handGun1.transform);
                        tempGun.SetActive(true);
                        handGun = tempGun;
                        PlayerManager.pv.RPC("gunHand", RpcTarget.AllBuffered, tempGun.tag);
                    }
                }
            }
        }
    }
}
