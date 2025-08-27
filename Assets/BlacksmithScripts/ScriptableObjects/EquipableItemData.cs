using BlacksmithCharacter;
using UnityEngine;

public enum EquipSlot { Head, Chest, Legs, Boots, Arms, Weapon, Shield, Amulet, Ring, }

public abstract class EquipableItemData : ItemData
{
    public EquipSlot equipSlot;
    public float weight;
    public int durability;

    public abstract void ApplyEffects(CharacterManager stats);
    public abstract void RemoveEffects(CharacterManager stats);
}
