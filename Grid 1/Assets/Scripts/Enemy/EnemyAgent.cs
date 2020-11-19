using System.Collections;
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
    public float rotSpeed = 10.0f;
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
        // Target is selected. Check if target is in range.
        if(currentTarget)
        {
            // If enemy is attacked by someone 
            if(aggroAttackTarget)
            {
                // If they are currently attacking a structure and the attacker is in their aggro range
                //if((currentTarget.gameObject.tag == "Structure")&&(aggroRangeList.Contains(aggroAttackTarget)))
                if(currentTarget.gameObject.tag == "Structure")
                {
                    currentTarget = aggroAttackTarget;
                    if(currentTarget.transform.position != lastTargetPosition)
                    {
                        MoveToLocation(currentTarget.transform.position);
                        lastTargetPosition = currentTarget.transform.position;
                    }
                }
            }
            // Target is in aggro range.
            if(aggroRangeList.Contains(currentTarget))
            {
                // Target is in attack range.
                if(rangeList.Contains(currentTarget))
                {
                    if(!attacking)
                    {
                        StopCoroutine(methodName: "Attack");
                        StartCoroutine(methodName: "Attack",value: currentTarget);
                    }
                }
                // Target is outside of buffer or target is outside of attack range and not being attacked
                else if ((!bufferList.Contains(currentTarget)) || (!attacking))
                {
                    // Target moves outside of buffer while attack routine is happening. Cancel the attack.
                    if(attacking)
                    {
                        StopCoroutine(methodName: "Attack");
                        attacking = false;
                    }
                    // Check if the target has moved and move to the new location
                    else if(currentTarget.transform.position != lastTargetPosition)
                    {
                        MoveToLocation(currentTarget.transform.position);
                        lastTargetPosition = currentTarget.transform.position;
                    }
                }
            }
            // Target has left the aggro range. Forget as current target.
            else if (!aggroAttackTarget)
            {
                currentTarget = null;
            }
        }
        // No target is selected, but there are targets within aggro range. Select new target.
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
        // Nothing is within aggro range, but aggro is drawn from an attack outside of aggro range.
        else if(aggroAttackTarget)
        {
            if(currentTarget != aggroAttackTarget)
            {
                MoveToLocation(aggroAttackTarget.transform.position);
                lastTargetPosition = aggroAttackTarget.transform.position;
            }
        }
        // There are no targets. Move towards default location.
        else if(basePos != lastTargetPosition)
        {
            MoveToLocation();
            lastTargetPosition = basePos;
        }
        // Enemy is at the target location and not attacking. Set to idle animation.
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
        
        Vector3 targetDirection = destination - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
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
            aggroAttackTarget = target;
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
        animationState=0;
        animator.SetInteger("state", animationState);
        // Turn towards target
        Vector3 targetDirection = target.transform.position - transform.position;
        while(Vector3.Angle(transform.forward, targetDirection) > 20.0f)
        {
            float singleStep = rotSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        }
        // Begin attack sequence
        animationState=2;
        animator.SetInteger("state", animationState);
        yield return new WaitForSeconds(1.75f);
        //Debug.Log("Attacked " + target.name);
        animationState=0;
        animator.SetInteger("state", animationState);
        yield return new WaitForSeconds(0.5f);
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
