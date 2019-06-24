using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class CheckPosition : MonoBehaviour
{

    private PhotonView pv;
    private void Start()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="position")
        {
          
        }
    }
    public void DamageByPosition()
    {

    }
   
}
