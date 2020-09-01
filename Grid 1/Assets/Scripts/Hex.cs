using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public int structure;
    
    // Start is called before the first frame update
    void Start()
    {
        structure = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStructure(int newType)
    {
        structure = newType;
    }

    public int GetStructure()
    {
        return structure;
    }
}
