using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    [SerializeField] private GameObject structurePrefab;
    [SerializeField] private GameObject bridgePrefab;
    [SerializeField] private GameObject stairsPrefab;

    public static BoardController board;
    public CastingToObject caster;
    public char currentCommand = 'E';
    private Vector3 spawnPonit = new Vector3 (0.0f, -20.0f, 0.0f);
    private GameObject structure;
    private Transform oldSelectedTile = null;
    private Hex selectedHex = null;
    bool available = false;

    private void Start() 
    {
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
    }

    private void Update()
    {
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
        int layerMask = 1 << 8;
        int tileState = 0;
        /////////// BRIDGE //////////////
        if (Input.GetKeyDown(KeyCode.N) && currentCommand == 'E')
        {
            currentCommand = 'N';
            structure = Instantiate(bridgePrefab, spawnPonit, Quaternion.identity);
            structure.name = "structure";
            structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (currentCommand == 'N')
        {
            Transform selectedTile = caster.SelectedTile();
            if((selectedTile != oldSelectedTile)||(selectedTile == null))
            {
                
                if(selectedTile != null)
                {
                    selectedHex = selectedTile.GetComponent<Hex>();
                    available = board.GetAvailability(structure.GetComponent<Structure>().GetEdges() ,selectedTile.gameObject);
                    //int[] availability = selectedHex.GetAvailability();
                    tileState = selectedHex.Structure;
                    if(tileState == 0)
                    {
                        structure.transform.position = selectedTile.position;
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                structure.GetComponent<Structure>().Rotate();
                available = board.GetAvailability(structure.GetComponent<Structure>().GetEdges() ,selectedTile.gameObject);
            }
            if (Input.GetMouseButtonDown(0) && tileState == 0 && available != false)
            {
                structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.white;
                structure.transform.parent = selectedTile;
                structure.GetComponent<Structure>().SetEdges();
                currentCommand = 'E';
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(structure);
                currentCommand = 'E';
            }
            oldSelectedTile = selectedTile;
        }
        /////////// STAIRS //////////////
        if (Input.GetKeyDown(KeyCode.M) && currentCommand == 'E')
        {
            currentCommand = 'M';
            structure = Instantiate(stairsPrefab, spawnPonit, Quaternion.identity);
            structure.name = "structure";
            structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.cyan;
            int[] test = structure.GetComponent<Structure>().GetEdges();
            Debug.Log(test[0] + " " + test[1] + " " + test[2] + " " + test[3] + " " + test[4] + " " + test[5]);
        }
        if (currentCommand == 'M')
        {
            Transform selectedTile = caster.SelectedTile();
            if((selectedTile != oldSelectedTile)||(selectedTile == null))
            {
                
                if(selectedTile != null)
                {
                    selectedHex = selectedTile.GetComponent<Hex>();
                    available = board.GetAvailability(structure.GetComponent<Structure>().GetEdges() ,selectedTile.gameObject);
                    
                    tileState = selectedHex.Structure;
                    if(tileState == 0)
                    {
                        structure.transform.position = selectedTile.position;
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                structure.GetComponent<Structure>().Rotate();
                available = board.GetAvailability(structure.GetComponent<Structure>().GetEdges() ,selectedTile.gameObject);
            }
            if (Input.GetMouseButtonDown(0) && tileState == 0 && available != false)
            {
                structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.white;
                structure.transform.parent = selectedTile;
                structure.GetComponent<Structure>().SetEdges();
                currentCommand = 'E';
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(structure);
                currentCommand = 'E';
            }
            oldSelectedTile = selectedTile;
        }
        /////////// Platform ///////////////
        if (Input.GetKeyDown(KeyCode.B) && currentCommand == 'E')
        {
            currentCommand = 'B';
            structure = Instantiate(structurePrefab, spawnPonit, Quaternion.identity);
            structure.name = "structure";
            structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (currentCommand == 'B')
        {
            Transform selectedTile = caster.SelectedTile();
            if((selectedTile != oldSelectedTile)||(selectedTile == null))
            {  
                if(selectedTile != null)
                {
                    selectedHex = selectedTile.GetComponent<Hex>();
                    available = board.GetAvailability(structure.GetComponent<Structure>().GetEdges() ,selectedTile.gameObject);
                    //int[] availability = selectedHex.GetAvailability();
                    tileState = selectedHex.Structure;
                    if(tileState == 0)
                    {
                        structure.transform.position = selectedTile.position;
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                available = board.GetAvailability(structure.GetComponent<Structure>().GetEdges() ,selectedTile.gameObject);
                structure.GetComponent<Structure>().Rotate();
            }
            if (Input.GetMouseButtonDown(0) && tileState == 0 && available != false)
            {
                structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.white;
                structure.transform.parent = selectedTile;
                structure.GetComponent<Structure>().SetEdges();
                //selectedHex.Structure = 1;
                currentCommand = 'E';
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(structure);
                currentCommand = 'E';
            }
            oldSelectedTile = selectedTile;
        }
        /////////// DESTROY ///////////////
        if (Input.GetKeyDown(KeyCode.V) && currentCommand == 'E')
        {
            currentCommand = 'V';
        }
        if (currentCommand == 'V')
        {
            Transform selectedTile = caster.SelectedTile();
            if((selectedTile != oldSelectedTile)||(selectedTile == null))
            {  
                if(selectedTile != null)
                {
                    selectedHex = selectedTile.GetComponent<Hex>();
                    tileState = selectedHex.Structure;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                tileState = selectedHex.Structure;
                if(tileState == 1)
                {
                    //Destroy(hit.transform.GetChild(0).gameObject);
                    Destroy(selectedTile.GetChild(0).gameObject);
                    selectedHex.ResetHex();
                }
                currentCommand = 'E';
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currentCommand = 'E';
            }
            oldSelectedTile = selectedTile;
        }
    }
}