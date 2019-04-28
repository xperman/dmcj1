using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PropManagement : MonoBehaviour
{
    #region private
    private PhotonView pv;
    //射线发射位置
    private Vector3 rayPos;
    #endregion
    #region public
    //拾取用的摄像机
    public Camera myCamera;
    //提示信息
    public Text weaponsText;
    //可拾取枪的集合
    public GameObject[] pickGuns;
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
        pv = this.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            AroundWeapons();
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
            weaponsText.gameObject.SetActive(true);
            switch (hit.collider.gameObject.tag)
            {
                case "sinper":
                    weaponsText.text = "F 拾取" + hit.collider.gameObject.tag;
                    PickWeaspon(hit.collider.gameObject.name, pickGuns[0]);
                    break;
                case "scar":
                    weaponsText.text = "F 拾取" + hit.collider.gameObject.tag;
                    PickWeaspon(hit.collider.gameObject.name, pickGuns[1]);
                    break;
                case "akm":
                    weaponsText.text = "F 拾取" + hit.collider.gameObject.tag;
                    PickWeaspon(hit.collider.gameObject.name, pickGuns[2]);
                    break;
                case "smg":
                    weaponsText.text = "F 拾取" + hit.collider.gameObject.tag;
                    PickWeaspon(hit.collider.gameObject.name, pickGuns[3]);
                    break;
                case "lever":
                    weaponsText.text = "F 拾取" + hit.collider.gameObject.tag;
                    PickWeaspon(hit.collider.gameObject.name, pickGuns[4]);
                    break;
            }
        }
        else
        {
            weaponsText.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 拾取地上的枪
    /// </summary>
    /// <param name="gunName"></param>
    /// <param name="hitObject"></param>
    private void PickWeaspon(string gunName, GameObject hitObject)
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
                        tempGun.transform.SetParent(handGun1.transform);
                        tempGun.SetActive(true);
                        handGun = tempGun;
                        pv.RPC("gunHand", RpcTarget.AllBuffered, tempGun.tag);

                    }
                }//如果手中和后背都有枪
                else if (handGun != null && backGun != null)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        weaponsText.text = "枪械已满，丢弃多余的枪可以捡起它";
                    }
                }//如果手中有枪且后背没枪
                else if (handGun != null && backGun == null)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        GameObject tempGun = Instantiate(pickGuns[i], handGun2.position, handGun2.rotation);
                        tempGun.transform.SetParent(handGun2.transform);
                        tempGun.SetActive(false);
                        backGun = tempGun;
                        //在远程玩家的后背生成一把额外的枪    
                        pv.RPC("gunBack", RpcTarget.AllBuffered, tempGun.tag);
                    }
                }
                else if (handGun == null && backGun != null)
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        GameObject tempGun = Instantiate(pickGuns[i], handGun1.position, handGun1.rotation);
                        tempGun.transform.SetParent(handGun1.transform);
                        tempGun.SetActive(true);
                        handGun = tempGun;
                        pv.RPC("gunHand", RpcTarget.AllBuffered, tempGun.tag);
                    }
                }
            }
        }
    }
}
