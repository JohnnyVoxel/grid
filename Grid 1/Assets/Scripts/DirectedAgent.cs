using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DirectedAgent : MonoBehaviour {

    private NavMeshAgent agent;
    private Animator animator;
    public int animationState = 0;
    public float rotSpeed = 0.1f;

    public List<GameObject> rangeList = new List<GameObject>();
    public GameObject attackTarget = null;
    public bool destination = false;
    public bool idle = true;
    public bool attacking = false;
    private bool autoAttack = true;

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

    void Update() 
    {
        // Target is selected. Move to or attack target
        if (attackTarget)
        {
            if (rangeList.Contains(attackTarget))
            {
                //// Attack routine ////
                if(!attacking)
                {
                    //Debug.Log("Target in range");
                    //attackTarget = rangeList[0];
                    StopCoroutine(methodName: "Attack");
                    StartCoroutine(methodName: "Attack",value: attackTarget);
                }
            }
            else
            {
                // Move to attackTarget
                MoveToLocation(attackTarget.transform.position);
            }
            idle = false;
        }
        
        // Target is not selected, but destinon is set.
        // Check if we've reached the destination
        else if (destination)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // Agent has reached destination. Set to false and set idle animation.
                    animationState = 0;
                    animator.SetInteger("state", animationState);
                    destination = false;
                    idle = true;
                }
            }
            else if(animationState != 1)
            {
                // A destination is set and agent is moving. Set animation to running.
                animationState = 1;
                animator.SetInteger("state", animationState);
                idle = false;
            }
        }

        // Agent has reached its destination, there are enemies in attack range, and AA is enabled
        else if((!destination) && (rangeList.Count>0) && (autoAttack))
        {
            //Debug.Log("Idle with AA");
            if(rangeList[0])
            {
                attackTarget = rangeList[0];
            }
            else
            {
                rangeList.RemoveAt(0);
            }
        }
        else
        {
            idle = true;
        }
        InstantlyTurn(agent.steeringTarget);
    }

    //// Public Commands ////

    public void MoveToLocation(Vector3 targetPoint)
    {
        if(attacking)
        {
            CancelAttack();
        }
        agent.destination = targetPoint;
        agent.isStopped = false;
        destination = true;
    }

    public void CancelAttack()
    {
        StopCoroutine(methodName: "Attack");
        attackTarget = null;
        attacking = false;
        animationState=0;
        animator.SetInteger("state", animationState);
        //Debug.Log("Attack Cancelled");
    }

    IEnumerator Attack(GameObject target)
    {
        // Stop navigating
        agent.ResetPath();
        // Turn towards target
        Vector3 targetDirection = target.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(targetDirection);

        attacking = true;
        animationState=2;
        animator.SetInteger("state", animationState);
        yield return new WaitForSeconds(2.2f);
        //Debug.Log("Attacked " + target.name);
        attacking = false;
        animationState=0;
        animator.SetInteger("state", animationState);

    }

    //// Rotation ////

    private void InstantlyTurn(Vector3 destination) 
    {
        //When on target -> dont rotate!
        if ((destination - transform.position).magnitude < 0.1f) return; 
        
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion  qDir= Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * rotSpeed);
    }

    //// Attacking Logic ////

    public void AddRangeTarget(GameObject target)
    {
        if(!rangeList.Contains(target))
        {
            rangeList.Add(target);
        }
    }

    public void RemoveRangeTarget(GameObject target)
    {
        if(rangeList.Contains(target))
        {
            rangeList.Remove(target);
        }
    }

    public void SetAttackTarget(GameObject target)
    {
        if(target)
        {
            attackTarget = target;
        }
        
    }

}