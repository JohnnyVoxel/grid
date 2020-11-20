using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        Animator animator = this.transform.GetComponent<Animator>();
        agent.AddAttackAggro(caller);
        currentLife -= damage;
        float percentLife = (float)currentLife/(float)maxLife;
        healthBar.SetSize(percentLife);
        if (currentLife <= 0)
        {
            StartCoroutine("DestroyEnemy");
        }
    }

    IEnumerator DestroyEnemy()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<EnemyAgent>().enabled = false;
        this.transform.Find("Body").GetComponent<BoxCollider>().enabled = false;
        GetComponent<Animator>().SetTrigger("Die");
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
