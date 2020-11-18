using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;
    
    // Start is called before the first frame update
    private void Start()
    {
        bar = transform.Find("Bar");
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void SetSize(float sizeNormalized)
    {
        if(sizeNormalized < 0.0f)
        {
            sizeNormalized = 0.0f;
        }
        if(sizeNormalized > 1.0f)
        {
            sizeNormalized = 1.0f;
        }
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }
}
