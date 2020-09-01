using UnityEngine;

public class CubePlacer : MonoBehaviour
{
    [SerializeField] private GameObject structurePrefab;
    private char currentCommand = 'E';
    private Vector3 spawnPonit = new Vector3 (0.0f, -20.0f, 0.0f);
    private GameObject structure;

    private void Update()
    {
        int layerMask = 1 << 8;
        int tileState = 0;

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
                    tileState = selectedTile.GetStructure();
                    if(tileState == 0)
                    {
                        structure.transform.position = hit.transform.position;
                        //structure.transform.parent = hit.transform;
                        //selectedTile.SetStructure(1);
                    }
                }
            }
            if (Input.GetMouseButtonDown(0) && tileState == 0)
            {
                structure.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.white;
                structure.transform.parent = hit.transform;
                selectedTile.SetStructure(1);
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
                    tileState = selectedTile.GetStructure();
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if(tileState == 1)
                {
                    Destroy(hit.transform.GetChild(0).gameObject);
                    selectedTile.SetStructure(0);
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