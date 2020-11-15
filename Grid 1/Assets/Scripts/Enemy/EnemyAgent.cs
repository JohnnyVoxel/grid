﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public int animationState = 0;
    private Vector3 basePos;
    public GameObject currentTarget;
    public Vector3 lastTargetPosition;
    public List<GameObject> aggroRangeList = new List<GameObject>();
    public List<GameObject> rangeList = new List<GameObject>();
    public List<GameObject> bufferList = new List<GameObject>();
    public GameObject aggroAttackTarget;
    public float rotSpeed = 0.1f;
    public bool attacking = false;
    
    public Vector3 BasePos
    {
        get {return basePos;}
        set {basePos = value;}
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("state", animationState);
    }

    void Awake () 
    {
        agent = GetComponent<NavMeshAgent> ();
        agent.updateRotation = false;  
    }

    void Update()
    {        
        if(currentTarget)
        {
            if(aggroRangeList.Contains(currentTarget))
            {
                if(rangeList.Contains(currentTarget))
                {
                    if(!attacking)
                    {
                        StopCoroutine(methodName: "Attack");
                        StartCoroutine(methodName: "Attack",value: currentTarget);
                    }
                }
                else if ((!bufferList.Contains(currentTarget)) || (!attacking))
                {
                    if(attacking)
                    {
                        StopCoroutine(methodName: "Attack");
                        attacking = false;
                    }
                    else if(currentTarget.transform.position != lastTargetPosition)
                    {
                        MoveToLocation(currentTarget.transform.position);
                        lastTargetPosition = currentTarget.transform.position;
                    }
                }
            }
            else
            {
                currentTarget = null;
            }
        }
        else if(aggroRangeList.Count > 0)
        {
            if(aggroRangeList[0])
            {
                currentTarget = aggroRangeList[0];
            }
            else
            {
                aggroRangeList.RemoveAt(0);
            }
        }
        else if(aggroAttackTarget)
        {
            if(currentTarget != aggroAttackTarget)
            {
                MoveToLocation(aggroAttackTarget.transform.position);
                lastTargetPosition = aggroAttackTarget.transform.position;
            }
        }
        else if(basePos != lastTargetPosition)
        {
            MoveToLocation();
            lastTargetPosition = basePos;
        }
        else if ((!agent.pathPending)&&(!attacking))
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
        animationState = 1;
        animator = GetComponent<Animator>();
        animator.SetInteger("state", animationState);
        agent.destination = targetPoint;
        agent.isStopped = false;
    }
    public void MoveToLocation()
    {
        animationState = 1;
        animator = GetComponent<Animator>();
        animator.SetInteger("state", animationState);
        agent.destination = basePos;
        agent.isStopped = false;
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

    //// Aggro Logic ////

    public void AddRangeAggro(GameObject target)
    {
        if(!aggroRangeList.Contains(target))
        {
            aggroRangeList.Add(target);
        }
    }

    public void RemoveRangeAggro(GameObject target)
    {
        if(aggroRangeList.Contains(target))
        {
            aggroRangeList.Remove(target);
        }
    }

    public void AddAttackAggro(GameObject target)  //Assuming target will be the gameObject of the character, not the projectile
    {
        if(!aggroRangeList.Contains(target))
        {
            if(aggroAttackTarget == target)
            {
                StopCoroutine("AggroForget");   // Calling as a string causes the coroutine to restart instead of resuming... because reasons.
                StartCoroutine("AggroForget");
            }
            else
            {
                aggroAttackTarget = target;
                StartCoroutine("AggroForget");
            }
            
        }
        else if(aggroRangeList.IndexOf(target) > 0)
        {
            aggroRangeList.Remove(target);
            aggroRangeList.Insert(0, target);
        }
    }

    IEnumerator AggroForget()
    {
        yield return new WaitForSeconds(4);
        aggroAttackTarget = null;
    }

    IEnumerator Attack(GameObject target)
    {
        attacking = true;
        // Stop navigating
        agent.ResetPath();
        // Turn towards target
        Vector3 targetDirection = target.transform.position - transform.position;
        //transform.rotation = Quaternion.LookRotation(targetDirection);
        animationState=2;
        animator.SetInteger("state", animationState);
        yield return new WaitForSeconds(1.75f);
        Debug.Log("Attacked " + target.name);
        animationState=0;
        animator.SetInteger("state", animationState);
        yield return new WaitForSeconds(1.0f);
        attacking = false;
    }

    //// Attack Logic ////

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

    public void AddRangeBuffer(GameObject target)
    {
        if(!bufferList.Contains(target))
        {
            bufferList.Add(target);
        }
    }

    public void RemoveRangeBuffer(GameObject target)
    {
        if(bufferList.Contains(target))
        {
            bufferList.Remove(target);
        }
    }
}