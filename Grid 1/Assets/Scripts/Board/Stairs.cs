﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : Structure
{
    // Start is called before the first frame update
    void Start()
    {
        edge[0] = 0;
        edge[1] = 3;
        edge[2] = 0;
        edge[3] = 0;
        edge[4] = 2;
        edge[5] = 0;
    }
}