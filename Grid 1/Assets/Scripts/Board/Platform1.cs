﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform1 : Structure
{
    // Start is called before the first frame update
    void Start()
    {
        edge[0] = 1;
        edge[1] = 1;
        edge[2] = 1;
        edge[3] = 1;
        edge[4] = 1;
        edge[5] = 1;

        whitelist = new int[,] {
            {0,0,0,0,1,1,1,1,1},
            {0,0,0,1,1,1,1,1,1},
            {0,0,1,1,1,1,1,1,1},
            {0,1,1,1,1,1,1,1,1},
            {1,1,1,1,3,1,1,1,1},
            {1,1,1,1,1,1,1,1,0},
            {1,1,1,1,1,1,1,0,0},
            {1,1,1,1,1,1,0,0,0},
            {1,1,1,1,1,0,0,0,0}
        };
    }
}
