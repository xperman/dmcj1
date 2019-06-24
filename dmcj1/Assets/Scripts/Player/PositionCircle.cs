using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionCircle : MonoBehaviour
{
    //毒圈缩减速度
    public float narrowSpeed;
    //毒圈出现时间
    private float appearTime = 90f;
    //毒圈出现时间Text
    public Text positionText;

    private bool timeOut;
    // Start is called before the first frame update
    void Start()
    {
        timeOut = false;
        narrowSpeed = 0.001f;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        appearTime -= Time.deltaTime;
        int translateTime = (int)appearTime;
        positionText.text = "毒圈出现时间： " + translateTime;
        if (translateTime <= 0f)
        {
            InitialCircle(narrowSpeed);
            appearTime = 0;
            timeOut = true;
        }
    }

    private void InitialCircle(float speed)
    {
        positionText.gameObject.SetActive(true);
        Vector3 currentScale = CircleReduce(speed);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<MeshCollider>().enabled = true;
        Debug.Log(currentScale);
    }

    private Vector3 CircleReduce(float speed)
    {
        float s_x = transform.localScale.x;
        float s_y = transform.localScale.y;
        float s_z = transform.localScale.z;
        Vector3 newScale = new Vector3(s_x -= Time.deltaTime * speed, s_y -= Time.deltaTime * speed, s_z);
        transform.localScale = newScale;
        return transform.localScale;
    }
}
