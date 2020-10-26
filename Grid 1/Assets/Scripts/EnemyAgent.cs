using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    private NavMeshAgent agent;

    // Use this for initialization
    void Awake () 
    {
        agent = GetComponent<NavMeshAgent> ();    
    }

    public void MoveToLocation(Vector3 targetPoint)
    {
        agent.destination = targetPoint;
        agent.isStopped = false;
    }
    void OnTriggerEnter(Collider other) {
        Debug.Log("Destroy!!!");
        Destroy(gameObject);
    }
}
