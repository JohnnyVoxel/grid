using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public int health = 100;
    public static int[] edge = new int[] {1,1,1,1,1,1};
    /*private void Start() 
    {
        SetEdge();
    }*/
    public void SetEdge()
    {
        this.transform.parent.gameObject.GetComponent<Hex>().SetAllEdge(edge);
        //this.transform.parent.GetComponent<Hex>().TestRef();
    }
}
