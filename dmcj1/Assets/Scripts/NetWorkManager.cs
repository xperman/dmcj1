using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NetWorkManager : MonoBehaviourPunCallbacks
{
    /*This is the third time i have written this script. I have dropped so many ' PITS '. However i believe this time is the last time.*/

    // Start Match button.
    [Header("Creat room")]
    [SerializeField] private Button startMatch;
    // Join a room button.
    [Header("Join random room")]
    [SerializeField] private Button joinRandomRoom;

    // The panel while we have connected master
    [Header("Lobby panel")]
    [SerializeField] private GameObject nextPanel;

    // Contains two gameObject(the load image and game title)
    [Header("Login panel")]
    [SerializeField] private GameObject[] initalState;

    //Player head images
    [Header("Player head icon")]
    [SerializeField] private Image[] headImage;

    // We can see some player at the current room 
    [Header("Current panel in the room")]
    [SerializeField] private GameObject playersPanel;

    //Custom your name
    [Header("Custom your nickname")]
    [SerializeField] private InputField playerName;

    //Enter your name
    [Header("Confirm your nickname")]
    [SerializeField] private Button enterYourName;

    //Load the Sand Box scene
    [Header("Exercise scene")]
    [SerializeField] private Button sandBox;

    //BatttleGround scene
    [Header("BattleGround scene")]
    [SerializeField] private Button battleground;

    //Selete map panel
    [Header("Selete room panel")]
    [SerializeField] private GameObject seletePanel;

    //Current room players amount
    [Header("Current room players amount")]
    [SerializeField] private Text currentPlayers;

    [SerializeField] private GameObject currentPlayer;
    [SerializeField] private GameObject matchInformation;

    //Whether to allow us to join the random room
    private bool joinRoomIs;

    private bool roomIs;

    private int mapNums = 0;

    private void Start()
    {
        //All client can update same scene with you.
        PhotonNetwork.AutomaticallySyncScene = true;
        //At the begin we try connect server firstly by the define setting document in the resources file.
        PhotonNetwork.ConnectUsingSettings();

        joinRoomIs = false;
        roomIs = false;

        //we creat a room that allow others to join it.
        startMatch.onClick.AddListener(CreatRoom);

        // join a random if prossible.
        joinRandomRoom.onClick.AddListener(JoinRoom);
        enterYourName.onClick.AddListener(() => { PhotonNetwork.NickName = playerName.text; });
        //Listen these two buttons
        sandBox.onClick.AddListener(() => { PhotonNetwork.LoadLevel(1); playersPanel.SetActive(true); currentPlayer.SetActive(true); });
        battleground.onClick.AddListener(() => { PhotonNetwork.LoadLevel(2); playersPanel.SetActive(true); currentPlayer.SetActive(true); });
    }

    private void Update()
    {
        if (roomIs == true)
        {
            headImage[PhotonNetwork.CurrentRoom.PlayerCount].gameObject.SetActive(true);
            currentPlayers.text = PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " / " + 20;
        }
    }

    private void CreatRoom()
    {
        if (joinRoomIs == true && PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 10 }, null, null);
        }
        else
        {
            Debug.Log("You can't creat a room, because you don't connected the master");
        }
    }

    private void JoinRoom()
    {
        if (joinRoomIs == true)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("There is no any open random room to join");
        }
    }

    public override void OnCreatedRoom()
    {
        //it creat successfully then there is will show a selete map panel
        seletePanel.SetActive(true);

        Debug.Log(" Successfully created a room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        matchInformation.SetActive(true);
        StartCoroutine("HideMatchInfor");
        Debug.Log("Join a random room field");
    }

    public override void OnJoinedRoom()
    {
        roomIs = true;
        //PhotonNetwork.LoadLevel(1);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We have connected master successfully");
        // You can join a random room now
        joinRoomIs = true;
        // Show the "nextPanel" in the scene
        nextPanel.SetActive(true);
        // Hide the "initalState" in the scene       
        initalState[0].SetActive(false);
        initalState[1].SetActive(false);
    }

    IEnumerator HideMatchInfor()
    {
        yield return new WaitForSeconds(2f);
        matchInformation.SetActive(false);
    }
}
