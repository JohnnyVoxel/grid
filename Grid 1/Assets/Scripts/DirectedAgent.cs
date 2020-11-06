using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DirectedAgent : MonoBehaviour {

    private NavMeshAgent agent;
    private Animator animator;
    public int animationState = 0;
    public float rotSpeed = 0.1f;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("state", animationState);
    }
    // Use this for initialization
    void Awake () 
    {
        agent = GetComponent<NavMeshAgent> ();
        agent.updateRotation = false;    
    }

    void Update() {
        // Check if we've reached the destination
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    animationState = 0;
                    animator.SetInteger("state", animationState);
                }
            }
        }
        InstantlyTurn(agent.steeringTarget);
    }

    public void MoveToLocation(Vector3 targetPoint)
    {
        agent.destination = targetPoint;
        animationState = 1;
        animator.SetInteger("state", animationState);
        agent.isStopped = false;
    }

     private void InstantlyTurn(Vector3 destination) 
     {
        //When on target -> dont rotate!
        if ((destination - transform.position).magnitude < 0.1f) return; 
        
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion  qDir= Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * rotSpeed);
    }
}