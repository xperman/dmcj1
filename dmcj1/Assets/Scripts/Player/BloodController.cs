using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodController : MonoBehaviour
{   
    void Start()
    {
        //血粒子特效过两秒消失
        Destroy(this.gameObject, 2);
    }
}
