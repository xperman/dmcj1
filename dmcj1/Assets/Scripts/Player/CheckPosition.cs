using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPosition : MonoBehaviour
{
    public void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.tag=="positionCircle")
        {
            Debug.Log("触碰到毒圈");
        }
    }
   
}
