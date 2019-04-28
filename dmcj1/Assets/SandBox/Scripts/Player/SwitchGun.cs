using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class SwitchGun : MonoBehaviour
{
    private PhotonView pv;
    //远程玩家的枪
    public GameObject[] gunsRemove;
    //远程玩家后背的枪
    public GameObject[] gunRemoveBack;
    //当前手中的枪的名字
    public string currentGunName;
    //当前枪的UI图片
    public Sprite[] gunUI;
    //枪械UI显示
    public Image gunImage;
    //远程玩家的动画
    public Animator anmRemove;
    //子弹数量显示
    public Text bulletsAmountText;
    //目前手中的枪械
    public GameObject currentHandGun;
    //子弹发射点
    public Vector3 rayStartPosition;
    //相机
    public Camera myCamera;
    //弹孔
    public GameObject bulletHole;
    //是否全自动开火
    private bool isAutomaticFire;
    //开火间隔
    public float fireRate;
    //开火速率
    float lastFired;

    private void Switch(string gunGame)
    {
        switch (gunGame)
        {
            case "sinper":
                //gunRemoveBack[0].SetActive(true);
                GunSetActive(gunRemoveBack, "sinper");
                gunImage.sprite = gunUI[0];
                break;
            case "scar":
                // gunRemoveBack[1].SetActive(true);
                GunSetActive(gunRemoveBack, "scar");
                gunImage.sprite = gunUI[1];
                break;
            case "akm":
                // gunRemoveBack[2].SetActive(true);
                GunSetActive(gunRemoveBack, "akm");
                gunImage.sprite = gunUI[2];
                break;
            case "smg":
                // gunRemoveBack[3].SetActive(true);
                GunSetActive(gunRemoveBack, "smg");
                gunImage.sprite = gunUI[3];
                break;
            case "lever":
                //gunRemoveBack[4].SetActive(true);
                GunSetActive(gunRemoveBack, "lever");
                gunImage.sprite = gunUI[4];
                break;
            case "hands":
                //gunRemoveBack[5].SetActive(true);
                GunSetActive(gunRemoveBack, "hands");
                gunImage.sprite = gunUI[5];
                break;
        }
    }
    private void SwitchTwoGuns()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && this.GetComponent<PropManagement>().handGun != null && this.GetComponent<PropManagement>().backGun != null)
        {
            this.GetComponent<PropManagement>().handGun.SetActive(true);
            currentHandGun = this.GetComponent<PropManagement>().handGun;
            this.GetComponent<PropManagement>().backGun.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && this.GetComponent<PropManagement>().handGun != null && this.GetComponent<PropManagement>().backGun != null)
        {
            this.GetComponent<PropManagement>().handGun.SetActive(false);
            currentHandGun = this.GetComponent<PropManagement>().backGun;
            this.GetComponent<PropManagement>().backGun.SetActive(true);

        }
    }
    private void Shoot()
    {
        rayStartPosition = myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        if (currentHandGun != null)
        {
            switch (currentHandGun.tag)
            {
                case "sinper":
                    currentHandGun.GetComponent<Sinper>().useBullets();
                    if (currentHandGun.GetComponent<Sinper>().scarBullets > 0)
                    {
                        VirtualRay();
                    }
                    break;
                case "scar":
                    currentHandGun.GetComponent<Scar>().useBullets();
                    if (currentHandGun.GetComponent<Scar>().scarBullets > 0)
                    {
                        VirtualRay();
                    }
                    break;
                case "akm":
                    currentHandGun.GetComponent<Akm>().useBullets();
                    if (currentHandGun.GetComponent<Akm>().scarBullets > 0)
                    {
                        VirtualRay();
                    }
                    break;
                case "smg":
                    currentHandGun.GetComponent<Smg>().useBullets();
                    if (currentHandGun.GetComponent<Smg>().scarBullets > 0)
                    {
                        VirtualRay();
                    }
                    break;
                case "lever":
                    currentHandGun.GetComponent<Lever>().useBullets();
                    if (currentHandGun.GetComponent<Lever>().scarBullets > 0)
                    {
                        VirtualRay();
                    }
                    break;
            }
        }
    }
    private void VirtualRay()
    {
        Ray ray = new Ray(rayStartPosition, myCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000))
        {
            //如果击中敌人
            if (hit.collider.gameObject.tag == "Player")
            {
                //调用敌人的减血代码
                hit.transform.GetComponent<PhotonView>().RPC("DamageGet", RpcTarget.AllBuffered, 10, gameObject.name, hit.point);
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
    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentHandGun != null)
        {
            switch (currentHandGun.tag)
            {
                case "sinper":
                    currentHandGun.GetComponent<Sinper>().scarBullets = 5;
                    currentHandGun.GetComponent<Sinper>().Reload();
                    break;
                case "scar":
                    currentHandGun.GetComponent<Scar>().scarBullets = 30;
                    currentHandGun.GetComponent<Scar>().Reload();
                    break;
                case "akm":
                    currentHandGun.GetComponent<Akm>().scarBullets = 30;
                    currentHandGun.GetComponent<Akm>().Reload();
                    break;
                case "smg":
                    currentHandGun.GetComponent<Smg>().scarBullets = 25;
                    currentHandGun.GetComponent<Smg>().Reload();
                    break;
                case "lever":
                    currentHandGun.GetComponent<Lever>().scarBullets = 2;
                    currentHandGun.GetComponent<Lever>().Reload();
                    break;
            }
            //远程玩家换弹夹操作
            anmRemove.SetTrigger("Reload");
        }
    }
    //调整枪状态
    private void GunSetActive(GameObject[] gunRemove, string name)
    {
        for (int i = 0; i < gunRemove.Length; i++)
        {
            if (gunRemove[i].name == name)
            {
                gunRemove[i].SetActive(true);
            }
            else
            {
                gunRemove[i].SetActive(false);
            }
        }
    }
    [PunRPC]
    public void gunBack(string name)
    {
        switch (name)
        {
            case "sinper":
                gunRemoveBack[0].SetActive(true);
                break;
            case "akm":
                gunRemoveBack[1].SetActive(true);
                break;
            case "scar":
                gunRemoveBack[2].SetActive(true);
                break;
            case "smg":
                gunRemoveBack[3].SetActive(true);
                break;
            case "lever":
                gunRemoveBack[4].SetActive(true);
                break;
        }
    }
    [PunRPC]
    public void gunHand(string name)
    {
        switch (name)
        {
            case "sinper":
                gunsRemove[0].SetActive(true);
                break;
            case "akm":
                gunsRemove[1].SetActive(true);
                break;
            case "scar":
                gunsRemove[2].SetActive(true);
                break;
            case "smg":
                gunsRemove[3].SetActive(true);
                break;
            case "lever":
                gunsRemove[4].SetActive(true);
                break;
        }
    }
    void Start()
    {
        pv = this.GetComponent<PhotonView>();
        gunImage.sprite = gunUI[0];
        isAutomaticFire = false;
    }
    private void Update()
    {
        if (pv.IsMine)
        {
            SwitchTwoGuns();
            BulletsText();
            Reload();
            Attack();
        }
    }
    private void Attack()
    {
        if (Input.GetKey(KeyCode.B))
        {
            isAutomaticFire = true;
        }
        // 如果枪械为单发模式
        if (Input.GetMouseButtonDown(0) && !isAutomaticFire && currentHandGun != null)
        {
            Debug.Log("单点");
            Shoot();
        }
        //如果枪为自动模式
        if (Input.GetMouseButton(0) && isAutomaticFire == true && currentHandGun != null)
        {
            Debug.Log("全自动");
            if (Time.time - lastFired > 1 / fireRate)
            {
                Shoot();
                lastFired = Time.time;
            }
        }
    }
    //显示当前枪的子弹数
    private void BulletsText()
    {
        if (currentHandGun != null)
        {
            switch (currentHandGun.tag)
            {
                case "sinper":
                    bulletsAmountText.text = currentHandGun.GetComponent<Sinper>().scarBullets.ToString() + "/5";
                    break;
                case "scar":
                    bulletsAmountText.text = currentHandGun.GetComponent<Scar>().scarBullets.ToString() + "/30";
                    break;
                case "akm":
                    bulletsAmountText.text = currentHandGun.GetComponent<Akm>().scarBullets.ToString() + "/30";
                    break;
                case "smg":
                    bulletsAmountText.text = currentHandGun.GetComponent<Smg>().scarBullets.ToString() + "/25";
                    break;
                case "lever":
                    bulletsAmountText.text = currentHandGun.GetComponent<Lever>().scarBullets.ToString() + "/2";
                    break;
            }
        }
    }
}
