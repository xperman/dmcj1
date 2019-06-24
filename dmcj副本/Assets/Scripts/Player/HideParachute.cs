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

    public float rotateSpeed;

    // Update is called once per frame
    void Update()
    {     
        parachuteCamera.transform.Rotate(-Input.GetAxis("Mouse Y") * rotateSpeed, Input.GetAxis("Mouse X") * rotateSpeed, 0);
    }
    private bool coll = true;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "buildings" && coll == true)
        {
            parachute.SetActive(false);
            parachuteCamera.SetActive(false);
            model.SetActive(false);
            PhotonNetwork.Instantiate(player.name, transform.position, transform.rotation, 0);
            coll = false;
        }
        return;
    }
}
