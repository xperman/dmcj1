﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//包括 捡枪和换枪和丢弃枪和射击
public class HandsOperating : MonoBehaviour
{
    //所有的物品，包括枪
    public GameObject[] items;
    private InputManager inputManager;
    public Camera myCamera;
    private Vector3 rayPos;
    public Transform handgun1;
    public Transform handgun2;
    public Transform handAndGuns;
    private bool gun1State; //判断1号位是否有枪
    private bool gun2State; //判断1号位是否有枪
    private PhotonView pv;
    public Animator myAnimator;
    //远程玩家的枪
    public GameObject[] gunRemove;
    private float lastFired;
    public float fireRate;
    private bool isAutoFire = false;
    private bool clickGo = true;
    public GameObject bulletHole;
    //可丢弃的枪
    public GameObject[] throwGuns;

    private void Start()
    {
        gun1State = false;
        gun2State = false;
        inputManager = this.GetComponent<InputManager>();
        pv = this.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (pv.IsMine)
        {
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
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            SwitchGuns();
            AutoFireControl();
            Shoot();
        }
    }

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
    //拾取枪
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
                    Debug.Log("萝卜");
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
                        Debug.Log("萝卜2");
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
                            //GunImageControl(itemName);
                            Debug.Log("哈哈1");
                        }
                        else if (handgun1.GetChild(0).gameObject.activeInHierarchy == true)
                        {
                            handgun2.GetChild(0).gameObject.SetActive(false);
                            handgun2.GetChild(0).gameObject.transform.SetParent(handAndGuns);
                            //把捡到的枪放在手中一号枪位
                            items[i].transform.SetParent(handgun2);
                            //关闭与拾取到的东西相同的物品
                            items[i].SetActive(false);
                            //GunImageControl(itemName);
                            Debug.Log("哈哈2");
                        }
                    }
                }
            }
        }
    }
    private void GunImageControl()
    {
        this.GetComponent<UIManager>().gunImage[0].gameObject.SetActive(false);
        this.GetComponent<UIManager>().gunImage[1].gameObject.SetActive(false);
        this.GetComponent<UIManager>().gunImage[2].gameObject.SetActive(false);
        this.GetComponent<UIManager>().gunImage[3].gameObject.SetActive(false);
        this.GetComponent<UIManager>().gunImage[4].gameObject.SetActive(false);
    }
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
    //切换枪
    private void SwitchGuns()
    {
        if (inputManager.GetInput(KeyCode.Alpha1))
        {
            if (handgun1.childCount > 0 && handgun2.childCount > 0)
            {
                handgun1.GetChild(0).gameObject.SetActive(true);
                handgun2.GetChild(0).gameObject.SetActive(false);
                GunImageControl(handgun1.GetChild(0).gameObject.tag);
                pv.RPC("RemovePlayerPick", RpcTarget.AllBuffered, handgun1.GetChild(0).gameObject.tag);
            }
        }
        if (inputManager.GetInput(KeyCode.Alpha2))
        {
            if (handgun1.childCount > 0 && handgun2.childCount > 0)
            {
                handgun1.GetChild(0).gameObject.SetActive(false);
                handgun2.GetChild(0).gameObject.SetActive(true);
                GunImageControl(handgun2.GetChild(0).gameObject.tag);
                pv.RPC("RemovePlayerPick", RpcTarget.AllBuffered, handgun2.GetChild(0).gameObject.tag);
            }
        }
    }
    //射击
    private void Shoot()
    {
        //单发模式
        if (Input.GetMouseButtonDown(0) && !isAutoFire)
        {
            AppleShoot();
        }
        //全自动模式
        if (Input.GetMouseButton(0) && isAutoFire == true)
        {
            if (Time.time - lastFired > 1 / fireRate)
            {
                AppleShoot();
                lastFired = Time.time;
            }
        }
        else
        {
            Debug.Log("Not");
        }
    }
    //实现射击
    private void AppleShoot()
    {
        if (handgun1.childCount != 0 || handgun2.childCount != 0)
        {
            if (handgun1.GetChild(0).gameObject.activeInHierarchy == true)
            {
                if (handgun1.GetChild(0).gameObject.tag == "AK47")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Akm>().scarBullets > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Akm>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Akm>().scarBullets.ToString() + "/30";
                        ShootRay(100);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "M4A1")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Scar>().scarBullets > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Scar>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Scar>().scarBullets.ToString() + "/30";
                        ShootRay(100);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Barrett")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Sinper>().scarBullets > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Sinper>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Sinper>().scarBullets.ToString() + "/5";
                        ShootRay(1000);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Shotgun")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Lever>().scarBullets > 0)
                    {
                        Debug.Log("here!!!!!");
                        handgun1.GetChild(0).gameObject.GetComponent<Lever>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Lever>().scarBullets.ToString() + "/2";
                        ShootRay(5);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "SMG")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Smg>().scarBullets > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Smg>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Smg>().scarBullets.ToString() + "/25";
                        ShootRay(10);
                    }
                }
            }
            else if (handgun2.GetChild(0).gameObject.activeInHierarchy == true)
            {
                if (handgun2.GetChild(0).gameObject.tag == "AK47")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Akm>().scarBullets > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Akm>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Akm>().scarBullets.ToString() + "/30";
                        ShootRay(100);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "M4A1")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Scar>().scarBullets > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Scar>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Scar>().scarBullets.ToString() + "/30";
                        ShootRay(100);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Barrett")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Sinper>().scarBullets > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Sinper>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Sinper>().scarBullets.ToString() + "/5";
                        ShootRay(1000);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Shotgun")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Lever>().scarBullets > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Lever>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Lever>().scarBullets.ToString() + "/2";
                        ShootRay(5);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "SMG")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Smg>().scarBullets > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Smg>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Smg>().scarBullets.ToString() + "/25";
                        ShootRay(10);
                    }
                }
            }
        }

    }
    //子弹射线
    private void ShootRay(float distance)
    {
        rayPos = myCamera.ViewportToWorldPoint(new Vector3(0.5f, Random.Range(0.5f, 1.0f), 0.0f));
        Ray ray = new Ray(rayPos, myCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            //如果击中敌人
            if (hit.collider.gameObject.tag == "Player")
            {
                //调用敌人的减血代码
                hit.transform.GetComponent<PhotonView>().RPC("DamageGet", RpcTarget.AllBuffered, 10, hit.point);
                //生成一个临时弹孔
                GameObject tempHole = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                //0.3s后销毁弹孔
                Destroy(tempHole, 0.3f);
            }
            //如果击中建筑物
            else if (hit.collider.gameObject.tag == "buildings")
            {
                //生成一个临时弹孔
                GameObject tempHole = Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                Destroy(tempHole, 0.3f);
            }
        }
    }
    //开火模式
    private void AutoFireControl()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (clickGo == true)
            {
                isAutoFire = true;
                clickGo = false;
                Debug.Log("自动");
                this.GetComponent<UIManager>().isAuto.text = "自动";
            }
            else
            {
                isAutoFire = false;
                clickGo = true;
                Debug.Log("单点");
                this.GetComponent<UIManager>().isAuto.text = "单发";
            }
        }
    }
    //重新装弹
    private void Reload()
    {
        if (handgun1.childCount != 0 || handgun2.childCount != 0)
        {
            if (handgun1.GetChild(0).gameObject.activeInHierarchy == true)
            {
                if (handgun1.GetChild(0).gameObject.tag == "AK47")
                {
                    handgun1.GetChild(0).gameObject.GetComponent<Akm>().Reload();
                    this.GetComponent<UIManager>().bandageAmountText.text = this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Akm>().scarBullets.ToString() + "/30";
                }
                else if (handgun1.GetChild(0).gameObject.tag == "M4A1")
                {
                    handgun1.GetChild(0).gameObject.GetComponent<Scar>().Reload();
                    this.GetComponent<UIManager>().bandageAmountText.text = this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Scar>().scarBullets.ToString() + "/30";
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Barrett")
                {
                    handgun1.GetChild(0).gameObject.GetComponent<Sinper>().Reload();
                    this.GetComponent<UIManager>().bandageAmountText.text = this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Sinper>().scarBullets.ToString() + "/5";
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Shotgun")
                {
                    handgun1.GetChild(0).gameObject.GetComponent<Lever>().Reload();
                    this.GetComponent<UIManager>().bandageAmountText.text = this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Lever>().scarBullets.ToString() + "/2";
                }
                else if (handgun1.GetChild(0).gameObject.tag == "SMG")
                {
                    handgun1.GetChild(0).gameObject.GetComponent<Smg>().Reload();
                    this.GetComponent<UIManager>().bandageAmountText.text = this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Smg>().scarBullets.ToString() + "/25";
                }
                //myAnimator.SetTrigger("Reload");
            }
            else if (handgun2.GetChild(0).gameObject.activeInHierarchy == true)
            {
                if (handgun2.GetChild(0).gameObject.tag == "AK47")
                {
                    handgun2.GetChild(0).gameObject.GetComponent<Akm>().Reload();
                    this.GetComponent<UIManager>().bandageAmountText.text = this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Akm>().scarBullets.ToString() + "/30";
                }
                else if (handgun2.GetChild(0).gameObject.tag == "M4A1")
                {
                    handgun2.GetChild(0).gameObject.GetComponent<Scar>().Reload();
                    this.GetComponent<UIManager>().bandageAmountText.text = this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Scar>().scarBullets.ToString() + "/30";
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Barrett")
                {
                    handgun2.GetChild(0).gameObject.GetComponent<Sinper>().Reload();
                    this.GetComponent<UIManager>().bandageAmountText.text = this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Sinper>().scarBullets.ToString() + "/5";
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Shotgun")
                {
                    handgun2.GetChild(0).gameObject.GetComponent<Lever>().Reload();
                    this.GetComponent<UIManager>().bandageAmountText.text = this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Lever>().scarBullets.ToString() + "/3";
                }
                else if (handgun2.GetChild(0).gameObject.tag == "SMG")
                {
                    handgun2.GetChild(0).gameObject.GetComponent<Smg>().Reload();
                    this.GetComponent<UIManager>().bandageAmountText.text = this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Smg>().scarBullets.ToString() + "/25";
                }
            }
        }
    }

    private void ThrowGuns(string name)
    {
        switch (name)
        {
            case "AK47":
                PhotonNetwork.Instantiate(throwGuns[2].name, transform.position, Quaternion.identity);
                break;
            case "Barrett":
                PhotonNetwork.Instantiate(throwGuns[0].name, transform.position, Quaternion.identity);
                break;
            case "Shotgun":
                PhotonNetwork.Instantiate(throwGuns[4].name, transform.position, Quaternion.identity);
                break;
            case "M4A1":
                PhotonNetwork.Instantiate(throwGuns[1].name, transform.position, Quaternion.identity);
                break;
            case "SMG":
                PhotonNetwork.Instantiate(throwGuns[3].name, transform.position, Quaternion.identity);
                break;
        }
    }
}
