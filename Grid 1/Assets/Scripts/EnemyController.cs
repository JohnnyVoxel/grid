using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject enemyPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawner());
        //SpawnWave();
    }

    // Update is called once per frame
    IEnumerator EnemySpawner()
    {
        for(int i = 0; i < 10; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                SpawnWave();
                yield return new WaitForSeconds(2);
            }
            yield return new WaitForSeconds(30);
        }
    }

    private void SpawnWave()
    {
        // Eventually pull these as a matrix from the board controller and loop through
        Vector3 spawnPoint01 = new Vector3 (9.0f,0.4f,13.85f);
        Vector3 spawnPoint02 = new Vector3 (16.5f,0.4f,18.2f);
        Vector3 spawnPoint03 = new Vector3 (24.0f,0.4f,13.85f);
        Vector3 spawnPoint04 = new Vector3 (24.0f,0.4f,5.2f);
        Vector3 spawnPoint05 = new Vector3 (16.5f,0.4f,0.87f);
        Vector3 spawnPoint06 = new Vector3 (9.0f,0.4f,5.2f);
        Vector3 target = new Vector3 (16.5f,0.4f,9.5f);

        GameObject enemy01 = Instantiate(enemyPrefab, spawnPoint01, Quaternion.identity);
        GameObject enemy02 = Instantiate(enemyPrefab, spawnPoint02, Quaternion.identity);
        GameObject enemy03 = Instantiate(enemyPrefab, spawnPoint03, Quaternion.identity);
        GameObject enemy04 = Instantiate(enemyPrefab, spawnPoint04, Quaternion.identity);
        GameObject enemy05 = Instantiate(enemyPrefab, spawnPoint05, Quaternion.identity);
        GameObject enemy06 = Instantiate(enemyPrefab, spawnPoint06, Quaternion.identity);

        enemy01.GetComponent<EnemyAgent>().MoveToLocation(target);
        enemy02.GetComponent<EnemyAgent>().MoveToLocation(target);
        enemy03.GetComponent<EnemyAgent>().MoveToLocation(target);
        enemy04.GetComponent<EnemyAgent>().MoveToLocation(target);
        enemy05.GetComponent<EnemyAgent>().MoveToLocation(target);
        enemy06.GetComponent<EnemyAgent>().MoveToLocation(target);
    }
}
