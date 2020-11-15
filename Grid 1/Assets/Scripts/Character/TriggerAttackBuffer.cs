using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAttackBuffer : MonoBehaviour
{
    public GameObject currentTarget;

    void OnTriggerEnter(Collider other) {
        var parent = transform.parent.gameObject.GetComponent<CharacterAgent>();
        if (other.gameObject.tag == "Enemy")
        {
            currentTarget = other.gameObject;
            parent.AddRangeBuffer(other.gameObject.transform.parent.gameObject);
        }
    }
    void OnTriggerExit(Collider other){
        var parent = transform.parent.gameObject.GetComponent<CharacterAgent>();
        if (other.gameObject.tag == "Enemy")
        {
            parent.RemoveRangeBuffer(other.gameObject.transform.parent.gameObject);
        }
    }
}
