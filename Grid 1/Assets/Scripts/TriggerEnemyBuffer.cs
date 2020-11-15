using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemyBuffer : MonoBehaviour
{
    public GameObject currentTarget;

    void OnTriggerEnter(Collider other) {
        var parent = transform.parent.gameObject.GetComponent<EnemyAgent>();
        if ((other.gameObject.tag == "Player")||(other.gameObject.tag == "Structure"))
        {
            currentTarget = other.gameObject;
            parent.AddRangeBuffer(other.gameObject.transform.parent.gameObject);
        }
    }
    void OnTriggerExit(Collider other){
        var parent = transform.parent.gameObject.GetComponent<EnemyAgent>();
        if ((other.gameObject.tag == "Player")||(other.gameObject.tag == "Structure"))
        {
            parent.RemoveRangeBuffer(other.gameObject.transform.parent.gameObject);
        }
    }
}
