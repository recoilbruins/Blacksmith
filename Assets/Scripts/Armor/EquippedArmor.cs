using Unity.Collections;
using UnityEngine;

public class EquippedArmor : MonoBehaviour
{
    public float totalPhysicalArmorVal { get; private set; } = 0;
    public float totalMagicArmorVal { get; private set; } = 0;
    public float totalStrengthVal { get; private set; } = 0;
    public float totalDexterityVal { get; private set; } = 0;
    public float totalEnduranceVal { get; private set; } = 0;
    public float totalVitalityVal { get; private set; } = 0;
    public float totalIntelligenceVal { get; private set; } = 0;
    public float totalLuckVal { get; private set; } = 0;

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
        totalPhysicalArmorVal = helmet.myArmorSO.physicalArmorVal + bodyArmor.myArmorSO.physicalArmorVal + gauntlets.myArmorSO.physicalArmorVal + belt.myArmorSO.physicalArmorVal + greaves.myArmorSO.physicalArmorVal + boots.myArmorSO.physicalArmorVal;
        
        totalMagicArmorVal = helmet.myArmorSO.magicArmorVal + bodyArmor.myArmorSO.magicArmorVal + gauntlets.myArmorSO.magicArmorVal + belt.myArmorSO.magicArmorVal + greaves.myArmorSO.magicArmorVal + boots.myArmorSO.magicArmorVal;
        
        totalStrengthVal = helmet.myArmorSO.strength + bodyArmor.myArmorSO.strength + gauntlets.myArmorSO.strength + belt.myArmorSO.strength + greaves.myArmorSO.strength + boots.myArmorSO.strength;
        
        totalDexterityVal = helmet.myArmorSO.dexterity + bodyArmor.myArmorSO.dexterity + gauntlets.myArmorSO.dexterity + belt.myArmorSO.dexterity + greaves.myArmorSO.dexterity + boots.myArmorSO.dexterity;
        
        totalIntelligenceVal = helmet.myArmorSO.intelligence + bodyArmor.myArmorSO.intelligence + gauntlets.myArmorSO.intelligence + belt.myArmorSO.intelligence + greaves.myArmorSO.intelligence + boots.myArmorSO.intelligence;
        
        totalLuckVal = helmet.myArmorSO.luck + bodyArmor.myArmorSO.luck + gauntlets.myArmorSO.luck + belt.myArmorSO.luck + greaves.myArmorSO.luck + boots.myArmorSO.luck;
    }

    public float GetJewelryDamageValue()
    {
        return amulet.myJewelrySO.health + leftRing.myJewelrySO.health + rightRing.myJewelrySO.health;
    }
}
