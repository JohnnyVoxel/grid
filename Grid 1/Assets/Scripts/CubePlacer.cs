using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    [SerializeField] private GameObject structurePrefab;
    [SerializeField] private GameObject bridgePrefab;
    public CastingToObject caster;
    private char currentCommand = 'E';
    private Vector3 spawnPonit = new Vector3 (0.0f, -20.0f, 0.0f);
    private GameObject structure;

    private void Update()
    {
        int layerMask = 1 << 8;
        int tileState = 0;
        /////////// BRIDGE //////////////
        if (Input.GetKeyDown(KeyCode.N) && currentCommand == 'E')
        {
            currentCommand = 'N';
            structure = Instantiate(bridgePrefab, spawnPonit, Quaternion.identity);
            structure.name = "structure";
            structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (currentCommand == 'N')
        {
            Transform selectedTile = caster.SelectedTile();
            Hex selectedHex = null;
            if(selectedTile != null)
            {
                selectedHex = selectedTile.GetComponent<Hex>();
                int[] availability = selectedHex.GetAvailability();//////////////////////////////////////
                Debug.Log(availability[0] + " " + availability[1] + " " + availability[2] + " " + availability[3] + " " + availability[4] + " " + availability[5]);
                tileState = selectedHex.Structure;
                if(tileState == 0)
                {
                    structure.transform.position = selectedTile.position;
                }
            }
            if (Input.GetMouseButtonDown(0) && tileState == 0)
            {
                structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.white;
                structure.transform.parent = selectedTile;
                selectedHex.Structure = 1;
                currentCommand = 'E';
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(structure);
                currentCommand = 'E';
            }
        }
        /////////// BUILD ///////////////
        if (Input.GetKeyDown(KeyCode.B) && currentCommand == 'E')
        {
            currentCommand = 'B';
            structure = Instantiate(structurePrefab, spawnPonit, Quaternion.identity);
            structure.name = "structure";
            structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.cyan;
        }
        if (currentCommand == 'B')
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Hex selectedTile = null;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                selectedTile = hit.transform.GetComponent<Hex>();
                if(selectedTile != null)
                {
                    tileState = selectedTile.Structure;
                    if(tileState == 0)
                    {
                        structure.transform.position = hit.transform.position;
                        //structure.transform.parent = hit.transform;
                        //selectedTile.Structure = 1;
                    }
                }
            }
            if (Input.GetMouseButtonDown(0) && tileState == 0)
            {
                structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.white;
                structure.transform.parent = hit.transform;
                //Debug.Log("Shaz");
                selectedTile.Structure = 1;
                structure.GetComponent<Structure>().SetEdge();
                currentCommand = 'E';
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(structure);
                currentCommand = 'E';
            }
        }
        /////////// DESTROY ///////////////
        if (Input.GetKeyDown(KeyCode.V) && currentCommand == 'E')
        {
            currentCommand = 'V';
        }
        if (currentCommand == 'V')
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Hex selectedTile = null;
            tileState = 0;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                selectedTile = hit.transform.GetComponent<Hex>();
                if(selectedTile != null)
                {
                    tileState = selectedTile.Structure;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if(tileState == 1)
                {
                    Destroy(hit.transform.GetChild(0).gameObject);
                    selectedTile.Structure = 0;
                }
                currentCommand = 'E';
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currentCommand = 'E';
            }
        }
    }
}