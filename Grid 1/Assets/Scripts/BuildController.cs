using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    // Prefabs for different structures
    public GameObject platform1Prefab;
    public GameObject platform2Prefab;
    public GameObject platform3Prefab;
    public GameObject bridge1Prefab;
    public GameObject bridge2Prefab;
    public GameObject stairs1Prefab;
    private GameObject selectedPrefab;      // Reference to the prefab passed to the build routine

    private static BoardController board;   // Reference to the board controller for determining hex availability
    public CameraCaster caster;             // Reference to the caster for determining currently selected hex
    //public char currentCommand = 'I';       // Stores user input for top level commands ((B)uild/(D)estroy/(I)dle/(U)pgrade)
    public int structureType = 0;           // Stores user input for which type of structure to build
    public GameController game;             // Stores reference to game controller. Do this programmatically in the future.

    private Vector3 spawnPoint = new Vector3 (0.0f, -20.0f, 0.0f);  // Point to spawn new instantiations until a legal target location is determined
    private GameObject structure = null;           // Reference to the currently selected structure
    private Transform selectedTile = null;  // The tile transform that was returned from the caster during the current loop
    private Transform previousTile = null;  // The tile transform that was returned from the caster during the prior loop
    private Hex selectedHex = null;         // The Hex component of the currently selected tile
    private Hex previousHex = null;         // The Hex component that was selected prior to selectedHex
    private bool available = false;         // Holder for the availability of the tile returned from the board controller
    private bool newTile = false;           // Indicates that a new tile has been selected by the caster

    // Start is called before the first frame update
    void Start()
    {
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
    }

    void OnEnable() {
        structureType = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        selectedTile = caster.SelectedTile();
        if (selectedTile != previousTile){
            newTile = true;
        }
        if(selectedTile){
            selectedHex = selectedTile.GetComponent<Hex>();
        }
        if (Input.GetKeyDown(KeyCode.Q)){
                game.Play();
        }
        if (structureType==0)
        {
            if (Input.GetKeyDown(KeyCode.X)){
                previousHex = null; // Hacky way of making the Destroy routine actually evaluate the hex that is selected when the function is called
                structureType = -1;
                Debug.Log("Destroy");
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                structureType = 1;
                selectedPrefab = platform1Prefab;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)){
                structureType = 2;
                selectedPrefab = platform2Prefab;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)){
                structureType = 3;
                selectedPrefab = platform3Prefab;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)){
                structureType = 4;
                selectedPrefab = bridge1Prefab;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)){
                structureType = 5;
                selectedPrefab = stairs1Prefab;
            }
            if (Input.GetKeyDown(KeyCode.Alpha6)){
                structureType = 6;
                selectedPrefab = bridge2Prefab;
            }
        }
        if (structureType>0){
            BuildStructure(selectedPrefab);
        }
        if (structureType==-1){
            DestroyRoutine();
        }

        previousTile = selectedTile;
        previousHex = selectedHex;
        newTile = false;
    }

    private void BuildStructure(GameObject structurePrefab)
    {
        /////// This should not have to be declared again. Figure out why it does not work globally.
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
        /////////////////////////////////////////////////////////////////////////////////////////////
        
        // If the structure has not been instantiated yet, do it. Should only run on first loop.
        if (!structure){
            structure = Instantiate(structurePrefab, spawnPoint, Quaternion.identity);
            structure.name = "structure";
            // If a tile is selected, check the availability against the structure type, color as appropriate
            if (selectedTile){
                available = board.GetAvailability(structure.GetComponent<Structure>().GetEdges(), selectedTile.gameObject);
                if (available){
                    SetHighlight(structure.transform, Color.cyan);
                }
                else {
                    SetHighlight(structure.transform, Color.red);
                }
            }
        }
        else {
            // If a tile is currently selected and its a newly selected tile or newly instantiated structure
            if (selectedTile && (newTile || (structure.transform.position == spawnPoint))) {
                // If selected hex does not already have a structure on it move the new structure to that tile
                if(selectedHex.Structure != 0) {
                    SetHighlight(structure.transform, Color.red);
                    available = false;
                }
                else {
                    structure.transform.position = selectedTile.position;
                    available = board.GetAvailability(structure.GetComponent<Structure>().GetEdges(), selectedTile.gameObject);
                    if (available){
                        SetHighlight(structure.transform, Color.cyan);
                    }
                    else {
                        SetHighlight(structure.transform, Color.red);
                    }
                }
            }
            //Rotate
            if (Input.GetMouseButtonDown(1))
            {
                structure.GetComponent<Structure>().Rotate();
                available = board.GetAvailability(structure.GetComponent<Structure>().GetEdges(), selectedTile.gameObject);
                if (available){
                    SetHighlight(structure.transform, Color.cyan);
                }
                else {
                    SetHighlight(structure.transform, Color.red);
                }
            }
            //Apply
            if (Input.GetMouseButtonDown(0) && selectedHex.Structure == 0 && available)
            {
                SetHighlight(structure.transform, Color.white);
                structure.transform.parent = selectedTile;
                structure.GetComponent<Structure>().SetEdges();
                structure = null;
                structureType = 0;
                board.RebuildNavMesh();
                //currentCommand = 'I';
                game.Return();
            }
            //Escape
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Destroy(structure.gameObject);
                structureType = 0;
                //currentCommand = 'I';
                game.Return();
            } 
        }
    }

    private void DestroyRoutine()
    {
        /////// This should not have to be declared again. Figure out why it does not work globally.
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
        /////////////////////////////////////////////////////////////////////////////////////////////
        
        /////////// Color initially selected structure ////////////////////
        if (selectedHex != previousHex)
        {
            if(selectedHex){

                if(selectedHex.Structure != 0) {
                    SetHighlight(selectedHex.transform.GetChild(0).transform, Color.red);
                }
            }
            if(previousHex){
                if(previousHex.Structure != 0) {
                    SetHighlight(previousHex.transform.GetChild(0).transform, Color.white);
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && (selectedHex.Structure == 1))
        {
            selectedTile.GetChild(0).gameObject.transform.position = spawnPoint;
            Destroy(selectedTile.GetChild(0).gameObject);
            selectedHex.ResetHex();
            //currentCommand = 'I';
            structureType = 0;
            Debug.Log("Rebuild");
            board.RebuildNavMesh();
            game.Play();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(selectedHex.Structure != 0) {
                SetHighlight(selectedHex.transform.GetChild(0).transform, Color.white);
            }
            //currentCommand = 'I';
            structureType = 0;
        }
    }

    private void SetHighlight(Transform newTransform, Color newColor)
    {
        foreach (Transform child in newTransform)
        {
            child.gameObject.GetComponent<Renderer>().material.color = newColor;
        }
    }
}