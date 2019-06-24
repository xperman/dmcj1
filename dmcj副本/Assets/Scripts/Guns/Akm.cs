using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Akm : MonoBehaviour
{
    //30发子弹
    public int bulletsAmount = 30;
    //备用子弹数
    public int backupBullets = 30;
    public AudioClip[] scarAudioClips;
    public AudioSource scarSource;
    public Animator gunAnimator;
    public Animator gunAnimatorRemove;
    public PhotonView pv;
    public int ClipSize = 30;
    //枪口火焰
    public GameObject muzzle;
    //枪口火焰的位置
    public Transform muzzlePos;

    void Update()
    {
        this.GetComponentInParent<UIManager>().bulletsAmountText.text = bulletsAmount.ToString();
        this.GetComponentInParent<UIManager>().backupBulletsAmounts.text = backupBullets.ToString();
    }
    public void useBullets()
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

    public void Reload()
    {
        if (backupBullets <= 0)
        {
            Debug.Log("无法换弹");
            return;
        }
        else if (backupBullets > 0 && backupBullets < 30)
        {
            bulletsAmount = backupBullets;
        }
        else
        {
            bulletsAmount = backupBullets - (backupBullets - 30);
            backupBullets = backupBullets - 30;

        }
        gunAnimator.SetTrigger("Reload");
        gunAnimatorRemove.SetTrigger("Reload");
        pv.RPC("PlayAudio", RpcTarget.AllBuffered, 2);
    }
    /// <summary>
    /// 生成枪口火花
    /// </summary>
    /// <param name="pos"></param>
    [PunRPC]
    public void ShowMuzzle()
    {
        muzzle.SetActive(true);
        //StartCoroutine("HideMuzzle");
    }
    /// <summary>
    /// 播放枪声
    /// </summary>
    /// <param name="state"></param>
    [PunRPC]
    public void PlayAudio(int state)
    {
        if (state == 0)
        {
            scarSource.clip = scarAudioClips[1];
            scarSource.Play();
        }
        else if (state == 1)
        {
            scarSource.clip = scarAudioClips[0];
            scarSource.Play();
        }
        else if (state == 2)
        {
            scarSource.clip = scarAudioClips[2];
            scarSource.Play();
        }
    }
    IEnumerator HideMuzzle()
    {
        yield return new WaitForSeconds(0.2f);
        muzzle.SetActive(false);
    }
}
