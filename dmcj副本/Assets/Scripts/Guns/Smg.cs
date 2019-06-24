using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Smg : MonoBehaviour
{
    //30发子弹
    public int bulletsAmount = 25;
    //备用子弹数
    public int backupBullets = 50;
    //是否可以射击
    private bool isShoot;
    // 0 : 开火声音 1 :卡壳声音
    public AudioClip[] scarAudioClips;
    public AudioSource scarSource;
    public Animator gunAnimator;
    public Animator gunAnimatorRemove;
    public PhotonView pv;
    //枪口火焰
    public GameObject muzzle;
    //枪口火焰的位置
    public Transform muzzlePos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponentInParent<UIManager>().bulletsAmountText.text = bulletsAmount.ToString();
        this.GetComponentInParent<UIManager>().backupBulletsAmounts.text = backupBullets.ToString();

    }

    public void UseBullets()
    {
        if (bulletsAmount <= 0)
        {
            scarSource.clip = scarAudioClips[1];
            scarSource.Play();
        }
        else
        {
            bulletsAmount--;
            scarSource.clip = scarAudioClips[0];
            scarSource.Play();
            gunAnimator.SetTrigger("Shoot");
            gunAnimatorRemove.SetTrigger("Shoot");
            pv.RPC("ShowMuzzle", RpcTarget.AllBuffered);
        }
    }
    /// <summary>
    /// 生成枪口火花
    /// </summary>
    /// <param name="pos"></param>
    [PunRPC]
    public void ShowMuzzle()
    {
        muzzle.SetActive(true);
        StartCoroutine("HideMuzzle");
    }
    public void Reload()
    {
        if (backupBullets <= 0)
        {
            Debug.Log("无法换单");
            return;
        }
        else if (backupBullets > 0 && backupBullets < 25)
        {
            bulletsAmount = backupBullets;
            backupBullets = 0;
        }
        else
        {
            bulletsAmount = backupBullets - (backupBullets - 25);
            backupBullets = backupBullets - 25;
        }
        gunAnimator.SetTrigger("Reload");
        gunAnimatorRemove.SetTrigger("Reload");
        scarSource.clip = scarAudioClips[2];
        scarSource.Play();
    }

    IEnumerator HideMuzzle()
    {
        yield return new WaitForSeconds(0.2f);
        muzzle.SetActive(false);
    }
}
