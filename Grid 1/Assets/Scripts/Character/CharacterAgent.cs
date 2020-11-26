using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharacterAgent : MonoBehaviour {

    private NavMeshAgent agent;
    private Animator animator;
    private BoardController board;
    public float rotSpeed = 10.0f;

    public List<GameObject> rangeList = new List<GameObject>();
    public List<GameObject> bufferList = new List<GameObject>();
    public GameObject attackTarget = null;
    public bool destination = false;
    public bool attacking = false;
    private bool autoAttack = true;

    private Vector3 currentPosition;
    private Vector3 lastPosition;
    private GameObject currentTile;
    private GameObject lastTile;

    private bool basicAttackEnabled = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        board = GameObject.Find("Board").GetComponent<BoardController>();
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
            // Check if target has been killed by seeing if its collider is disabled
            if (!attackTarget.transform.Find("Body").GetComponent<BoxCollider>().enabled)
            {
                RemoveRangeBuffer(attackTarget);
                RemoveRangeTarget(attackTarget);
                attackTarget = null;
            }
            // Target is in attacking range
            else if (rangeList.Contains(attackTarget))
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
                    animator.SetBool("Run", false);
                    destination = false;
                }
            }
            //// Moving ////
            else if(!animator.GetBool("Run"))
            {
                // A destination is set and agent is moving. Set animation to running.
                animator.SetBool("Run", true);
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
                if(animator.GetBool("Run"))
                {
                    // Not attacking or moving. Set animation to idle.
                    animator.SetBool("Run", false);
                }
            }
        }
        InstantlyTurn(agent.steeringTarget);

        // Calculate which tile the character is on
        currentPosition = transform.position;

        if (currentPosition != lastPosition)
        {
            currentTile = board.WorldSpaceToTile(currentPosition);
            if(currentTile != lastTile)
            {
                if(lastTile)
                {
                    lastTile.GetComponent<Hex>().RemovePlayer(this.gameObject);
                    //////////////////////////// hex based basic attack spaghetti //////////////////////////////
                    if (basicAttackEnabled)
                    {
                        List <GameObject> selectableTiles = BoardController.Instance.Ring(1, lastTile);
                        BoardController.Instance.HighlightRangeOff(selectableTiles);
                    }
                    ///////////////////////////////////////////////////////////////////////////////////////////
                }
                currentTile.GetComponent<Hex>().AddPlayer(this.gameObject);
                //////////////////////////// hex based basic attack spaghetti //////////////////////////////
                if (basicAttackEnabled)
                {
                    List <GameObject> selectableTiles = BoardController.Instance.Ring(1, currentTile);
                    BoardController.Instance.HighlightRangeOn(selectableTiles, "red");
                }
                ///////////////////////////////////////////////////////////////////////////////////////////
                lastTile = currentTile;
            }
            lastPosition = currentPosition;
        }
    }

    //// Control Commands ////

    public void MoveToLocation(Vector3 targetPoint)
    {
        animator = GetComponent<Animator>();
        if(attacking)
        {
            CancelAttack();
        }
        if(!animator.GetBool("Run"))
        {
            animator.SetBool("Run", true);
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
        //Debug.Log("Attack Cancelled");
    }

    IEnumerator Attack(GameObject target)
    {
        animator = GetComponent<Animator>();
        attacking = true;
        // Stop navigating
        agent.ResetPath();
        animator.SetBool("Run", false);
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
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.87f);
        if(target)
        {
            if(bufferList.Contains(target))
            {
                // Eventually pull damage from Stats script
                target.GetComponent<EnemyStats>().TakeDamage(20, this.gameObject);
            }
        }
        //Debug.Log("Attacked " + target.name);
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

    //// Basic Attack ////
    public void BasicAttackInitiate()
    {
        if(!basicAttackEnabled)
        {
            basicAttackEnabled = true;
            Debug.Log("Attack");
            List <GameObject> selectableTiles = BoardController.Instance.Ring(1, currentTile);
            BoardController.Instance.HighlightRangeOn(selectableTiles, "red");
        }
    }

    public bool BasicAttackExecute()
    {
        List <GameObject> selectableTiles = BoardController.Instance.Ring(1, currentTile);
        BoardController.Instance.HighlightRangeOff(selectableTiles);
        basicAttackEnabled = false;
        return true;
    }

}