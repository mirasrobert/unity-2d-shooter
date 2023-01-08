using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;

    [SerializeField] int health = 100;

    //public GameObject deathEffect;

    private void Awake()
    {
        health = maxHealth;
    }

    public virtual void ReduceHealth(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Died();
        }
    }

    public virtual void Died()
    {
        /*
        if(deathEffect != null)
        {
            var deathFX = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(deathFX);
        }
        */

        Destroy(gameObject);
    }

    public void AddHealth(int addHealthAmount)
    {
        int newHealth = this.health + addHealthAmount;

        // Prevent health over 100
        if(newHealth > 100)
        {
            this.health = 100;
        }
        else
        {
            this.health += addHealthAmount;
        }
        
    }

    public int GetHealth()
    {
        return health;
    }

}
