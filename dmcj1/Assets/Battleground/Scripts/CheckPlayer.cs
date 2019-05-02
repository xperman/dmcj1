using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CheckPlayer : MonoBehaviour
{
    public GameObject bulletHole;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //调用敌人的减血代码
            other.gameObject.GetComponent<PhotonView>().RPC("DamageGet", RpcTarget.AllBuffered, 10, other.gameObject.transform.position);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "buildings")
        {
            //生成一个临时弹孔
            GameObject tempHole = Instantiate(bulletHole, other.gameObject.transform.position, Quaternion.FromToRotation(Vector3.forward, other.gameObject.transform.position.normalized));
            Destroy(tempHole, 0.3f);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * 100 * Time.deltaTime);
    }
}
