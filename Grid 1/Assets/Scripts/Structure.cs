using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public int health = 100;
    public int[] edge = new int[] {1,1,1,1,1,1};

    public int[] SetEdge()
    {
        return edge;
    }
}
