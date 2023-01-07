using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int health = 100;

    public GameObject deathEffect;
    
    public virtual void ReduceHealth(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Died();
        }
    }

    public virtual void Died()
    {
        if(deathEffect != null)
        {
            var deathFX = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(deathFX);
        }

        Destroy(gameObject);
    }
}
