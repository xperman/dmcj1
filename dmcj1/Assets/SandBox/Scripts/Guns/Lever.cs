using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    //30发子弹
    private int bulletsAmount = 2;
    //是否可以射击
    private bool isShoot;
    public int scarBullets { get { return bulletsAmount; } set { bulletsAmount = value; } }
    public bool IsShoot { get { return isShoot; } }
    // 0 : 开火声音 1 :卡壳声音
    public AudioClip[] scarAudioClips;
    public AudioSource scarSource;
    public Animator gunAnimator;
    public Animator gunAnimatorRemove;
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
        }
    }

    public void Reload()
    {
        gunAnimator.SetTrigger("Reload");
        gunAnimatorRemove.SetTrigger("Reload");
        bulletsAmount = 2;
        
        scarSource.clip = scarAudioClips[2];
        scarSource.Play();
    }
}
