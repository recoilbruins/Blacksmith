using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject
{
    [Header("Animator")]
    public AnimatorOverrideController animatorOverrideController;
    public enum Rarity { COMMON, UNCOMMON, RARE, VERYRARE, LEGENDARY, UNIQUE };
    public Rarity rarity;
    public enum WeaponType { ONEHANDWEAPON, TWOHANDWEAPON, UNARMED, SHIELD, SPELL };
    public WeaponType weaponType;

    [Space(10)]
    [Header("Animation Names")]
    public string LIGHT_ATTACK_ONE_ANIMATION_NAME = "";
    public string LIGHT_ATTACK_TWO_ANIMATION_NAME = "";

    public string HEAVY_ATTACK_ONE_ANIMATION_NAME = "";
    public string HEAVY_ATTACK_TWO_ANIMATION_NAME = "";

    public string JUMP_ATTACK_ANIMATION_NAME = "";
    public string SPRINT_ATTACK_ANIMATION_NAME = "";
    public string BLOCK__ANIMATION_NAME = "";

    [Space(10)]
    [Header("Attack Variables")]
    public float attackSpeed;
    public int lightAttackMaxCombo;
    public int lightAttackHalfCombo;
    public float weaponDamage;
    [HideInInspector] public float defaultDamage;
}
