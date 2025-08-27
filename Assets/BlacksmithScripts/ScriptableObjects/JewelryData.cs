using UnityEngine;

public enum JewelryType { Ring, Amulet }

[CreateAssetMenu(fileName = "NewJewelry", menuName = "Items/Jewelry")]
public class JewelryData : ScriptableObject
{
    public string jewelryName;
    public JewelryType jewelryType;
    public Sprite icon;
    public GameObject prefab;

    public string description;
    public float weight;
    public int durability;

    public float damage;
    public float moveSpeed;
    public float attackSpeed;


    [Header("Stat Bonuses")]
    public int bonusHealth;
    public int bonusStamina;
    public int bonusMana;

    [Header("Resistances")]
    public float poisonResist;
    public float bleedResist;
    public float curseResist;

    [Header("Special Effects")]
    public bool regeneratesHealth;
    public bool boostsDamage;
    public bool reducesCooldown;

    public string specialEffectDescription;
}