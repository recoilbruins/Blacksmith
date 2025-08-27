using BlacksmithCharacter;
using UnityEngine;

public enum ArmorType { Helmet, Chestplate, Gauntlets, Belt, Greaves, Boots, Shield }
public enum ResistanceType { Physical, Fire, Ice, Lightning, Magic }

[CreateAssetMenu(fileName = "NewArmor", menuName = "Items/Armor")]
public class ArmorData : EquipableItemData
{
    public string armorName;
    public ArmorType armorType;
    public GameObject prefab;

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

    public float physicalDefense;
    public float magicDefense;
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;

    public int poiseBonus; // Resistance to staggering


    public override void ApplyEffects(CharacterManager stats)
    {
        stats.physicalDefense += physicalDefense;
        stats.magicDefense += magicDefense;
        stats.fireResistance += fireResistance;
    }

    public override void RemoveEffects(CharacterManager stats)
    {
        stats.physicalDefense -= physicalDefense;
        stats.magicDefense -= magicDefense;
        stats.fireResistance -= fireResistance;
    }
}