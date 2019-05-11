using System.Collections;
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
    public Animator hands;
    //远程玩家的枪
    public GameObject[] gunRemove;
    private float lastFired;
    public float fireRate;
    private bool isAutoFire = false;
    private bool clickGo = true;
    public GameObject bulletHole;
    //可丢弃的枪
    public GameObject[] throwGuns;

    public GameObject handsMelle;

    public Animator[] tempGun;

    private void Start()
    {
        gun1State = false;
        gun2State = false;
        isFire = true;
        nowCamera = myCamera;
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
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (handgun1.childCount != 0 || handgun2.childCount != 0)
                {
                    if (handgun1.GetChild(0).gameObject.activeInHierarchy == true)
                    {
                        handgun1.GetChild(0).gameObject.SetActive(false);
                        handsMelle.SetActive(true);
                    }
                    else if (handgun2.GetChild(0).gameObject.activeInHierarchy == true)
                    {
                        handgun2.GetChild(0).gameObject.SetActive(false);
                        handsMelle.SetActive(true);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                isFire = false;
                Reload();
                StartCoroutine("ReloadTime");
            }
            SwitchGuns();
            AutoFireControl();
            Shoot();
            Aim();
            OpenDoor();
            if (Input.GetMouseButtonDown(0) || handgun1.childCount != 0 || handgun2.childCount != 0)
            {
                hands.SetTrigger("Hand");
            }
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
                handsMelle.SetActive(false);
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
                handsMelle.SetActive(false);
                pv.RPC("RemovePlayerPick", RpcTarget.AllBuffered, handgun2.GetChild(0).gameObject.tag);
            }
        }
    }
    //射击
    private void Shoot()
    {
        //单发模式
        if (Input.GetMouseButtonDown(0) && !isAutoFire && isFire == true)
        {
            AppleShoot();
        }
        //全自动模式
        if (Input.GetMouseButton(0) && isAutoFire == true && isFire == true)
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

    private bool isAimming = false;

    //实现射击
    private void AppleShoot()
    {
        if (handgun1.childCount != 0 || handgun2.childCount != 0)
        {
            if (handgun1.GetChild(0).gameObject.activeInHierarchy == true)
            {
                if (handgun1.GetChild(0).gameObject.tag == "AK47")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Akm>().bulletsAmount > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Akm>().useBullets();
                        tempGun[2].SetTrigger("Fire");
                        if (isAimming == true)
                        {
                            ShootRayAim(100, 25);
                            handgun1.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("AimFire");
                        }
                        else if (isAimming == false)
                        {
                            ShootRay(100, 25);
                        }
                        pv.RPC("ShootRemove", RpcTarget.AllBuffered, handgun1.GetChild(0).gameObject.tag);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "M4A1")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Scar>().bulletsAmount > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Scar>().useBullets();
                        tempGun[1].SetTrigger("Fire");
                        ShootRay(100, 20);
                        pv.RPC("ShootRemove", RpcTarget.AllBuffered, handgun1.GetChild(0).gameObject.tag);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Barrett")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Sinper>().bulletsAmount > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Sinper>().useBullets();

                        ShootRay(1000, 90);
                        pv.RPC("ShootRemove", RpcTarget.AllBuffered, handgun1.GetChild(0).gameObject.tag);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Shotgun")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Lever>().bulletsAmount > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Lever>().useBullets();

                        ShootRay(10, 50);
                        pv.RPC("ShootRemove", RpcTarget.AllBuffered, handgun1.GetChild(0).gameObject.tag);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "SMG")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Smg>().bulletsAmount > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Smg>().UseBullets();

                        ShootRay(50, 10);
                        pv.RPC("ShootRemove", RpcTarget.AllBuffered, handgun1.GetChild(0).gameObject.tag);
                    }
                }
            }
            else if (handgun2.GetChild(0).gameObject.activeInHierarchy == true)
            {
                if (handgun2.GetChild(0).gameObject.tag == "AK47")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Akm>().bulletsAmount > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Akm>().useBullets();
                        tempGun[2].SetTrigger("Fire");
                        ShootRay(100, 25);
                        pv.RPC("ShootRemove", RpcTarget.AllBuffered, handgun2.GetChild(0).gameObject.tag);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "M4A1")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Scar>().bulletsAmount > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Scar>().useBullets();
                        tempGun[1].SetTrigger("Fire");
                        ShootRay(100, 20);
                        pv.RPC("ShootRemove", RpcTarget.AllBuffered, handgun2.GetChild(0).gameObject.tag);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Barrett")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Sinper>().bulletsAmount > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Sinper>().useBullets();

                        ShootRay(1000, 90);
                        pv.RPC("ShootRemove", RpcTarget.AllBuffered, handgun2.GetChild(0).gameObject.tag);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Shotgun")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Lever>().bulletsAmount > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Lever>().useBullets();

                        ShootRay(10, 50);
                        pv.RPC("ShootRemove", RpcTarget.AllBuffered, handgun2.GetChild(0).gameObject.tag);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "SMG")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Smg>().bulletsAmount > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Smg>().UseBullets();

                        ShootRay(50, 10);
                        pv.RPC("ShootRemove", RpcTarget.AllBuffered, handgun2.GetChild(0).gameObject.tag);
                    }
                }
            }
        }

    }
    private Camera nowCamera;
    public Camera aimCamera;
    //子弹射线
    private void ShootRay(float distance, int power)
    {
        rayPos = nowCamera.ViewportToWorldPoint(new Vector3(0.5f, Random.Range(0.5f, 1.0f), 0.0f));
        Ray ray = new Ray(rayPos, nowCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            //如果击中敌人
            if (hit.collider.gameObject.tag == "Player")
            {
                //调用敌人的减血代码
                hit.transform.GetComponent<PhotonView>().RPC("DamageGet", RpcTarget.AllBuffered, power, hit.point);
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

    private Vector3 rayPosAim;
    private void ShootRayAim(float distance, int power)
    {
        rayPosAim = redPointPos.transform.position;
        Ray ray = new Ray(rayPosAim, redPointPos.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, distance))
        {
            //如果击中敌人
            if (hit.collider.gameObject.tag == "Player")
            {
                //调用敌人的减血代码
                hit.transform.GetComponent<PhotonView>().RPC("DamageGet", RpcTarget.AllBuffered, power, hit.point);
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

    private bool isAim = true;

    public GameObject[] aimStateGuns;

    public Animator test;

    public Transform redPointPos;

    private void Aim()
    {
        if (handgun1.childCount != 0 || handgun2.childCount != 0)
        {
            if (handgun1.GetChild(0).gameObject.activeInHierarchy == true)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    this.GetComponent<UIManager>().aimImage.gameObject.SetActive(false);
                    if (handgun1.GetChild(0).gameObject.tag == "Barrett")
                    {
                        nowCamera = aimCamera;
                        aimStateGuns[0].SetActive(true);

                        this.GetComponent<RotateView>().sensitivityHor = 0.3f;
                        this.GetComponent<RotateView>().sensitivityVert = 0.3f;
                    }
                    else if (handgun1.GetChild(0).gameObject.tag == "AK47")
                    {
                        isAimming = true;
                        handgun1.GetChild(0).gameObject.GetComponent<Animator>().SetBool("Aim", true);
                        //改变子弹射线的初始位置
                        rayPosAim = redPointPos.transform.position;
                        test.SetBool("Aim", true);
                    }
                    else if (handgun1.GetChild(0).gameObject.tag == "M4A1")
                    {
                        nowCamera.cullingMask = ~(1 << 14);
                        aimStateGuns[1].SetActive(true);
                        tempGun[1].SetBool("Aim", true);
                    }
                    else if (handgun1.GetChild(0).gameObject.tag == "SMG")
                    {
                        nowCamera.cullingMask = ~(1 << 14);
                        aimStateGuns[3].SetActive(true);
                    }
                }
                if (Input.GetMouseButtonUp(1))
                {
                    this.GetComponent<UIManager>().aimImage.gameObject.SetActive(true);
                    if (handgun1.GetChild(0).gameObject.tag == "Barrett")
                    {
                        nowCamera = myCamera;
                        aimStateGuns[0].SetActive(false);
                        this.GetComponent<RotateView>().sensitivityHor = 2;
                        this.GetComponent<RotateView>().sensitivityVert = 2;
                    }
                    else if (handgun1.GetChild(0).gameObject.tag == "AK47")
                    {
                        isAimming = false;
                        test.SetBool("Aim", false);
                    }
                    else if (handgun1.GetChild(0).gameObject.tag == "M4A1")
                    {
                        nowCamera.cullingMask = -1;
                        aimStateGuns[1].SetActive(false);
                        tempGun[1].SetBool("Aim", false);
                    }
                    else if (handgun1.GetChild(0).gameObject.tag == "SMG")
                    {
                        nowCamera.cullingMask = -1;
                        aimStateGuns[3].SetActive(false);
                    }
                }
            }
            else if (handgun2.GetChild(0).gameObject.activeInHierarchy == true)
            {

                if (Input.GetMouseButtonDown(1))
                {
                    this.GetComponent<UIManager>().aimImage.gameObject.SetActive(false);
                    if (handgun2.GetChild(0).gameObject.tag == "Barrett")
                    {
                        nowCamera = aimCamera;
                        aimStateGuns[0].SetActive(true);
                        this.GetComponent<RotateView>().sensitivityHor = 0.3f;
                        this.GetComponent<RotateView>().sensitivityVert = 0.3f;
                    }
                    else if (handgun2.GetChild(0).gameObject.tag == "AK47")
                    {
                        nowCamera.cullingMask = ~(1 << 14);
                        aimStateGuns[2].SetActive(true);
                    }
                    else if (handgun2.GetChild(0).gameObject.tag == "M4A1")
                    {
                        nowCamera.cullingMask = ~(1 << 14);
                        aimStateGuns[1].SetActive(true);
                    }

                    else if (handgun2.GetChild(0).gameObject.tag == "SMG")
                    {
                        nowCamera.cullingMask = ~(1 << 14);
                        aimStateGuns[3].SetActive(true);
                    }
                }
                if (Input.GetMouseButtonUp(1))
                {
                    this.GetComponent<UIManager>().aimImage.gameObject.SetActive(true);
                    if (handgun2.GetChild(0).gameObject.tag == "Barrett")
                    {
                        nowCamera = myCamera;
                        aimStateGuns[0].SetActive(false);
                        this.GetComponent<RotateView>().sensitivityHor = 2;
                        this.GetComponent<RotateView>().sensitivityVert = 2;
                    }
                    else if (handgun2.GetChild(0).gameObject.tag == "AK47")
                    {
                        nowCamera.cullingMask = -1;
                        aimStateGuns[2].SetActive(false);
                    }
                    else if (handgun2.GetChild(0).gameObject.tag == "M4A1")
                    {
                        nowCamera.cullingMask = -1;
                        aimStateGuns[1].SetActive(false);
                    }
                    else if (handgun2.GetChild(0).gameObject.tag == "SMG")
                    {
                        nowCamera.cullingMask = -1;
                        aimStateGuns[3].SetActive(false);
                    }
                }
            }
        }

    }

    private void OpenDoor()
    {
        rayPos = myCamera.ViewportToWorldPoint(new Vector3(0.5f, Random.Range(0.5f, 1.0f), 0.0f));
        Ray ray = new Ray(rayPos, myCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2))
        {
            if (hit.collider.gameObject.tag == "door")
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    hit.collider.gameObject.GetComponent<PhotonView>().RPC("Open", RpcTarget.AllBuffered);
                }
            }
        }
    }
    private bool isFire;
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
                    if (handgun1.GetChild(0).gameObject.GetComponent<Akm>().bulletsAmount < 30)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Akm>().Reload();
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "M4A1")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Scar>().bulletsAmount < 30)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Scar>().Reload();
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Barrett")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Sinper>().bulletsAmount < 5)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Sinper>().Reload();
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Shotgun")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Lever>().bulletsAmount < 2)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Lever>().Reload();
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "SMG")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Smg>().bulletsAmount < 25)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Smg>().Reload();
                    }
                }
            }
            else if (handgun2.GetChild(0).gameObject.activeInHierarchy == true)
            {
                if (handgun2.GetChild(0).gameObject.tag == "AK47")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Akm>().bulletsAmount < 30)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Akm>().Reload();
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "M4A1")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Scar>().bulletsAmount < 30)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Scar>().Reload();
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Barrett")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Sinper>().bulletsAmount < 5)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Sinper>().Reload();
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Shotgun")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Lever>().bulletsAmount < 2)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Lever>().Reload();
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "SMG")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Smg>().bulletsAmount < 25)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Smg>().Reload();
                    }
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

    public GameObject[] audioBied;

    [PunRPC]
    public void ShootRemove(string name)
    {
        switch (name)
        {
            case "AK47":
                Instantiate(audioBied[2], this.transform.position, Quaternion.identity);
                break;
            case "Barrett":
                Instantiate(audioBied[0], this.transform.position, Quaternion.identity);
                break;
            case "Shotgun":
                Instantiate(audioBied[4], this.transform.position, Quaternion.identity);
                break;
            case "M4A1":
                Instantiate(audioBied[1], this.transform.position, Quaternion.identity);
                break;
            case "SMG":
                Instantiate(audioBied[3], this.transform.position, Quaternion.identity);
                break;
        }
    }

    IEnumerator ReloadTime()
    {
        yield return new WaitForSeconds(1.4f);
        isFire = true;
    }
}
