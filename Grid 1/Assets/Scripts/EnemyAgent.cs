﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 basePos;
    
    public Vector3 BasePos
    {
        get {return basePos;}
        set {basePos = value;}
    }


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
    public void MoveToLocation()
    {
        agent.destination = basePos;
        agent.isStopped = false;
    }
    /*void OnTriggerEnter(Collider other) {
        Debug.Log("Destroy!!!");
        Destroy(gameObject);
    }*/
    public void AgroStart(GameObject target)
    {
        MoveToLocation(target.transform.position);
    }
    public void AgroStop(GameObject target)
    {
        MoveToLocation();
    }
}