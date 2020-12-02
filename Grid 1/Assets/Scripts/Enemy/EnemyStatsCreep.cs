using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsCreep : EnemyStats
{
    // Start is called before the first frame update
    void Start()
    {
        maxLife = 100;
        damage = 5;
        currentLife = maxLife;
    }
}
