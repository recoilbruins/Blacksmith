using BlacksmithCharacter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDamageCaluclator : MonoBehaviour
{
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
