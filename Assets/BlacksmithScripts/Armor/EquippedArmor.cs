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

    public void EquipArmorPiece(ArmorData armorData, Armor armorItem)
    {
        switch(armorData.armorType)
        {
            case ArmorType.Helmet:
                UnequipArmorPiece(helmet);
                helmet = armorItem;
                break;
            case ArmorType.Chestplate:
                UnequipArmorPiece(bodyArmor);
                bodyArmor = armorItem;
                break;
            case ArmorType.Gauntlets:
                UnequipArmorPiece(gauntlets);
                gauntlets = armorItem;
                break;
            case ArmorType.Belt:
                UnequipArmorPiece(belt);
                belt = armorItem;
                break;
            case ArmorType.Greaves:
                UnequipArmorPiece(greaves);
                greaves = armorItem;
                break;
            case ArmorType.Boots:
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
    private Armor[] GetEquippedArmor()
    {
        return new Armor[] { helmet, bodyArmor, gauntlets, belt, greaves, boots };
    }

    private void UpdateArmorValues()
    {
        totalPhysicalArmorVal = 0;
        totalMagicArmorVal = 0;
        totalStrengthVal = 0;
        totalDexterityVal = 0;
        totalEnduranceVal = 0;
        totalVitalityVal = 0;
        totalIntelligenceVal = 0;
        totalLuckVal = 0;

        foreach (var armor in GetEquippedArmor())
        {
            if (armor == null || armor.myArmorData == null) continue;

            totalPhysicalArmorVal += armor.myArmorData.physicalDefense;
            totalMagicArmorVal += armor.myArmorData.magicDefense;
            totalStrengthVal += armor.myArmorData.strength;
            totalDexterityVal += armor.myArmorData.dexterity;
            totalEnduranceVal += armor.myArmorData.endurance;
            totalVitalityVal += armor.myArmorData.vitality;
            totalIntelligenceVal += armor.myArmorData.intelligence;
            totalLuckVal += armor.myArmorData.luck;
        }
    }

    public float GetJewelryDamageValue()
    {
        return amulet.myJewelryData.bonusHealth + leftRing.myJewelryData.bonusHealth + rightRing.myJewelryData.bonusHealth;
    }
}
