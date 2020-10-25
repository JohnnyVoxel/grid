using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCaster : MonoBehaviour
{
    public Transform selectedObject;
    public RaycastHit hit;
    private int layerMask = 1 << 8;
    private Camera rtsCamera;

     void Start() 
    {
        rtsCamera = GetComponentInParent<Camera>();    
    }

    public Transform SelectedTile()
    {
        Ray ray = rtsCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
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

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }
}
