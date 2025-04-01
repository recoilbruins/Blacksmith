using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    public void TakeDamage(float damage)
    {
        maxHealth -= damage;
        if (maxHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        // Play death animation
        // Death UI
        // Respawn
    }
}
