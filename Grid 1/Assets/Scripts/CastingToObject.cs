using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingToObject : MonoBehaviour
{
    public Transform selectedObject;
    public RaycastHit hit;
    private int layerMask = 1 << 8;

    public Transform SelectedTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            selectedObject = hit.transform;
        }
        else
        {
            selectedObject = null;
        }
        //Debug.Log(selectedObject);
        return selectedObject;
    }
}
