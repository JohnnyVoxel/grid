using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int maxLife = 100;
    public int currentLife;
    
    // Start is called before the first frame update
    void Start()
    {
        currentLife = maxLife;
    }

    // Update is called once per frame
    public void TakeDamage(int damage, GameObject caller)
    {
        HealthBar healthBar = transform.Find("Healthbar").GetComponent<HealthBar>();
        EnemyAgent agent = this.transform.GetComponent<EnemyAgent>();
        agent.AddAttackAggro(caller);
        currentLife -= damage;
        float percentLife = (float)currentLife/(float)maxLife;
        healthBar.SetSize(percentLife);
        if (currentLife <= 0)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
