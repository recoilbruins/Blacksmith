using BlacksmithCharacter;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount, DamageType type, GameObject source);
    CharacterManager GetCharacterStats();
}

