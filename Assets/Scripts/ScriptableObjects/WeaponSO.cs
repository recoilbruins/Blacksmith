using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    public AnimatorOverrideController animatorOverrideController;

    public const string LIGHT_ATTACK_ONE_ANIMATION_NAME = "";
    public const string LIGHT_ATTACK_TWO_ANIMATION_NAME = "";

    public const string HEAVY_ATTACK_ONE_ANIMATION_NAME = "";
    public const string HEAVY_ATTACK_TWO_ANIMATION_NAME = "";

    public const string JUMP_ATTACK_ANIMATION_NAME = "";
    public const string SPRINT_ATTACK_ANIMATION_NAME = "";

    public const string BLOCK__ANIMATION_NAME = "";


    public enum Rarity { COMMON, UNCOMMON, RARE, VERYRARE, LEGENDARY, UNIQUE };
    public Rarity rarity;
    public enum WeaponType { ONEHANDWEAPON, TWOHANDWEAPON, SHIELD, SPELL };
    public WeaponType weaponType;

    public float attackSpeed;

    public float weaponDamage;
    [HideInInspector] public float defaultDamage;
}
