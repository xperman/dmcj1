using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.rotation = Quaternion.Euler(new Vector3(90, target.eulerAngles.y, 0));
    }
}
