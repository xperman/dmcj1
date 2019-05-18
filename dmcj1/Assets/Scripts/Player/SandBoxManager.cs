using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SandBoxManager : MonoBehaviour
{
    // The player prefabs in rescource file
    public GameObject player;
    // Player spawn position
    public Transform[] playerSpawnPos;
    // Start is called before the first frame update
    void Start()
    {

        PhotonNetwork.Instantiate(player.name, playerSpawnPos[Random.Range(0, 5)].position, Quaternion.identity, 0);
    }
}
