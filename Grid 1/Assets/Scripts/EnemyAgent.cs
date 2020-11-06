using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private int animationState = 0;
    private Vector3 basePos;
    public Vector3 currentTarget;
    public Vector3 lastTarget;
    public List<GameObject> aggroRangeList = new List<GameObject>();
    public GameObject aggroAttackTarget;
    public float rotSpeed = 0.1f;
    
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
        if(aggroRangeList.Count > 0)
        {
            if(aggroRangeList[0])
            {
                currentTarget = aggroRangeList[0].transform.position;
            }
            else
            {
                aggroRangeList.RemoveAt(0);
            }
        }
        else if(aggroAttackTarget)
        {
            currentTarget = aggroAttackTarget.transform.position;
        }
        else
        {
            currentTarget = basePos;
        }
    
        if(currentTarget!=lastTarget)
        {
            MoveToLocation(currentTarget);
            lastTarget = currentTarget;
        }

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

    //// Attack Logic ////
}
