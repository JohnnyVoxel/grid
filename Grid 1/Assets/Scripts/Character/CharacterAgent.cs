using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharacterAgent : MonoBehaviour {
    
    private NavMeshAgent agent;
    private Animator animator;
    private BoardController board;
    private CharacterController controller;
    public float rotSpeed = 10.0f;

    private Vector3 destinationPosition;
    //private Vector3 currentPosition;
    //private Vector3 lastPosition;
    private GameObject currentTile;
    private GameObject lastTile;

    private bool actionEnabled = false;
    private bool basicAttackEnabled = false;
    private bool destination = false;
    private List<GameObject> selectableTileList = new List <GameObject>();
    private char actionCommand;

    void Start()
    {
        animator = GetComponent<Animator>();
        board = GameObject.Find("Board").GetComponent<BoardController>();
        controller = GetComponent<CharacterController>();
    }
    // Use this for initialization
    void Awake() 
    {
        agent = GetComponent<NavMeshAgent> ();
        agent.updateRotation = false;    
    }

    void Update()
    {
        if (actionEnabled)
        {
            /*
            if(CharacterAction.Action(actionCommand));
            {
                actionEnabled = false;
                controller.currentCommand = 'I';
            }
            */
        }
        else if (destination)
        {
            // Move to destination
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
        else if (basicAttackEnabled)
        {
            Debug.Log("Attack logic");
        }
        else
        {
            //idle
        } 
    }

    void FixedUpdate()
    {
        InstantlyTurn(agent.steeringTarget);
    }

    private void InstantlyTurn(Vector3 destination) 
    {
        //When on target -> dont rotate!
        if ((destination - transform.position).magnitude < 0.1f) return; 

        Vector3 targetDirection = destination - transform.position;
        float singleStep = rotSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    void ActionEnable(char newActionCommand)
    {
        // Setup for the action routine
        if (newActionCommand != actionCommand)
        {
            Debug.Log("Cancel " + actionCommand);
            Debug.Log("Initiate " + newActionCommand);
            // if CharacterAction.CancelCommand returns true
                actionCommand = newActionCommand;
        }
        animator = GetComponent<Animator>();
        // If destination flag is true, cancel movement
        if(destination)
        {
            agent.ResetPath();
            destination = false;
            if(animator.GetBool("Run"))
            {
                animator.SetBool("Run", false);
            }
        }
        // If basic attack flag is true, cancel attacking
        if(basicAttackEnabled)
        {
            // basic attack cancel method
            basicAttackEnabled = false;
        }
        actionEnabled = true;
    }

    public void MoveToLocation(Vector3 targetPoint)
    {
        animator = GetComponent<Animator>();
        if(basicAttackEnabled)
        {
            //CharacterAction.BasicAttackCancel();
            basicAttackEnabled = false;
        }
        if(!animator.GetBool("Run"))
        {
            animator.SetBool("Run", true);
        }
        agent.destination = targetPoint;
        agent.isStopped = false;
        destination = true;
    }

    public void BasicAttackInitiate()
    {
        BasicAttackEnabled = true;
        if(destination)
        {
            agent.ResetPath();
            destination = false;
            if(animator.GetBool("Run"))
            {
                animator.SetBool("Run", false);
            }
        }
    }

    public void CancelCommand()
    {
        if(actionEnabled)
        {
            /*
            if(CharacterAction.ActionCancel())
            {
                actionEnabled = false;
            }*/
        }
        if(basicAttackEnabled)
        {
            // CharacterAction.BasicAttackCancel();
        }
    }

    public void Execute()
    {
        if(actionEnabled)
        {
            // CharacterAction.ActionExecute()
        }
        if(basicAttackEnabled)
        {
            // CharacterAction.BasicAttackExecute();
        }
    }