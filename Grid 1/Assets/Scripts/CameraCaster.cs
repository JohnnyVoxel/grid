using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCaster : MonoBehaviour
{
    public Transform selectedObject;
    public RaycastHit hit;
    private int layerMaskBoard = 1 << 8;
    private int layerMaskBoardDefault = 257;
    private Camera rtsCamera;
    public GameObject sphere;

    static CameraCaster _instance;
    public static CameraCaster Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<CameraCaster>();
            }
            return _instance;
        }
    }

    void Start() 
    {
        rtsCamera = GetComponentInParent<Camera>();    
    }

    public Transform SelectedTile()
    {
        Ray ray = rtsCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskBoard))
        {
            selectedObject = hit.transform;
        }
        else
        {
            selectedObject = null;
        }
        return selectedObject;
    }

    public Vector3? SelectedDestination()  // Return a nullable Vector3
    {
        Ray ray = rtsCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskBoardDefault))
        {
            //Instantiate(sphere, hit.point, Quaternion.identity); ////////// Debugging for destination target hits.
            return hit.point;
        }
        else
        {
            return null;
        }
    }
}
