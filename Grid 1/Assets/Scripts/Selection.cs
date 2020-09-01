using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    //public Color selectColor = Color.red;
    void OnMouseEnter()
    {
        transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
    void OnMouseExit()
    {
        transform.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
}
