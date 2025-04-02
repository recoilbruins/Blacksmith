using UnityEngine;

[CreateAssetMenu(fileName = "JewelerySO", menuName = "Scriptable Objects/JewelerySO")]
public class JewelrySO : ScriptableObject
{
    public float health;
    public float mana;
    public float stamina;

    public float damage;
    public float moveSpeed;
    public float attackSpeed;

    //0: common, 1: uncommon, 2: rare, 3: very rare, 4: Legendary, 5: Unique
    public enum Rarity { COMMON, UNCOMMON, RARE, VERYRARE, LEGENDARY, UNIQUE };
    public Rarity rarity;

    public enum JewelryType { HELMET, CHEST, GLOVES, BELT, GREAVES, BOOTS };
    public JewelryType jewelryType;
}
