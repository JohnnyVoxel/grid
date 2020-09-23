using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text menu;
    private BuildController build;
    
    void Update()
    {
        BuildController build = GameObject.Find("Selection").GetComponent<BuildController>();
        if(build.currentCommand == 'I')
        {
            menu.text = "MAIN MENU:\n B) Build\n X) Destroy\n";
        }
        if(build.currentCommand == 'B')
        {
            if(build.structureType == 0)
            {
                menu.text = "BUILD MENU:\n 1) Open Platform 1\n 2) Tri-Platform 2\n 3) Half-Platform 2\n 4) Bridge\n 5) Stairs\n Q) Back";
            }
            else
            {
                menu.text = "BUILD MENU:\n L-click) Place 1\n R-click) Rotate\n Q) Back";
            }
        }
        if(build.currentCommand == 'D')
        {
            menu.text = "DESTROY MENU:\n L-click) Destroy\n Q) Back";
        }
    }
}
