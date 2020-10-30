using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 basePos;
    private List<GameObject> aggroRangeList = new List<GameObject>();
    private GameObject aggroAttackTarget;
    
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

    void Update()
    {
        if(aggroRangeList.Count > 0)
        {
            MoveToLocation(aggroRangeList[0].transform.position);
        }
        else if(aggroAttackTarget)
        {
            MoveToLocation(aggroAttackTarget.transform.position);
        }
        else
        {
            MoveToLocation();
        }
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
    }

    IEnumerator AggroForget()
    {
        yield return new WaitForSeconds(4);
        aggroAttackTarget = null;
    }
}
