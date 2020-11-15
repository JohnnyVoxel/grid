using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackRange : MonoBehaviour
{
    public GameObject currentTarget;
    
    void OnTriggerEnter(Collider other) {
        var parent = transform.parent.gameObject.GetComponent<CharacterAgent>();
        if (other.gameObject.tag == "Enemy")
        {
            currentTarget = other.gameObject;
            parent.AddRangeTarget(other.gameObject.transform.parent.gameObject);
        }
    }
    void OnTriggerExit(Collider other){
        var parent = transform.parent.gameObject.GetComponent<CharacterAgent>();
        if (other.gameObject.tag == "Enemy")
        {
            parent.RemoveRangeTarget(other.gameObject.transform.parent.gameObject);
        }
    }
}
