using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : Structure
{
    // Start is called before the first frame update
    void Start()
    {
        edge[0] = 1;
        edge[1] = 0;
        edge[2] = 1;
        edge[3] = 0;
        edge[4] = 1;
        edge[5] = 0;
    }
}
