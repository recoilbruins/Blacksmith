using UnityEngine;

[CreateAssetMenu(fileName = "ArmorSO", menuName = "Scriptable Objects/ArmorSO")]
public class ArmorSO : ScriptableObject
{
    public float physicalArmorVal;
    public float magicArmorVal;
    //public int evasion;

    public float health;
    public float mana;
    public float stamina;

    public float strength;
    public float dexterity;
    public float endurance;
    public float vitality;
    public float intelligence;
    public float luck;

    public float moveSpeed;

    //0: common, 1: uncommon, 2: rare, 3: very rare, 4: Legendary, 5: Unique
    public enum Rarity { COMMON, UNCOMMON, RARE, VERYRARE, LEGENDARY, UNIQUE };
    public Rarity rarity;

    public enum ArmorType { HELMET, CHEST, GLOVES, BELT, GREAVES, BOOTS };
    public ArmorType armorType;
}
