using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class UIUser : MonoBehaviour
{
    private PhotonView pv;

    // Whither is the backPanel close or open.
    private bool isBack = true;

    private void Start()
    {
        pv = this.GetComponent<PhotonView>();
        this.GetComponent<UIManager>().backGame.onClick.AddListener(() => { this.GetComponent<UIManager>().backPanel.SetActive(false); this.GetComponent<UIManager>().miniMap.SetActive(true); });
        this.GetComponent<UIManager>().backLobby.onClick.AddListener(() => { SceneManager.LoadScene(0); });
    }

    void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isBack == true)
                {
                    this.GetComponent<UIManager>().backPanel.SetActive(true);
                    this.GetComponent<RotateView>().enabled = false;
                    this.GetComponent<ControlGuns>().enabled = false;
                    this.GetComponent<UIManager>().aimImage.gameObject.SetActive(false);
                    //this.GetComponent<UIManager>().miniMap.SetActive(false);
                    isBack = false;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    this.GetComponent<UIManager>().backPanel.SetActive(false);
                    this.GetComponent<RotateView>().enabled = true;
                    this.GetComponent<ControlGuns>().enabled = true;
                    this.GetComponent<UIManager>().aimImage.gameObject.SetActive(true);
                   // this.GetComponent<UIManager>().miniMap.SetActive(true);
                    isBack = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }
    }
}
