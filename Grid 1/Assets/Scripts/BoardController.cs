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

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(map.GetLength(1));
        //Debug.Log(map.GetLength(0));
        GameObject[,] tileMap = new GameObject[map.GetLength(0),map.GetLength(1)];
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
                //Debug.Log(tileMap[q,r]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
