using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public static BoardController board;
    public int Structure { get; set; } = 0;
    public int struc = 0;
    public int Edge0 { get; set; } = 0;
    public int e0 = 0;
    public int Edge1 { get; set; } = 0;
    public int e1 = 0;
    public int Edge2 { get; set; } = 0;
    public int e2 = 0;
    public int Edge3 { get; set; } = 0;
    public int e3 = 0;
    public int Edge4 { get; set; } = 0;
    public int e4 = 0;
    public int Edge5 { get; set; } = 0;
    public int e5 = 0;

    public List<GameObject> enemyList = new List<GameObject>();
    public List<GameObject> playerList = new List<GameObject>();

    private void Start() 
    {
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
    }

    private void Update() 
    {
        this.e0 = this.Edge0;
        this.e1 = this.Edge1;
        this.e2 = this.Edge2;
        this.e3 = this.Edge3;
        this.e4 = this.Edge4;
        this.e5 = this.Edge5;
        this.struc = this.Structure;
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
        this.Structure = 1;
    }

    public void ResetHex()
    {
        this.Edge0 = 0;
        this.Edge1 = 0;
        this.Edge2 = 0;
        this.Edge3 = 0;
        this.Edge4 = 0;
        this.Edge5 = 0;
        this.Structure = 0;
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

    public int[] GetAvailability()
    {
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
        int[] availability = board.GetAdjacent(this.gameObject);
        return availability;
    }
    //// Agent lists ////
    public void AddEnemy(GameObject target)
    {
        if(!enemyList.Contains(target))
        {
            enemyList.Add(target);
        }
    }
    public void RemoveEnemy(GameObject target)
    {
        if(enemyList.Contains(target))
        {
            enemyList.Remove(target);
        }
    }
    public void AddPlayer(GameObject target)
    {
        if(!playerList.Contains(target))
        {
            playerList.Add(target);
        }
    }
    public void RemovePlayer(GameObject target)
    {
        if(playerList.Contains(target))
        {
            playerList.Remove(target);
        }
    }
    public List<GameObject> GetEnemy()
    {
        return enemyList;
    }
    public List<GameObject> GetPlayer()
    {
        return playerList;
    }
    //// Highlights ////
    public void HighlightOn(string command)
    {
        Color color;
        if (command == "red")
        {
            color = new Color(1, 0, 0, 0.2f);
        }
        else if (command == "cyan")
        {
            color = new Color(0, 1, 1, 0.2f);
        }
        else
        {
            color = new Color(0, 1, 0, 0.2f);
        }
        Renderer highlight = transform.Find("Highlight").GetComponent<Renderer>();
        highlight.enabled = true;
        highlight.material.color = color;
    }
    public void HighlightOff()
    {
        Renderer highlight = transform.Find("Highlight").GetComponent<Renderer>();
        highlight.enabled = false;
    }

}