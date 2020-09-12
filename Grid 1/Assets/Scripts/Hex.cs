using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public int Structure { get; set; } = 0;
    public int Edge1 { get; set; } = 0;
    public int Edge2 { get; set; } = 0;
    public int Edge3 { get; set; } = 0;
    public int Edge4 { get; set; } = 0;
    public int Edge5 { get; set; } = 0;
    public int Edge6 { get; set; } = 0;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public class Platform : Hex
    {
        public Platform()
        {
            this.Edge1 = 1;
            this.Edge3 = 1;
            this.Edge5 = 1;
        }
    }
}
