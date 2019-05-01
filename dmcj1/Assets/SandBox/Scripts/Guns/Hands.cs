using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    private Animator handAnimator;
    public Animator gunAnimatorRemove;
    void Start()
    {
        handAnimator = this.GetComponent<Animator>();
    }

    public void Attack()
    {
        handAnimator.SetTrigger("shoot");
    }

}
