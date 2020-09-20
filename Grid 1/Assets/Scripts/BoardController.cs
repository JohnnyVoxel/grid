using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public float size = .6f;
    
    [SerializeField] private GameObject tilePrefab;

    //private int[,]  map = {{0,0,0,2,2,2,2},{0,0,2,2,1,1,2},{0,2,2,1,1,1,1},{2,1,1,3,1,1,1},{2,1,1,1,1,1,0},{2,2,1,1,1,0,0},{2,2,2,2,0,0,0}};
    //private int[,]  map = {{0,0,0,0,1,1,1},{0,0,2,2,1,0,0},{1,1,1,0,0,0,0}};
    private int[,] map = {
        {0,0,0,0,0,0,0,0,2,4,2,0,0,0},
        {0,0,0,0,0,0,0,2,2,2,2,0,0,0},
        {0,0,0,0,2,2,0,2,2,2,0,2,2,0},
        {0,0,0,4,2,2,1,1,1,1,2,2,4,0},
        {0,0,2,2,2,1,1,1,1,1,2,2,2,0},
        {0,0,2,2,1,1,1,1,1,1,2,2,0,0},
        {0,0,0,1,1,1,3,1,1,1,0,0,0,0},
        {0,2,2,1,1,1,1,1,1,2,2,0,0,0},
        {2,2,2,1,1,1,1,1,2,2,2,0,0,0},
        {4,2,2,1,1,1,1,2,2,4,0,0,0,0},
        {2,2,0,2,2,2,0,2,2,0,0,0,0,0},
        {0,0,2,2,2,2,0,0,0,0,0,0,0,0},
        {0,0,2,4,2,0,0,0,0,0,0,0,0,0}
    };
    public static GameObject[,] tileMap = new GameObject[1,1];

    // Start is called before the first frame update
    public void Start()
    {
        tileMap = new GameObject[map.GetLength(0),map.GetLength(1)];
        //Debug.Log(tileMap.GetLength(0));
        for (int q = 0; q < map.GetLength(0); q++)
        {
            for (int r = 0; r < map.GetLength(1); r++)
            {
                if(map[q,r]!=0)
                {
                    var x = size * (Mathf.Sqrt(3) * q + Mathf.Sqrt(3)/2 * r);
                    var z = size * (3f/2 * r);
                    var point = new Vector3(x, 0f, z);
                    GameObject tileInstance = Instantiate(tilePrefab, point, Quaternion.identity);
                    tileInstance.layer = 8;
                    tileInstance.name = q + "-" + r;
                    //Debug.Log(newTile.layer);
                    tileMap[q,r] = tileInstance;
                }
                else
                {
                    tileMap[q,r] = null;
                }
            }
        }
    }
    public int[] GetAdjacent(GameObject tile)
    {
        int[] tilePosition = GetCoordinates(tile);
        int[] adjacent = new int[] {9,9,9,9,9,9};
        
        // Position 0 (top right)
        int[] tileNeighbor = tilePosition;
        tileNeighbor[0]+=1;
        tileNeighbor[1]-=1;
        GameObject tile0 = GetTile(tileNeighbor);
        if(tile0)
        {
            adjacent[0] = tile0.GetComponent<Hex>().GetEdge(0);
        }
        
        tileNeighbor = tilePosition;
        tileNeighbor[0]+=1;
        GameObject tile1 = GetTile(tileNeighbor);
        if(tile1)
        {
            adjacent[1] = tile1.GetComponent<Hex>().GetEdge(1);
        }

        tileNeighbor = tilePosition;
        tileNeighbor[1]+=1;
        GameObject tile2 = GetTile(tileNeighbor);
        if(tile2)
        {
            adjacent[2] = tile2.GetComponent<Hex>().GetEdge(2);
        }

        tileNeighbor = tilePosition;
        tileNeighbor[0]-=1;
        tileNeighbor[1]+=1;
        GameObject tile3 = GetTile(tileNeighbor);
        if(tile3)
        {
            adjacent[3] = tile3.GetComponent<Hex>().GetEdge(3);
        }

        tileNeighbor = tilePosition;
        tileNeighbor[0]-=1;
        GameObject tile4 = GetTile(tileNeighbor);
        if(tile4)
        {
            adjacent[4] = tile4.GetComponent<Hex>().GetEdge(4);
        }

        tileNeighbor = tilePosition;
        tileNeighbor[1]-=1;
        GameObject tile5 = GetTile(tileNeighbor);
        if(tile5)
        {
            adjacent[5] = tile5.GetComponent<Hex>().GetEdge(5);
        }

        return adjacent;
    }

    public int[] GetCoordinates(GameObject tile)
    {
        int[] position = new int[2];
        for (int q = 0; q < tileMap.GetLength(0); q++)
        {
            for (int r = 0; r < tileMap.GetLength(1); r++)
            {
                if(tileMap[q,r]==tile)
                {
                    position[0]=q;
                    position[1]=r;
                    return position;
                }
            }
        }
        return position;
    }

    public GameObject GetTile(int[] position)
    {
        if((position[0] >= 0) && (position[0] < tileMap.GetLength(0)) && (position[1] >=0) && (position[1] < tileMap.GetLength(1)))
        {
            GameObject tile = tileMap[position[0],position[1]];
            return tile;
        }
        else
        {
            return null;
        }
    }
}
