using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public int health = 100;
    public static int[] edge = new int[] {0,0,0,0,0,0};
 
    public void SetEdges()
    {
        this.transform.parent.gameObject.GetComponent<Hex>().SetAllEdge(edge);
    }
    public int[] GetEdges()
    {
        return edge;
    }
}
