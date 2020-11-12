using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackRange : MonoBehaviour
{
    public GameObject currentTarget;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other) {
        var parent = transform.parent.gameObject.GetComponent<DirectedAgent>();
        if (other.gameObject.tag == "Enemy")
        {
            currentTarget = other.gameObject;
            parent.AddRangeTarget(other.gameObject.transform.parent.gameObject);
        }
    }
    void OnTriggerExit(Collider other){
        var parent = transform.parent.gameObject.GetComponent<DirectedAgent>();
        if (other.gameObject.tag == "Enemy")
        {
            parent.RemoveRangeTarget(other.gameObject.transform.parent.gameObject);
        }
    }
}
