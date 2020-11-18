using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharacterAgent : MonoBehaviour {

    private NavMeshAgent agent;
    private Animator animator;
    public int animationState = 0;
    public float rotSpeed = 10.0f;

    public List<GameObject> rangeList = new List<GameObject>();
    public List<GameObject> bufferList = new List<GameObject>();
    public GameObject attackTarget = null;
    public bool destination = false;
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
        // Target is selected. Move to or attack target.
        if (attackTarget)
        {
            // Target is in attacking range
            if (rangeList.Contains(attackTarget))
            {
                //// Directed attack ////
                if(!attacking)
                {
                    StopCoroutine(methodName: "Attack");
                    StartCoroutine(methodName: "Attack",value: attackTarget);
                }
            }
            // Target has left the buffer or the attack has ended
            else if ((!bufferList.Contains(attackTarget)) || (!attacking))
            {
                //// Move to attackTarget ////
                if(attacking)
                {
                    StopCoroutine(methodName: "Attack");
                    attacking = false;
                }
                MoveToLocation(attackTarget.transform.position);
            }
        }
        
        // Target is not selected, but destinon is set.
        else if (destination)
        {
            // Check if destination is reached.
            //// Idle ////
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    // Agent has reached destination. Set to false and set idle animation.
                    animationState = 0;
                    animator.SetInteger("state", animationState);
                    destination = false;
                }
            }
            //// Moving ////
            else if(animationState != 1)
            {
                // A destination is set and agent is moving. Set animation to running.
                animationState = 1;
                animator.SetInteger("state", animationState);
            }
        }

        // There is no attackTarget or destination set
        else
        {
            //// Auto Attack ////
            if((rangeList.Count>0) && (autoAttack))
            {
                if(rangeList[0])
                {
                    attackTarget = rangeList[0];
                }
                else
                {
                    rangeList.RemoveAt(0);
                }

                if(bufferList.Count>0)
                {
                    if(!bufferList[0])
                    {
                        bufferList.RemoveAt(0);
                    }
                }
            }
            //// Idle ////
            else
            {
                if(animationState != 0)
                {
                    // Not attacking or moving. Set animation to idle.
                    animationState = 0;
                    animator.SetInteger("state", animationState);
                }
            }
        }
        InstantlyTurn(agent.steeringTarget);
    }

    //// Control Commands ////

    public void MoveToLocation(Vector3 targetPoint)
    {
        if(attacking)
        {
            CancelAttack();
        }
        if(animationState != 1)
        {
            animationState=1;
            animator.SetInteger("state", animationState);
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
        //animationState=0;
        //animator.SetInteger("state", animationState);
        //Debug.Log("Attack Cancelled");
    }

    IEnumerator Attack(GameObject target)
    {
        attacking = true;
        // Stop navigating
        agent.ResetPath();
        // Turn towards enemy
        Vector3 targetDirection = target.transform.position - transform.position;
        while(Vector3.Angle(transform.forward, targetDirection) > 1.0f)
        {
            float singleStep = rotSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        }
        // Beging attack sequence
        animationState=2;
        animator.SetInteger("state", animationState);
        yield return new WaitForSeconds(0.87f);
        if(target)
        {
            if(bufferList.Contains(target))
            {
                // Eventually pull damage from Stats script
                target.GetComponent<EnemyStats>().TakeDamage(20);
            }
        }
        //Debug.Log("Attacked " + target.name);
        animationState=0;
        animator.SetInteger("state", animationState);
        attacking = false;
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

    public void SetAttackTarget(GameObject target)
    {
        if(target)
        {
            attackTarget = target;
        }
        
    }

}