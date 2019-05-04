using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CustomBackground : MonoBehaviour
{
    public GameObject circlePrefabs;
    private GameObject tempCircle;
    //毒圈生成的时间
    private int circleBeginTime;


    // Start is called before the first frame update
    void Start()
    {
         tempCircle = PhotonNetwork.InstantiateSceneObject(circlePrefabs.name, Vector3.zero, Quaternion.identity, 0, null);

    }
    void Update()
    {
        tempCircle.transform.localScale = new Vector3(Time.deltaTime * -1, Time.deltaTime * -1, Time.deltaTime * -1);
    }
}
