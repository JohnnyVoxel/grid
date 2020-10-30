using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAggro : MonoBehaviour
{
    public GameObject currentTarget;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other) {
        var parent = transform.parent.gameObject.GetComponent<EnemyAgent>();
        if (other.gameObject.tag == "Player")
        {
            currentTarget = other.gameObject;
            parent.AddRangeAggro(other.gameObject);
            //parent.AddAttackAggro(other.gameObject);  // Testing attack aggro before I implemented and attack
        }
    }
    void OnTriggerExit(Collider other){
        var parent = transform.parent.gameObject.GetComponent<EnemyAgent>();
        if (other.gameObject.tag == "Player")
        {
            parent.RemoveRangeAggro(other.gameObject);
        }
    }
}
