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
            }
        }
        if (inputManager.GetInput(KeyCode.Alpha2))
        {
            if (handgun1.childCount > 0 && handgun2.childCount > 0)
            {
                handgun1.GetChild(0).gameObject.SetActive(false);
                handgun2.GetChild(0).gameObject.SetActive(true);
                GunImageControl(handgun2.GetChild(0).gameObject.tag);
            }
        }
    }

    private float lastFired;
    public float fireRate;

    private void Shoot()
    {
        //单发模式
        if (Input.GetMouseButtonDown(0) && !isAutoFire)
        {
            AppleShoot();
        }
        if (Input.GetMouseButtonUp(0) && !isAutoFire)
        {
            Debug.Log("停止射击");
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
                        ShootRay();
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "M4A1")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Scar>().scarBullets > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Scar>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Scar>().scarBullets.ToString() + "/30";
                        ShootRay();
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Barrett")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Sinper>().scarBullets > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Sinper>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Sinper>().scarBullets.ToString() + "/5";
                        ShootRay();
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Shotgun")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Lever>().scarBullets > 0)
                    {
                        Debug.Log("here!!!!!");
                        handgun1.GetChild(0).gameObject.GetComponent<Lever>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Lever>().scarBullets.ToString() + "/2";
                        ShootRay();
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "SMG")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Smg>().scarBullets > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Smg>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun1.GetChild(0).gameObject.GetComponent<Smg>().scarBullets.ToString() + "/25";
                        ShootRay();
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
                        ShootRay();
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "M4A1")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Scar>().scarBullets > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Scar>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Scar>().scarBullets.ToString() + "/30";
                        ShootRay();
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Barrett")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Sinper>().scarBullets > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Sinper>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Sinper>().scarBullets.ToString() + "/5";
                        ShootRay();
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Shotgun")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Lever>().scarBullets > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Lever>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Lever>().scarBullets.ToString() + "/2";
                        ShootRay();
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "SMG")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Smg>().scarBullets > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Smg>().useBullets();
                        this.GetComponent<UIManager>().bulletsAmountText.text = handgun2.GetChild(0).gameObject.GetComponent<Smg>().scarBullets.ToString() + "/25";
                        ShootRay();
                    }
                }
            }
        }

    }
    private void ShootRay()
    {
        rayPos = myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Ray ray = new Ray(rayPos, myCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000))
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
    private bool isAutoFire = false;
    private bool clickGo = true;
    public GameObject bulletHole;

    private void AutoFireControl()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (clickGo == true)
            {
                isAutoFire = true;
                clickGo = false;
                Debug.Log("自动");
            }
            else
            {
                isAutoFire = false;
                clickGo = true;
                Debug.Log("单点");
            }
        }
    }

    private void Reload()
    {
        if (handgun1.childCount != 0 || handgun2.childCount != 0)
        {
            if (handgun1.GetChild(0).gameObject.activeInHierarchy == true)
            {
                if (handgun1.GetChild(0).gameObject.tag == "AK47")
                {
                    handgun1.GetChild(0).gameObject.GetComponent<Akm>().Reload();
                }
                else if (handgun1.GetChild(0).gameObject.tag == "M4A1")
                {
                    handgun1.GetChild(0).gameObject.GetComponent<Scar>().Reload();
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Barrett")
                {
                    handgun1.GetChild(0).gameObject.GetComponent<Sinper>().Reload();
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Shotgun")
                {
                    handgun1.GetChild(0).gameObject.GetComponent<Lever>().Reload();
                }
                else if (handgun1.GetChild(0).gameObject.tag == "SMG")
                {
                    handgun1.GetChild(0).gameObject.GetComponent<Smg>().Reload();
                }
            }
            else if (handgun2.GetChild(0).gameObject.activeInHierarchy == true)
            {
                if (handgun2.GetChild(0).gameObject.tag == "AK47")
                {
                    handgun2.GetChild(0).gameObject.GetComponent<Akm>().Reload();
                }
                else if (handgun2.GetChild(0).gameObject.tag == "M4A1")
                {
                    handgun2.GetChild(0).gameObject.GetComponent<Scar>().Reload();
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Barrett")
                {
                    handgun2.GetChild(0).gameObject.GetComponent<Sinper>().Reload();
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Shotgun")
                {
                    handgun2.GetChild(0).gameObject.GetComponent<Lever>().Reload();
                }
                else if (handgun2.GetChild(0).gameObject.tag == "SMG")
                {
                    handgun2.GetChild(0).gameObject.GetComponent<Smg>().Reload();
                }
            }
        }
    }
}
