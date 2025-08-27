using BlacksmithCharacter;
using System.Collections.Generic;
using UnityEngine;
public class CharacterEquipmentManager : MonoBehaviour
{
    public Transform maleRoot;
    public Transform femaleRoot;
    public Transform allGenderRoot;

    private Dictionary<EquipSlot, GameObject> equippedModels = new();

    public enum Gender { Male, Female }
    public Gender characterGender;

    [SerializeField] private CharacterManager characterStats;

    public void EquipItem(EquipableItemData item)
    {
        // Apply item effects
        item.ApplyEffects(characterStats);

        if (item is VisualEquipableItemData visualItem)
        {
            EquipVisual(visualItem);
        }
    }

    private void EquipVisual(VisualEquipableItemData item)
    {
        // Disable current model in slot
        if (equippedModels.TryGetValue(item.equipSlot, out var currentModel))
            currentModel.SetActive(false);

        // Select appropriate model
        GameObject model = characterGender switch
        {
            Gender.Male => item.maleModel ?? item.allGenderModel,
            Gender.Female => item.femaleModel ?? item.allGenderModel,
            _ => item.allGenderModel
        };

        if (model != null)
        {
            model.SetActive(true);
            equippedModels[item.equipSlot] = model;
        }
    }
}
