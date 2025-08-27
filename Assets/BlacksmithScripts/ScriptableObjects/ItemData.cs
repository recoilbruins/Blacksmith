using UnityEngine;

public enum ItemType { Material, Consumable, Weapon, Armor, Tool }

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Generic Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite icon;
    public string description;
    public int maxStack = 99;
    public int sellPrice;
}