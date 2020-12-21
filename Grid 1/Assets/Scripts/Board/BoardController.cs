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

    // Build the board
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

    // Return the availability for structures of all adjacent tile edges for the passed tile.
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

    // Return the board position (Q, R) of a reference to a tile
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

    // Return the reference to the tile at the board position (Q, R)
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

    // Find and return the worldspace position of the enemy spawn points as a list.
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

    // Return the reference to the tile that encompases the current worldspace
    // Players and enemies check to see which tile to report to
    public GameObject WorldSpaceToTile (Vector3 position)
    {
        float x = (Mathf.Sqrt(3)/3*position.x - 1f/3*position.z)/size;
        float z = (2f/3*position.z)/size;
        float y = -x-z;
        float rx = Mathf.Round(x);
        float ry = Mathf.Round(y);
        float rz = Mathf.Round(z);
        float xDiff = Mathf.Abs(rx-x);
        float yDiff = Mathf.Abs(ry-y);
        float zDiff = Mathf.Abs(rz-z);
        if (xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry-rz;
        }
        else if (yDiff > zDiff)
        {
            ry = -rx-rz;
        }
        else
        {
            rz = -rx-ry;
        }
        int[] axial = new int[] {(int)rx, (int)rz};
        GameObject tile = GetTile(axial);
        return tile;
    }

    public void HighlightRangeOn(List <GameObject> tileList, string command)
    {
        foreach (GameObject tile in tileList)
        {
            if(tile)
            {
                tile.GetComponent<Hex>().HighlightOn(command);
            }
        }
    }

    public void HighlightInstantRangeOn(List <GameObject> tileList, string command)
    {
        foreach (GameObject tile in tileList)
        {
            if(tile)
            {
                tile.GetComponent<Hex>().HighlightInstantOn(command);
            }
        }
    }

    public void HighlightRangeOff(List <GameObject> tileList)
    {
        foreach (GameObject tile in tileList)
        {
            if(tile)
            {
                tile.GetComponent<Hex>().HighlightOff();
            }
        }
    }

    public void HighlightInstantRangeOff(List <GameObject> tileList)
    {
        foreach (GameObject tile in tileList)
        {
            if(tile)
            {
                tile.GetComponent<Hex>().HighlightInstantOff();
            }
        }
    } 

    public void HighlightAllOff()
    {
        foreach (GameObject tile in tileMap)
        {
            if(tile)
            {
                tile.GetComponent<Hex>().HighlightOff();
            }
        }
    }

    public void HighlightInstantAllOff()
    {
        foreach (GameObject tile in tileMap)
        {
            if(tile)
            {
                tile.GetComponent<Hex>().HighlightInstantOff();
            }
        }
    }

    public List <GameObject> Ring(int distance, GameObject startTile)
    {
        int[] startCoordinates = GetCoordinates(startTile);
        int xStart = startCoordinates[0];
        int zStart = startCoordinates[1];
        int yStart = -xStart - zStart;
        List <GameObject> tileList = new List <GameObject>();
        for(int x = 0 - distance; x <= distance; x++)
        {
            for(int y = 0 - distance; y <= distance; y++)
            {
                for(int z = 0 - distance; z <= distance; z++)
                {
                    if(x + y + z == 0)
                    {
                        if((Mathf.Abs(x)==distance)||(Mathf.Abs(y)==distance)||(Mathf.Abs(z)==distance))
                        {
                            int [] newTileCoordinates = new int[] {x+xStart, z+zStart};
                            if(GetTile(newTileCoordinates))
                            {
                                tileList.Add(GetTile(newTileCoordinates));
                            }
                        }
                    }
                }
            }
        }
        return tileList;
    }

    public List <GameObject> Circle(int distance, GameObject startTile)
    {
        int[] startCoordinates = GetCoordinates(startTile);
        int xStart = startCoordinates[0];
        int zStart = startCoordinates[1];
        int yStart = -xStart - zStart;
        List <GameObject> tileList = new List <GameObject>();
        for(int x = 0 - distance; x <= distance; x++)
        {
            for(int y = 0 - distance; y <= distance; y++)
            {
                for(int z = 0 - distance; z <= distance; z++)
                {
                    if(x + y + z == 0)
                    {
                        int [] newTileCoordinates = new int[] {x+xStart, z+zStart};
                        if(GetTile(newTileCoordinates))
                        {
                            tileList.Add(GetTile(newTileCoordinates));
                        }
                    }
                }
            }
        }
        return tileList;
    }
    //// change to recieve list of tiles and overload with single tile
    public List<GameObject> GetEnemy(GameObject tile)
    {
        return tile.GetComponent<Hex>().GetEnemy();
    }

    //// Find a list of whitelisted tiles from the given tile and array
    public List<GameObject> WhitelistArrayToList(int[,] whitelistArray, GameObject selectedTile)
    {
        int [] selectedCoordinate =  GetCoordinates(selectedTile);
        int [] evaluatedTile = new  int[2];
        List <GameObject> whitelistList = new List <GameObject>();
        for(int x = 0; x < whitelistArray.GetLength(0); x++)
        {
            int q = selectedCoordinate[0] + (x - 4); // Figure q dynamically from the array sized in the future if needed
            for(int y = 0; y < whitelistArray.GetLength(1); y++)
            {
                int r = selectedCoordinate[1] + (y - 4); // Figure r dynamically from the array sized in the future if needed
                if(whitelistArray[x,y]==1)
                {
                    evaluatedTile = new int[] {q,r};
                    if(GetTile(evaluatedTile))
                    {
                        whitelistList.Add(GetTile(evaluatedTile));
                    }
                }
            }
        }
        return whitelistList;
    }
}
