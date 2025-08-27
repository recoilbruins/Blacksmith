using BlacksmithCharacter;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Equippables/Visual Item")]
public class VisualEquipableItemData : EquipableItemData
{
    [Header("Visual Models")]
    public GameObject maleModel;
    public GameObject femaleModel;
    public GameObject allGenderModel;

    public override void ApplyEffects(CharacterManager stats)
    {
        // Apply stat bonuses here, if any
    }

    public override void RemoveEffects(CharacterManager stats)
    {
        // Remove stat bonuses here
    }
}
