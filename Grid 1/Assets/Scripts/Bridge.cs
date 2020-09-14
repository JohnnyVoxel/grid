using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : Structure
{
    // Start is called before the first frame update
    void Start()
    {
        this.edge[0] = 0;
        this.edge[1] = 1;
        this.edge[2] = 0;
        this.edge[3] = 0;
        this.edge[4] = 1;
        this.edge[5] = 0;
    }
    public void rotate()
    {
        
    }
}
