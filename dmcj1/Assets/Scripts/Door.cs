using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Door : MonoBehaviour
{
    private PhotonView pv;
    public Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        pv = this.GetComponent<PhotonView>();      
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    [PunRPC]
    public void Open()
    {
        myAnimator.SetBool("IsOpen", true);
    }
}
