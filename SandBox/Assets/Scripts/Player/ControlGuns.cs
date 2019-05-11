using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//只控制射击
public class ControlGuns : MonoBehaviour
{
    private PhotonView pv;
    //输入管理
    private InputManager inputManager;
    //模拟射击子弹的射线的摄像机
    public Camera myCamera;
    //射线位置
    private Vector3 rayPos;
    //主武器
    public Transform handgun1;
    //副武器
    public Transform handgun2;
    //徒手动画
    public Animator hands;
    //上一次的射击时间
    private float lastFired;
    //开火速率
    public float fireRate;
    //是否自动开火
    private bool isAutoFire = false;
    //弹痕
    public GameObject bulletHole;
    //是否可以射击
    private bool isFire;
    //脱离瞄准时的摄像机
    private Camera nowCamera;
    //控制开关
    private bool clickGo = true;

    private void Start()
    {
        inputManager = this.GetComponent<InputManager>();
        pv = this.GetComponent<PhotonView>();
        nowCamera = myCamera;
        isFire = true;
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            rayPos = myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            if (Input.GetMouseButtonDown(0) || handgun1.childCount != 0 || handgun2.childCount != 0)
            {
                hands.SetTrigger("Hand");
            }
            Shoot();
            AutoFireControl();
            if (Input.GetKeyDown(KeyCode.R))
            {
                isFire = false;
                Reload();
                StartCoroutine("ReloadTime");
            }
        }
    }
    /// <summary>
    /// 实例射击
    /// </summary>
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
                        ShootRay(100, 25);
                        handgun1.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("AimFire");
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "M4A1")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Scar>().bulletsAmount > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Scar>().useBullets();
                        ShootRay(100, 20);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Barrett")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Sinper>().bulletsAmount > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Sinper>().useBullets();
                        ShootRay(1000, 90);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "Shotgun")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Lever>().bulletsAmount > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Lever>().useBullets();
                        ShootRay(10, 50);
                    }
                }
                else if (handgun1.GetChild(0).gameObject.tag == "SMG")
                {
                    if (handgun1.GetChild(0).gameObject.GetComponent<Smg>().bulletsAmount > 0)
                    {
                        handgun1.GetChild(0).gameObject.GetComponent<Smg>().UseBullets();
                        ShootRay(50, 10);
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
                        ShootRay(100, 25);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "M4A1")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Scar>().bulletsAmount > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Scar>().useBullets();
                        ShootRay(100, 20);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Barrett")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Sinper>().bulletsAmount > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Sinper>().useBullets();
                        ShootRay(1000, 90);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "Shotgun")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Lever>().bulletsAmount > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Lever>().useBullets();
                        ShootRay(10, 50);
                    }
                }
                else if (handgun2.GetChild(0).gameObject.tag == "SMG")
                {
                    if (handgun2.GetChild(0).gameObject.GetComponent<Smg>().bulletsAmount > 0)
                    {
                        handgun2.GetChild(0).gameObject.GetComponent<Smg>().UseBullets();
                        ShootRay(50, 10);
                    }
                }
            }
        }
    }
    /// <summary>
    /// 开始射击
    /// </summary>
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
    /// <summary>
    /// 模拟子弹的射线
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="power"></param>
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
    /// <summary>
    /// 开火模式
    /// </summary>
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
    /// <summary>
    /// 重新装弹
    /// </summary>
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
    /// <summary>
    /// 子弹填充的时间
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadTime()
    {
        yield return new WaitForSeconds(1.4f);
        isFire = true;
    }
}
