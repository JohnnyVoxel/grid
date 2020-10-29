using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAgro : MonoBehaviour
{
    public GameObject currentTarget;
    // Start is called before the first frame update
    void OnTriggerStay(Collider other) {
        var parent = transform.parent.gameObject.GetComponent<EnemyAgent>();
        if (other.gameObject.tag == "Player")
        {
            currentTarget = other.gameObject;
            parent.AgroStart(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other){
        var parent = transform.parent.gameObject.GetComponent<EnemyAgent>();
        if (other.gameObject.tag == "Player")
        {
            parent.AgroStop(other.gameObject);
        }
    }
}
