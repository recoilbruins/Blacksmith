using UnityEngine;

[CreateAssetMenu(fileName = "ArmorSO", menuName = "Scriptable Objects/ArmorSO")]
public class ArmorSO : ScriptableObject
{
    public int armorVal;
    public int evasion;

    public int health;
    public int mana;
    public int strength;
    public int dexterity;
    public int intelligence;
    public float moveSpeed;

    //0: common, 1: uncommon, 2: rare, 3: very rare, 4: Legendary, 5: Unique
    public enum Rarity { COMMON, UNCOMMON, RARE, VERYRARE, LEGENDARY, UNIQUE };
    public Rarity rarity;

    public enum ArmorType { HELMET, CHEST, GLOVES, BOOTS, SHIELD };
    public ArmorType armorType;
}
