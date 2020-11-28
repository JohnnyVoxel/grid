using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS_Cam;

public class CharacterController : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject characterPrefab2;
    public GameObject characterPrefab3;
    public GameObject characterPrefab4;
    private CharacterAgent characterAgent;
    public Vector3 destination;
    public char currentCommand = 'I';
    private RTS_Camera rtscamera;
    private GameObject character01;
    private GameObject character02;
    private GameObject character03;
    private GameObject character04;
    private GameObject selectedCharacter;
    private bool cameraJump = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPoint01 = new Vector3 (16.0f,0.4f,10.3f);
        Vector3 spawnPoint02 = new Vector3 (16.9f,0.4f,10.3f);
        Vector3 spawnPoint03 = new Vector3 (16.0f,0.4f,8.8f);
        Vector3 spawnPoint04 = new Vector3 (16.9f,0.4f,8.8f);
        character01 = Instantiate(characterPrefab, spawnPoint01, Quaternion.identity);
        character02 = Instantiate(characterPrefab2, spawnPoint02, Quaternion.identity);
        character03 = Instantiate(characterPrefab3, spawnPoint03, Quaternion.identity);
        character04 = Instantiate(characterPrefab4, spawnPoint04, Quaternion.identity);
        character01.name = "Player 1";
        character02.name = "Player 2";
        character03.name = "Player 3";
        character04.name = "Player 4";
        selectedCharacter = character01;
        characterAgent = character01.GetComponent<CharacterAgent>();
        rtscamera = Camera.main.GetComponent<RTS_Camera>();
    }

    void OnEnable() {
        currentCommand = 'I';
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) 
        {
            if (CameraCaster.Instance.SelectedDestination() != null) 
            {
                characterAgent.MoveToLocation((Vector3)CameraCaster.Instance.SelectedDestination());
            }
        }
        if (Input.GetMouseButton(0))
        {
            characterAgent.Execute();
        }
        if (Input.GetKeyDown(KeyCode.B) && currentCommand == 'I'){
            currentCommand = 'B';
        }
        if (Input.GetKeyDown(KeyCode.A) && currentCommand == 'I'){
            //currentCommand = 'A';
            characterAgent.BasicAttackInitiate();
        }
        if (Input.GetKeyDown(KeyCode.Q) && currentCommand == 'I'){
            currentCommand = 'Q';
            characterAgent.BasicAttackInitiate(currentCommand);
        }
        if (Input.GetKeyDown(KeyCode.Q) && currentCommand == 'I'){
            currentCommand = 'W';
            characterAgent.BasicAttackInitiate(currentCommand);
        }
        if (Input.GetKeyDown(KeyCode.Q) && currentCommand == 'I'){
            currentCommand = 'E';
            characterAgent.BasicAttackInitiate(currentCommand);
        }
        if (Input.GetKeyDown(KeyCode.Q) && currentCommand == 'I'){
            currentCommand = 'R';
            characterAgent.BasicAttackInitiate(currentCommand);
        }

        //// Cancel ////
        if (Input.GetKeyDown(KeyCode.Escape) && currentCommand == 'I'){
            Application.Quit();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && currentCommand != 'B'){
            characterAgent.CancelCommand();
        }

        //// Build Command ////
        if (currentCommand == 'B') {
            GameController.Instance.Build();
        }

        //// Camera Control ////
        if (Input.GetKeyDown(KeyCode.Space)){
            rtscamera.SetTarget(selectedCharacter.transform);
        }
        if (Input.GetKeyUp(KeyCode.Space)||(cameraJump)){
            rtscamera.ResetTarget();
            cameraJump = false;
        }

        //// Character Selection ////
        if  (Input.GetKeyDown(KeyCode.Alpha1)||Input.GetKeyDown(KeyCode.Keypad1)){
            rtscamera.SetTarget(character01.transform);
            selectedCharacter = character01;
            characterAgent = character01.GetComponent<CharacterAgent>();
            cameraJump = true;
        }
        if  (Input.GetKeyDown(KeyCode.Alpha2)||Input.GetKeyDown(KeyCode.Keypad2)){
            rtscamera.SetTarget(character02.transform);
            selectedCharacter = character02;
            characterAgent = character02.GetComponent<CharacterAgent>();
            cameraJump = true;
        }
        if  (Input.GetKeyDown(KeyCode.Alpha3)||Input.GetKeyDown(KeyCode.Keypad3)){
            rtscamera.SetTarget(character03.transform);
            selectedCharacter = character03;
            characterAgent = character03.GetComponent<CharacterAgent>();
            cameraJump = true;
        }
        if  (Input.GetKeyDown(KeyCode.Alpha4)||Input.GetKeyDown(KeyCode.Keypad4)){
            rtscamera.SetTarget(character04.transform);
            selectedCharacter = character04;
            characterAgent = character04.GetComponent<CharacterAgent>();
            cameraJump = true;
        }
    }
}
