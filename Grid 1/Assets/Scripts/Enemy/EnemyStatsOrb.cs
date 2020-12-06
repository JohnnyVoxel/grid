using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsOrb : EnemyStats
{
    public void Start()
    {
        maxLife = 40;
        damage = 2;
        currentLife = maxLife;
    }
}
