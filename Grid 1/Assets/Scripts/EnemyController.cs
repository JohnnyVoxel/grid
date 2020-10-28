using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject enemyPrefab;
    private List <Vector3> spawns;
    private Vector3 target;
    
    // Start is called before the first frame update
    void Start()
    {
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
        spawns = board.GetEnemySpawnPoints();
        target = board.GetBasePosition();
        StartCoroutine(EnemySpawner());
    }
    
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
        foreach (Vector3 point in spawns)
        {
            GameObject enemy = Instantiate(enemyPrefab, point, Quaternion.identity);
            enemy.GetComponent<EnemyAgent>().MoveToLocation(target);
        }

    }
}
