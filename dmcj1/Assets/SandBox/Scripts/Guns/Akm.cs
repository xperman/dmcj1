using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Akm : MonoBehaviour
{
    //30发子弹
    private int bulletsAmount = 30;
    //是否可以射击
    private bool isShoot;
    public int scarBullets { get { return bulletsAmount; } set { bulletsAmount = value; } }
    public bool IsShoot { get { return isShoot; } }
    // 0 : 开火声音 1 :卡壳声音
    public AudioClip[] scarAudioClips;
    public AudioSource scarSource;
    public Animator gunAnimator;
    public Animator gunAnimatorRemove;
    public PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        isShoot = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void useBullets()
    {

        if (bulletsAmount <= 0)
        {
            isShoot = false;
            pv.RPC("PlayAudio", RpcTarget.AllBuffered, 0);
        }
        else
        {
            bulletsAmount--;

            gunAnimator.SetTrigger("Shoot");
            gunAnimatorRemove.SetTrigger("Shoot");
            pv.RPC("PlayAudio", RpcTarget.AllBuffered, 1);
        }
    }

    public void Reload()
    {
        gunAnimator.SetTrigger("Reload");
        gunAnimatorRemove.SetTrigger("Reload");
        bulletsAmount = 30;
        pv.RPC("PlayAudio", RpcTarget.AllBuffered, 2);
    }

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
}
