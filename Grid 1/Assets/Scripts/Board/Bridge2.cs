using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge2 : Structure
{
    // Start is called before the first frame update
    void Start()
    {
        edge[0] = 0;
        edge[1] = 2;
        edge[2] = 0;
        edge[3] = 0;
        edge[4] = 2;
        edge[5] = 0;

        whitelist = new int[,] {
            {0,0,0,0,2,2,2,2,2},
            {0,0,0,2,2,2,2,2,2},
            {0,0,2,2,2,2,2,2,2},
            {0,2,2,2,2,2,2,2,2},
            {2,2,2,2,3,2,2,2,2},
            {2,2,2,2,2,2,2,2,0},
            {2,2,2,2,2,2,2,0,0},
            {2,2,2,2,2,2,0,0,0},
            {2,2,2,2,2,0,0,0,0}
        };
    }
}
