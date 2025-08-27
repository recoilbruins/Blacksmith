using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { OneHanded, TwoHanded, Spell, Unarmed, DualWield, Shield, }
public enum DamageType { Physical, Fire, Ice, Lightning, Magic }

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Items/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public WeaponType weaponType;
    public DamageType damageType;
    public Sprite icon;
    public GameObject prefab;
    public AnimationClip[] attackAnimations;

    public List<AttackData> rightHandLightAttackCombo;
    public List<AttackData> leftHandLightAttackCombo;
    public List<AttackData> heavyAttackCombo;

    public int maxLightComboCount = 6; // Maximum number of combos allowed

    public AnimatorOverrideController animatorOverrideController;

    public float baseDamage;
    public float staminaCost;
    public float attackSpeed;
    public float poiseDamage;
}
