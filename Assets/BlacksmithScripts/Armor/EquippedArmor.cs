using Unity.Collections;
using UnityEngine;

public class EquippedArmor : MonoBehaviour
{
    public float totalPhysicalArmor { get; private set; } = 0;
    public float totalMagicArmor { get; private set; } = 0;
    public float totalStrength { get; private set; } = 0;
    public float totalDexterity { get; private set; } = 0;
    public float totalEndurance { get; private set; } = 0;
    public float totalVitality { get; private set; } = 0;
    public float totalIntelligence { get; private set; } = 0;
    public float totalLuck { get; private set; } = 0;

    [Header("Armor")]
    [ReadOnly]
    public Armor helmet = null;
    [ReadOnly]
    public Armor bodyArmor = null;
    [ReadOnly]
    public Armor gauntlets = null;
    [ReadOnly]
    public Armor belt = null;
    [ReadOnly]
    public Armor greaves = null;
    [ReadOnly]
    public Armor boots = null;
    [ReadOnly]
    public Jewelry amulet = null;
    [ReadOnly]
    public Jewelry leftRing = null;
    [ReadOnly]
    public Jewelry rightRing = null;

    public void EquipArmorPiece(ArmorSO armorSO, Armor armorItem)
    {
        switch(armorSO.armorType)
        {
            case ArmorSO.ArmorType.HELMET:
                UnequipArmorPiece(helmet);
                helmet = armorItem;
                break;
            case ArmorSO.ArmorType.CHEST:
                UnequipArmorPiece(bodyArmor);
                bodyArmor = armorItem;
                break;
            case ArmorSO.ArmorType.GLOVES:
                UnequipArmorPiece(gauntlets);
                gauntlets = armorItem;
                break;
            case ArmorSO.ArmorType.BELT:
                UnequipArmorPiece(belt);
                belt = armorItem;
                break;
            case ArmorSO.ArmorType.GREAVES:
                UnequipArmorPiece(greaves);
                greaves = armorItem;
                break;
            case ArmorSO.ArmorType.BOOTS:
                UnequipArmorPiece(boots);
                boots = armorItem;
                break;
        }
        UpdateArmorValues();
    }
    private void UnequipArmorPiece(Armor armorPiece)
    {
        armorPiece = null;
    }

    private void UpdateArmorValues()
    {
        totalPhysicalArmor = helmet.myArmorSO.physicalArmorVal + bodyArmor.myArmorSO.physicalArmorVal + gauntlets.myArmorSO.physicalArmorVal + belt.myArmorSO.physicalArmorVal + greaves.myArmorSO.physicalArmorVal + boots.myArmorSO.physicalArmorVal;
        
        totalMagicArmor = helmet.myArmorSO.magicArmorVal + bodyArmor.myArmorSO.magicArmorVal + gauntlets.myArmorSO.magicArmorVal + belt.myArmorSO.magicArmorVal + greaves.myArmorSO.magicArmorVal + boots.myArmorSO.magicArmorVal;
        
        totalStrength = helmet.myArmorSO.strength + bodyArmor.myArmorSO.strength + gauntlets.myArmorSO.strength + belt.myArmorSO.strength + greaves.myArmorSO.strength + boots.myArmorSO.strength;
        
        totalDexterity = helmet.myArmorSO.dexterity + bodyArmor.myArmorSO.dexterity + gauntlets.myArmorSO.dexterity + belt.myArmorSO.dexterity + greaves.myArmorSO.dexterity + boots.myArmorSO.dexterity;
        
        totalIntelligence = helmet.myArmorSO.intelligence + bodyArmor.myArmorSO.intelligence + gauntlets.myArmorSO.intelligence + belt.myArmorSO.intelligence + greaves.myArmorSO.intelligence + boots.myArmorSO.intelligence;
        
        totalLuck = helmet.myArmorSO.luck + bodyArmor.myArmorSO.luck + gauntlets.myArmorSO.luck + belt.myArmorSO.luck + greaves.myArmorSO.luck + boots.myArmorSO.luck;
    }

    public float GetJewelryDamageValue()
    {
        return amulet.myJewelrySO.health + leftRing.myJewelrySO.health + rightRing.myJewelrySO.health;
    }
}
