using UnityEngine;

public abstract class LifeForm : MonoBehaviour
{
    public float maxHealth { get; set; }
    public float maxStamina { get; set; }
    public float maxMana { get; set; }

    public float currentHealth { get; set; }
    public float currentStamina { get; set; }
    public float currentMana { get; set; }

    public abstract int TakeDamage (float damage);

    public abstract void Die();
}
