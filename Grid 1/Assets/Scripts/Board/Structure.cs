using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    public int health = 100;
    public static int[] edge = new int[] {0,0,0,0,0,0};
    public int[,] whitelist;
 
    public void SetEdges()
    {
        this.transform.parent.gameObject.GetComponent<Hex>().SetAllEdge(edge);
    }
    public int[] GetEdges()
    {
        return edge;
    }
    public void Rotate()
    {
        this.transform.Rotate(0.0f, 60.0f, 0.0f, Space.Self);
        int park = edge[0];
        edge[0] = edge[1];
        edge[1] = edge[2];
        edge[2] = edge[3];
        edge[3] = edge[4];
        edge[4] = edge[5];
        edge[5] = park;

        int[,] parkinglot = new int[,] {
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0}
        };
        for(int col = 0; col < 9; col++)
        {
            int x = col - 4;
            for(int row = 0; row < 9; row++)
            {
                int z = row - 4;
                int y = -x - z;
                if(whitelist[col,row] != 0)
                {
                    parkinglot[-y+4,-x+4] = whitelist[col,row];
                }
            }
        }
        whitelist = parkinglot;


    }
}
