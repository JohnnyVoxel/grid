using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject playerController;
    public GameObject buildController;
    public GameObject currentMode;
    public GameObject previousMode;

    static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }
            return _instance;
        }
    }
    void Awake()
    {
        currentMode = playerController;
        previousMode = null;
        playerController.SetActive(true);
        buildController.SetActive(false);
    }
    
    public void Build()
    {
        previousMode = currentMode;
        currentMode.SetActive(false);
        buildController.SetActive(true);
        currentMode = buildController;
    }

    public void Play()
    {
        previousMode = currentMode;
        currentMode.SetActive(false);
        playerController.SetActive(true);
        currentMode = playerController;
    }

    public void Return()
    {
        GameObject holder = previousMode;
        currentMode.SetActive(false);
        previousMode.SetActive(true);
        previousMode = currentMode;
        currentMode = holder;
    }


}
