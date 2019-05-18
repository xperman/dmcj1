using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Parachute : MonoBehaviour
{
    //飞机
    public GameObject playerPrefabs;
    public Transform spawnPos;
    private PhotonView pv;
    private GameObject tempPlane;


    void Start()
    {
        pv = GetComponent<PhotonView>();
        tempPlane = PhotonNetwork.Instantiate(playerPrefabs.name, spawnPos.position, spawnPos.rotation, 0);
        StartCoroutine("HidePlane");
    }

    private void Update()
    {
        tempPlane.transform.Translate(transform.forward * 20f * Time.deltaTime, Space.Self);
    }

    IEnumerator HidePlane()
    {
        yield return new WaitForSeconds(70f);
        Destroy(tempPlane);
    }
}
