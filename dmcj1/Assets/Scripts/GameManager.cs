using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    //The player 
    public GameObject playerPrefab;
    //Player spawn
    public Transform[] spawnPositions;
    //Game begin time down
    public Text timeDown;
    //airplane
    public GameObject airPlane;

    public float flySpeed = 30f;

    private bool onSeat;

    private void Start()
    {
        onSeat = true;
    }

    private void Update()
    {
        airPlane.SetActive(true);
        airPlane.transform.Translate(transform.forward * Time.deltaTime * flySpeed);
        if (Input.GetKeyDown(KeyCode.F) && onSeat == true)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, airPlane.transform.position, Quaternion.identity);
            onSeat = false;
        }
    }
}
