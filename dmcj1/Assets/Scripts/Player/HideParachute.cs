using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HideParachute : MonoBehaviour
{

    private CharacterController myCharacterController;

    public GameObject parachuteCamera;
    public GameObject parachute;
    //人物完整模型
    public GameObject model;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        // myCharacterController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(myCharacterController.isGrounded==true)
        //{


        //    //this.GetComponent<HideParachute>().enabled = false;
        //}
    }
    private bool s = true;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "buildings"&&s==true)
        {
            parachute.SetActive(false);
            parachuteCamera.SetActive(false);
            model.SetActive(false);
            PhotonNetwork.Instantiate(player.name, transform.position, transform.rotation, 0);
            s = false;
        }
        return;
    }
}
