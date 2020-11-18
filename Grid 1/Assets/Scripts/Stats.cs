using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int maxLife = 100;
    public int currentLife;
    
    // Start is called before the first frame update
    void Start()
    {
        currentLife = maxLife;
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        HealthBar healthBar = transform.Find("Healthbar").GetComponent<HealthBar>();
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
