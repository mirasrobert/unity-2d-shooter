using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] Healthbar healthbar;
    public int healthAmount = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // Current health
            int playerHealth = collision.GetComponent<Player>().GetHealth();
            int newHealth = playerHealth + healthAmount;
            healthbar.SetHealthBar(newHealth); // Set Health Bar
            collision.GetComponent<Player>().AddHealth(healthAmount); // Add Health To Player

            Destroy(gameObject);

        }
    }
}
