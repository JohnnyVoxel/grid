using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject enemyPrefab01;
    public GameObject enemyPrefab02;
    public List<GameObject> enemyList = new List<GameObject>();
    private List <Vector3> spawns;
    private Vector3 target;
    
    // Start is called before the first frame update
    void Start()
    {
        BoardController board = GameObject.Find("Board").GetComponent<BoardController>();
        enemyList.Add(enemyPrefab01);
        enemyList.Add(enemyPrefab02);
        spawns = board.GetEnemySpawnPoints();
        target = board.GetBasePosition();
        StartCoroutine(EnemySpawner());
    }
    
    IEnumerator EnemySpawner()
    {
        yield return new WaitForSeconds(1);
        for(int i = 0; i < 1; i++)
        {
            for(int j = 0; j < 1; j++)
            {
                SpawnWave(1);
                yield return new WaitForSeconds(4);
                SpawnWave(0);
                yield return new WaitForSeconds(4);
                SpawnWave(0);
                yield return new WaitForSeconds(4);
            }
            yield return new WaitForSeconds(60);
        }
    }

    private void SpawnWave(int enemyIndex)
    {
        foreach (Vector3 point in spawns)
        {
            Vector3 shiftedPoint = point;
            shiftedPoint.y += 0.2f; // Make sure the enemies spawn on the board
            GameObject enemy = Instantiate(enemyList[enemyIndex], shiftedPoint, Quaternion.identity);
            enemy.GetComponent<EnemyAgent>().BasePos = target;
            enemy.GetComponent<EnemyAgent>().MoveToLocation();
            //return; //////////// DEBUG /////////////
        }

    }
}
