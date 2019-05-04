using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    //30发子弹
    public int bulletsAmount = 2;
    //备用子弹数
    public int backupBullets = 10;

    // 0 : 开火声音 1 :卡壳声音
    public AudioClip[] scarAudioClips;
    public AudioSource scarSource;
    public Animator gunAnimator;
    public Animator gunAnimatorRemove;
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
            Debug.Log("ssss");
        }
    }

    public void Reload()
    {
        if (backupBullets <= 0)
        {
            Debug.Log("无法换单");
            return;
        }
        else if (backupBullets > 0 && backupBullets < 2)
        {
            bulletsAmount = backupBullets;
        }
        else
        {
            bulletsAmount = backupBullets - (backupBullets - 2);
            backupBullets = backupBullets - 2;
        }
        gunAnimator.SetTrigger("Reload");
        gunAnimatorRemove.SetTrigger("Reload");      
        scarSource.clip = scarAudioClips[2];
        scarSource.Play();
    }
}
