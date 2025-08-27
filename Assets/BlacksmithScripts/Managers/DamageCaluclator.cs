using BlacksmithCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaluclator : MonoBehaviour
{
    public static DamageCaluclator Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public static void DealDamage(IDamageable target, float baseDamage, DamageType type, GameObject attacker = null)
    {
        float finalDamage = CalculateFinalDamage(baseDamage, target.GetCharacterStats(), type);

        target.TakeDamage(finalDamage, type, attacker);
    }

    private static float CalculateFinalDamage(float baseDamage, CharacterManager stats, DamageType type)
    {
        float resist = 0f;

        switch (type)
        {
            case DamageType.Physical:
                resist = stats.physicalDefense;
                break;
            case DamageType.Magic:
                resist = stats.magicDefense;
                break;
            case DamageType.Fire:
                resist = stats.fireResistance;
                break;
            default:
                resist = 0f;
                break;
        }

        float mitigation = resist / (100f + resist); // Diminishing returns
        float final = baseDamage * (1f - mitigation);
        return Mathf.Max(final, 0f);
    }

    public float PhysicalDamageCalculation(CharacterManager attacker, EquippedWeapons attackWeapons, EquippedArmor defenderArmor, CharacterManager defender)
    {
        float damage = 0;
        float attackerStrength = attacker.strength;
        float defenderEndurance = defender.endurance;
        float attackerLevel = attacker.characterLevel;
        float defenderLevel = defender.characterLevel;
        damage = attackWeapons.weaponDamage * (attackerStrength / defenderEndurance) * (attackerLevel / defenderLevel);
        return damage;
    } 
    public float MagicDamageCalculation(CharacterManager attacker, EquippedWeapons attackWeapons, EquippedArmor defenderArmor, CharacterManager defender)
    {
        float damage = 0;
        float attackIntelligence = attacker.strength;
        float defenderEndurance = defender.endurance;
        float attackerLevel = attacker.characterLevel;
        float defenderLevel = defender.characterLevel;
        damage = attackWeapons.weaponDamage * (attackIntelligence / defenderEndurance) * (attackerLevel / defenderLevel);
        return damage;
    }
}
