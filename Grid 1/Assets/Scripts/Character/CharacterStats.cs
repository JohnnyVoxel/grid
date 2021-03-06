﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxLife = 100;
    public int currentLife;
    public float cooldownBasicAttack = 1.0f;
    public int attackDamage = 15;
    
    // Start is called before the first frame update
    void Start()
    {
        currentLife = maxLife;
    }

    // Update is called once per frame
    public void TakeDamage(int damage, GameObject caller)
    {
        HealthBar healthBar = transform.Find("Healthbar").GetComponent<HealthBar>();
        currentLife -= damage;
        float percentLife = (float)currentLife/(float)maxLife;
        healthBar.SetSize(percentLife);
        if (currentLife <= 0)
        {
            currentLife = 0;
            //Character KO

        }
    }
}
