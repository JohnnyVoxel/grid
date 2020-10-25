using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS_Cam;

public class CharacterController : MonoBehaviour
{
    public GameObject characterPrefab;
    public DirectedAgent directedAgent;
    public CameraCaster caster;
    public Vector3 destination;
    public char currentCommand = 'I';
    public GameController game;
    private RTS_Camera rtscamera;
    private GameObject character01;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPoint01 = new Vector3 (16.0f,0.4f,11.0f);
        character01 = Instantiate(characterPrefab, spawnPoint01, Quaternion.identity);
        directedAgent = character01.GetComponent<DirectedAgent>();
        rtscamera = Camera.main.GetComponent<RTS_Camera>();
    }

    void OnEnable() {
        currentCommand = 'I';
        Debug.Log("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) {
            if (caster.SelectedDestination() != null) {
                directedAgent.MoveToLocation((Vector3)caster.SelectedDestination());
            }
        }
        if (Input.GetKeyDown(KeyCode.B) && currentCommand == 'I'){
            currentCommand = 'B';
        }
        if (Input.GetKeyDown(KeyCode.Q) && currentCommand == 'I'){
            Application.Quit();
        }

        if (currentCommand == 'B') {
            game.Build();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rtscamera.SetTarget(character01.transform);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rtscamera.ResetTarget();
        }

    }
}
