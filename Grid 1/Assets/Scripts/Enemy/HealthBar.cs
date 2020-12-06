using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;
    private Transform target;
    
    // Start is called before the first frame update
    private void Start()
    {
        bar = transform.Find("Bar");
        target = transform.Find("Target");
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

    public void TargetOn()
    {
        target.gameObject.SetActive(true);
    }

    public void TargetOff()
    {
        target.gameObject.SetActive(false);
    }

    public bool GetTarget()
    {
        return target.gameObject.activeSelf;
    }
}
