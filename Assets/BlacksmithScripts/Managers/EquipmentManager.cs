using BlacksmithCharacter;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public CharacterManager playerStats;

    private Dictionary<EquipSlot, EquipableItemData> equippedItems = new();

    public void EquipItem(EquipableItemData item)
    {
        if (equippedItems.TryGetValue(item.equipSlot, out EquipableItemData currentItem))
        {
            currentItem.RemoveEffects(playerStats);
        }

        equippedItems[item.equipSlot] = item;
        item.ApplyEffects(playerStats);

        Debug.Log($"Equipped: {item.name}");
    }

    public void UnequipItem(EquipSlot slot)
    {
        if (equippedItems.TryGetValue(slot, out EquipableItemData item))
        {
            item.RemoveEffects(playerStats);
            equippedItems.Remove(slot);
            Debug.Log($"Unequipped: {item.name}");
        }
    }

    public EquipableItemData GetEquippedItem(EquipSlot slot)
    {
        equippedItems.TryGetValue(slot, out EquipableItemData item);
        return item;
    }
}
