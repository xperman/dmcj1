using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EmemyControl : MonoBehaviour
{

    private NavMeshAgent meshAgent;

    // Start is called before the first frame update
    void Start()
    {
        meshAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
