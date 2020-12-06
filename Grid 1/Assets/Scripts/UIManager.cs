using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text menu;
    public BuildController build;
    public CharacterController character;
    public GameController game;
    void Start()
    {
        //BuildController build = GameObject.Find("Selection").GetComponent<BuildController>();
    }
    
    void Update()
    {
        //Debug.Log(game.currentMode);
        if(game.currentMode == build.gameObject)
        {
            if(build.structureType == 0)
            {
                menu.text = "BUILD MENU:\n 1) Open Platform\n 2) Tri-Platform\n 3) Half-Platform\n 4) Bridge\n 5) Stairs\n 6) Monkeybars\n X) Destroy\n Esc) Exit";
            }
            else if(build.structureType > 0)
            {
                menu.text = "BUILD MENU:\n L-click) Place\n R-click) Rotate\n Esc) Back";
            }
            else if(build.structureType == -1)
            {
                menu.text = "DESTROY MENU:\n L-click) Destroy\n Esc) Back";
            }
        }
        if(game.currentMode == character.gameObject)
        {
            menu.text = "12/2/2020\nPLAY:\n R-click) Move\n 1-4) Switch character\n A) Attack\n B) Build\n Esc) Quit\n Space) Camera follow";
        }
    }
}
