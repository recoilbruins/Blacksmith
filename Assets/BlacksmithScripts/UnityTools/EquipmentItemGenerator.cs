// Editor/EquipmentItemGenerator.cs
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EquipmentItemGenerator : EditorWindow
{
    private string sourceFolder = "Assets/PolygonFantasyHeroCharacters/Prefabs/Characters_ModularParts_Static";
    private string outputFolder = "Assets/ModularArmor/Items";

    [MenuItem("Tools/RPG/Create Equipment Items From Models")]
    public static void ShowWindow()
    {
        GetWindow<EquipmentItemGenerator>("Equipment Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Generate EquipmentItem Assets", EditorStyles.boldLabel);

        sourceFolder = EditorGUILayout.TextField("Source Folder", sourceFolder);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);

        if (GUILayout.Button("Generate Items"))
        {
            GenerateEquipmentItems();
        }
    }

    private void GenerateEquipmentItems()
    {
        if (!AssetDatabase.IsValidFolder(sourceFolder))
        {
            Debug.LogError("Invalid source folder.");
            return;
        }

        if (!AssetDatabase.IsValidFolder(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
            AssetDatabase.Refresh();
        }

        string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { sourceFolder });
        Dictionary<string, VisualEquipableItemData> createdItems = new();

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            string modelName = model.name.ToLower();

            string itemKey = GetItemKey(modelName);
            if (string.IsNullOrEmpty(itemKey)) continue;

            if (!createdItems.TryGetValue(itemKey, out var item))
            {
                item = ScriptableObject.CreateInstance<VisualEquipableItemData>();
                item.itemName = itemKey;
                item.equipSlot = GetPartType(itemKey);

                string assetPath = Path.Combine(outputFolder, itemKey + ".asset");
                AssetDatabase.CreateAsset(item, assetPath);
                createdItems[itemKey] = item;
            }

            if (modelName.Contains("male")) item.maleModel = model;
            else if (modelName.Contains("female")) item.femaleModel = model;
            else if (modelName.Contains("uni") || modelName.Contains("all")) item.allGenderModel = model;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Generated {createdItems.Count} EquipmentItem assets.");
    }

    private string GetItemKey(string name)
    {
        // Extract logical name (e.g. Chest_Male => "Chest")
        string key = name.ToLower();
        key = key.Replace("_male", "").Replace("_female", "").Replace("_unisex", "").Replace("_uni", "").Replace("_all", "");
        key = key.Trim();
        return string.IsNullOrWhiteSpace(key) ? null : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(key);
    }

    private EquipSlot GetPartType(string name)
    {
        if (name.Contains("chest")) return EquipSlot.Chest;
        if (name.Contains("head") || name.Contains("helmet")) return EquipSlot.Head;
        if (name.Contains("leg")) return EquipSlot.Legs;
        if (name.Contains("boot")) return EquipSlot.Boots;
        if (name.Contains("arm")) return EquipSlot.Arms;
        if (name.Contains("shield")) return EquipSlot.Shield;
        if (name.Contains("weapon") || name.Contains("sword") || name.Contains("axe")) return EquipSlot.Weapon;

        return EquipSlot.Chest; // default fallback
    }
}