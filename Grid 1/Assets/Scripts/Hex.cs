using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public int Structure { get; set; } = 0; //Commit test
    public int Edge0 { get; set; } = 0;
    public int Edge1 { get; set; } = 0;
    public int Edge2 { get; set; } = 0;
    public int Edge3 { get; set; } = 0;
    public int Edge4 { get; set; } = 0;
    public int Edge5 { get; set; } = 0;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    public int[] GetStatus()
    {
        int[] status = new int[] {this.Edge0, this.Edge1, this.Edge2, this.Edge3, this.Edge4, this.Edge5, this.Structure};
        return status;
    }
    public void SetEdge(int[] status)
    {
        this.Edge0 = status[0];
        this.Edge1 = status[1];
        this.Edge2 = status[2];
        this.Edge3 = status[3];
        this.Edge4 = status[4];
        this.Edge5 = status[5];
    }
}