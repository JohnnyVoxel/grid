using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoardController : MonoBehaviour
{
    private float size = .577f;
    
    public GameObject tilePrefab;
    public GameObject tilePrefab2;
    public GameObject tilePrefab3;
    public GameObject structurePrefab;

    //private int[,]  map = {{0,0,0,2,2,2,2},{0,0,2,2,1,1,2},{0,2,2,1,1,1,1},{2,1,1,3,1,1,1},{2,1,1,1,1,1,0},{2,2,1,1,1,0,0},{2,2,2,2,0,0,0}};
    //private int[,]  map = {{0,0,0,0,1,1,1},{0,0,2,2,1,0,0},{1,1,1,0,0,0,0}};
    /*private int[,] map = {
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
    };*/
    private int[,] map = {
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,4,2,2,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,0,0,0,0},
        {0,0,0,0,0,0,0,2,2,2,2,0,2,2,2,2,2,2,0,2,2,2,2},
        {0,0,0,0,0,0,2,2,2,2,2,0,2,2,2,2,2,0,2,2,2,2,2},
        {0,0,0,0,0,2,4,2,2,2,2,1,1,1,1,1,1,2,2,2,2,4,2},
        {0,0,0,0,2,2,2,2,2,2,1,1,1,1,1,1,1,2,2,2,2,2,2},
        {0,0,0,0,2,2,2,2,2,1,1,1,1,1,1,1,1,2,2,2,2,2,0},
        {0,0,0,0,2,2,2,2,1,1,1,1,1,1,1,1,1,2,2,2,2,0,0},
        {0,0,0,0,2,2,2,1,1,1,1,1,1,1,1,1,1,2,2,2,0,0,0},
        {0,0,0,0,0,0,1,1,1,1,1,3,1,1,1,1,1,0,0,0,0,0,0},
        {0,0,0,2,2,2,1,1,1,1,1,1,1,1,1,1,2,2,2,0,0,0,0},
        {0,0,2,2,2,2,1,1,1,1,1,1,1,1,1,2,2,2,2,0,0,0,0},
        {0,2,2,2,2,2,1,1,1,1,1,1,1,1,2,2,2,2,2,0,0,0,0},
        {2,2,2,2,2,2,1,1,1,1,1,1,1,2,2,2,2,2,2,0,0,0,0},
        {2,4,2,2,2,2,1,1,1,1,1,1,2,2,2,2,4,2,0,0,0,0,0},
        {2,2,2,2,2,0,2,2,2,2,2,0,2,2,2,2,2,0,0,0,0,0,0},
        {2,2,2,2,0,2,2,2,2,2,2,0,2,2,2,2,0,0,0,0,0,0,0},
        {0,0,0,0,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,2,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,2,2,4,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    };
    public static GameObject[,] tileMap = new GameObject[1,1];
    static BoardController _instance;
    public static BoardController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<BoardController>();
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    public void Awake()
    {
        tileMap = new GameObject[map.GetLength(0),map.GetLength(1)];
        for (int q = 0; q < map.GetLength(0); q++)
        {
            for (int r = 0; r < map.GetLength(1); r++)
            {
                if(map[q,r]!=0)
                {
                    var x = size * (Mathf.Sqrt(3) * q + Mathf.Sqrt(3)/2 * r);
                    var z = size * (3f/2 * r);
                    var point = new Vector3(x, 0f, z);
                    if(map[q,r]==1)
                    {
                        GameObject tileInstance = Instantiate(tilePrefab, point, Quaternion.identity);
                        tileInstance.layer = 8;
                        tileInstance.name = q + "-" + r;
                        tileInstance.transform.parent = this.transform;
                        tileMap[q,r] = tileInstance;
                    }
                    else if(map[q,r]==3)
                    {
                        GameObject tileInstance = Instantiate(tilePrefab, point, Quaternion.identity);
                        tileInstance.layer = 8;
                        tileInstance.name = q + "-" + r;
                        tileInstance.transform.parent = this.transform;
                        tileMap[q,r] = tileInstance;
                        GameObject structure = Instantiate(structurePrefab, point, Quaternion.identity);
                        structure.name = "structure";
                        structure.transform.parent = tileInstance.transform;
                        //// Set appropriate tag for structure
                        structure.tag = "Structure";
                        int numberChildren = structure.transform.childCount;
                        if (numberChildren > 0)
                        {
                            for (int n=0;n<numberChildren; n++)
                            {
                                structure.transform.GetChild(n).tag = "Structure";
                            }
                        }
                        ////
                        int[] platform = {1,1,1,1,1,1};
                        tileInstance.GetComponent<Hex>().SetAllEdge(platform);
                    }
                    else if(map[q,r]==4)
                    {
                        GameObject tileInstance = Instantiate(tilePrefab3, point, Quaternion.identity);
                        tileInstance.layer = 8;
                        tileInstance.name = q + "-" + r;
                        tileInstance.transform.parent = this.transform;
                        tileMap[q,r] = tileInstance;
                    }
                    else
                    {
                        GameObject tileInstance = Instantiate(tilePrefab2, point, Quaternion.identity);
                        tileInstance.layer = 8;
                        tileInstance.name = q + "-" + r;
                        tileInstance.transform.parent = this.transform;
                        tileMap[q,r] = tileInstance;
                    }
                    
                }
                else
                {
                    tileMap[q,r] = null;
                }
            }
        }
        RebuildNavMesh();
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
            adjacent[0] = tile0.GetComponent<Hex>().GetEdge(3);
        }
        
        tileNeighbor[1]+=1;
        GameObject tile1 = GetTile(tileNeighbor);
        if(tile1)
        {
            adjacent[1] = tile1.GetComponent<Hex>().GetEdge(4);
        }

        tileNeighbor[0]-=1;
        tileNeighbor[1]+=1;
        GameObject tile2 = GetTile(tileNeighbor);
        if(tile2)
        {
            adjacent[2] = tile2.GetComponent<Hex>().GetEdge(5);
        }

        tileNeighbor[0]-=1;
        GameObject tile3 = GetTile(tileNeighbor);
        if(tile3)
        {
            adjacent[3] = tile3.GetComponent<Hex>().GetEdge(0);
        }

        tileNeighbor[1]-=1;
        GameObject tile4 = GetTile(tileNeighbor);
        if(tile4)
        {
            adjacent[4] = tile4.GetComponent<Hex>().GetEdge(1);
        }

        tileNeighbor[0]+=1;
        tileNeighbor[1]-=1;
        GameObject tile5 = GetTile(tileNeighbor);
        if(tile5)
        {
            adjacent[5] = tile5.GetComponent<Hex>().GetEdge(2);
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

    public bool GetAvailability(int[] edges, GameObject tile)
    {
        int[] adjacent = GetAdjacent(tile);
        int requirement = 0;
        int satisfied = 0;
        for (int i=0; i<6; i++)
        {
            if(edges[i]==1)
            {
                if(adjacent[i]==3)
                {
                    return false;
                }
            }
            if(edges[i]==2)
            {
                requirement++;
                if((adjacent[i]==3)||(adjacent[i]==9))
                {
                    return false;
                }
                if((adjacent[i]==1)||(adjacent[i]==2))
                {
                    satisfied++;
                }
            }
            if(edges[i]==3)
            {
                if((adjacent[i]==2)||(adjacent[i]==1))
                {
                    return false;
                }
            }
        }
        if ((requirement > 0)&&(satisfied==0))
        {
            return false;
        }
        return true;
    }

    public void RebuildNavMesh()
    {
        Component[] navMeshes;
        navMeshes = GetComponents(typeof(NavMeshSurface));
        foreach (NavMeshSurface surface in navMeshes)
        {
            surface.BuildNavMesh();
        }
    }

    // Find and return the worldspace position of the starting base.
    public Vector3 GetBasePosition()
    {
        Vector3 position = new Vector3 (0f, 0f, 0f); // Possibly make nullable to catch errors
        for (int q = 0; q < map.GetLength(0); q++)
        {
            for (int r = 0; r < map.GetLength(1); r++)
            {
                if(map[q,r]==3)
                {
                    float x = size * (Mathf.Sqrt(3) * q + Mathf.Sqrt(3)/2 * r);
                    float z = size * (3f/2 * r);
                    position = new Vector3(x, 0f, z);
                }
            }
        }
        return position;
    }

    public List <Vector3> GetEnemySpawnPoints()
    {
        List <Vector3> spawns = new List <Vector3>(); 
        for (int q = 0; q < map.GetLength(0); q++)
        {
            for (int r = 0; r < map.GetLength(1); r++)
            {
                if(map[q,r]==4)
                {
                    float x = size * (Mathf.Sqrt(3) * q + Mathf.Sqrt(3)/2 * r);
                    float z = size * (3f/2 * r);
                    spawns.Add(new Vector3(x, 0f, z));
                }
            }
        }
        return spawns;
    }
}
