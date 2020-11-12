using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS_Cam;

public class CharacterController : MonoBehaviour
{
    public GameObject characterPrefab;
    private DirectedAgent directedAgent;
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
        //character02 = Instantiate(characterPrefab, spawnPoint02, Quaternion.identity);
        //character03 = Instantiate(characterPrefab, spawnPoint03, Quaternion.identity);
        //character04 = Instantiate(characterPrefab, spawnPoint04, Quaternion.identity);
        selectedCharacter = character01;
        directedAgent = character01.GetComponent<DirectedAgent>();
        rtscamera = Camera.main.GetComponent<RTS_Camera>();
    }

    void OnEnable() {
        currentCommand = 'I';
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) {
            if (CameraCaster.Instance.SelectedTarget() != null) {
                directedAgent.SetAttackTarget(CameraCaster.Instance.SelectedTarget());
            }
            else if (CameraCaster.Instance.SelectedDestination() != null) {
                directedAgent.MoveToLocation((Vector3)CameraCaster.Instance.SelectedDestination());
            }
        }
        if (Input.GetKeyDown(KeyCode.B) && currentCommand == 'I'){
            currentCommand = 'B';
        }
        if (Input.GetKeyDown(KeyCode.Q) && currentCommand == 'I'){
            Application.Quit();
        }


        if (currentCommand == 'B') {
            GameController.Instance.Build();
        }
        if (Input.GetKeyDown(KeyCode.Space)){
            rtscamera.SetTarget(selectedCharacter.transform);
        }
        if (Input.GetKeyUp(KeyCode.Space)||(cameraJump)){
            rtscamera.ResetTarget();
            cameraJump = false;
        }
        if  (Input.GetKeyDown(KeyCode.Alpha1)||Input.GetKeyDown(KeyCode.Keypad1)){
            rtscamera.SetTarget(character01.transform);
            selectedCharacter = character01;
            directedAgent = character01.GetComponent<DirectedAgent>();
            cameraJump = true;
        }
        if  (Input.GetKeyDown(KeyCode.Alpha2)||Input.GetKeyDown(KeyCode.Keypad2)){
            rtscamera.SetTarget(character02.transform);
            selectedCharacter = character02;
            directedAgent = character02.GetComponent<DirectedAgent>();
            cameraJump = true;
        }
        if  (Input.GetKeyDown(KeyCode.Alpha3)||Input.GetKeyDown(KeyCode.Keypad3)){
            rtscamera.SetTarget(character03.transform);
            selectedCharacter = character03;
            directedAgent = character03.GetComponent<DirectedAgent>();
            cameraJump = true;
        }
        if  (Input.GetKeyDown(KeyCode.Alpha4)||Input.GetKeyDown(KeyCode.Keypad4)){
            rtscamera.SetTarget(character04.transform);
            selectedCharacter = character04;
            directedAgent = character04.GetComponent<DirectedAgent>();
            cameraJump = true;
        }

    }
}
