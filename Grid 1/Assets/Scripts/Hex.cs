using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public static BoardController board;
    public int Structure { get; set; } = 0;
    public int Edge0 { get; set; } = 0;
    public int Edge1 { get; set; } = 0;
    public int Edge2 { get; set; } = 0;
    public int Edge3 { get; set; } = 0;
    public int Edge4 { get; set; } = 0;
    public int Edge5 { get; set; } = 0;

    private void Start() 
    {
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
        //Debug.Log(board);

    }

    public int[] GetAllEdge()
    {
        int[] status = new int[] {this.Edge0, this.Edge1, this.Edge2, this.Edge3, this.Edge4, this.Edge5, this.Structure};
        return status;
    }

    public void SetAllEdge(int[] status)
    {
        this.Edge0 = status[0];
        this.Edge1 = status[1];
        this.Edge2 = status[2];
        this.Edge3 = status[3];
        this.Edge4 = status[4];
        this.Edge5 = status[5];
        //Debug.Log(this.parent.)
    }

    public int GetEdge(int edge)
    {
        switch (edge)
        {
            case 0:
                return this.Edge0;
                break;
            case 1:
                return this.Edge1;
                break;
            case 2:
                return this.Edge2;
                break;
            case 3:
                return this.Edge3;
                break;
            case 4:
                return this.Edge4;
                break;
            case 5:
                return this.Edge5;
                break;
            default:
                return 0;
                break;
        }
    }

    public void SetEdge(int edge, int status)
    {
        switch (edge)
        {
            case 0:
                this.Edge0 = status;
                break;
            case 1:
                this.Edge1 = status;
                break;
            case 2:
                this.Edge2 = status;
                break;
            case 3:
                this.Edge3 = status;
                break;
            case 4:
                this.Edge4 = status;
                break;
            case 5:
                this.Edge5 = status;
                break;
        }
    }

    public void TestRef()
    {
        Debug.Log("Test");
    }

    public int[] GetAvailability()
    {
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
        int[] availability = board.GetAdjacent(this.gameObject);
        return availability;
    }

}