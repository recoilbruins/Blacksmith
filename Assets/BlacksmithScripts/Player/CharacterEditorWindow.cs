using UnityEngine;
using UnityEditor;

public class CharacterEditorWindow : EditorWindow
{
    private CharacterConfigData selectedConfig;
    private ModularCharacter targetCharacter;
    private Vector2 scrollPosition;

    [MenuItem("Tools/Character Editor")]
    public static void ShowWindow()
    {
        GetWindow<CharacterEditorWindow>("Character Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Character Configurator", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        targetCharacter = (ModularCharacter)EditorGUILayout.ObjectField("Target Character", targetCharacter, typeof(ModularCharacter), true);
        selectedConfig = (CharacterConfigData)EditorGUILayout.ObjectField("Character Config", selectedConfig, typeof(CharacterConfigData), false);

        if (selectedConfig == null) return;

        // Start scrollable area
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Preview & Edit Config", EditorStyles.boldLabel);
        DrawEditableConfig();

        EditorGUILayout.Space();

        // Buttons
        if (GUILayout.Button("Apply to Character"))
        {
            if (targetCharacter != null)
            {
                Undo.RecordObject(targetCharacter, "Apply Character Config");
                targetCharacter.ApplyCharacterConfig(selectedConfig);
            }
            else
            {
                Debug.LogWarning("No target character assigned.");
            }
        }

        if (GUILayout.Button("Save Changes to Asset"))
        {
            EditorUtility.SetDirty(selectedConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Character config saved.");
        }

        EditorGUILayout.EndScrollView(); // End scrollable area
    }

    private void DrawEditableConfig()
    {
        EditorGUI.BeginChangeCheck();

        selectedConfig.gender = (Gender)EditorGUILayout.EnumPopup("Gender", selectedConfig.gender);
        selectedConfig.race = (Race)EditorGUILayout.EnumPopup("Race", selectedConfig.race);
        selectedConfig.skinColor = (SkinColor)EditorGUILayout.EnumPopup("Skin Color", selectedConfig.skinColor);
        selectedConfig.headCovering = (HeadCovering)EditorGUILayout.EnumPopup("Head Covering", selectedConfig.headCovering);
        selectedConfig.elements = (Elements)EditorGUILayout.EnumPopup("Elements", selectedConfig.elements);
        selectedConfig.facialHair = (FacialHair)EditorGUILayout.EnumPopup("Facial Hair", selectedConfig.facialHair);
        selectedConfig.isWearingArmor = EditorGUILayout.Toggle("Wearing Armor", selectedConfig.isWearingArmor);
        selectedConfig.isWearingHelmet = EditorGUILayout.Toggle("Wearing Helmet", selectedConfig.isWearingHelmet);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Indexes", EditorStyles.boldLabel);
        selectedConfig.head = EditorGUILayout.IntField("Head", selectedConfig.head);
        selectedConfig.eyebrows = EditorGUILayout.IntField("Eyebrows", selectedConfig.eyebrows);
        selectedConfig.facialHairIndex = EditorGUILayout.IntField("Facial Hair Index", selectedConfig.facialHairIndex);
        selectedConfig.hair = EditorGUILayout.IntField("Hair", selectedConfig.hair);
        selectedConfig.headCoveringIndex = EditorGUILayout.IntField("Head Covering Index", selectedConfig.headCoveringIndex);
        selectedConfig.elfEar = EditorGUILayout.IntField("Elf Ear", selectedConfig.elfEar);

        selectedConfig.torso = EditorGUILayout.IntField("Torso", selectedConfig.torso);
        selectedConfig.upperArmRight = EditorGUILayout.IntField("Upper Arm Right", selectedConfig.upperArmRight);
        selectedConfig.upperArmLeft = EditorGUILayout.IntField("Upper Arm Left", selectedConfig.upperArmLeft);
        selectedConfig.lowerArmRight = EditorGUILayout.IntField("Lower Arm Right", selectedConfig.lowerArmRight);
        selectedConfig.lowerArmLeft = EditorGUILayout.IntField("Lower Arm Left", selectedConfig.lowerArmLeft);
        selectedConfig.handRight = EditorGUILayout.IntField("Hand Right", selectedConfig.handRight);
        selectedConfig.handLeft = EditorGUILayout.IntField("Hand Left", selectedConfig.handLeft);
        selectedConfig.hips = EditorGUILayout.IntField("Hips", selectedConfig.hips);
        selectedConfig.legRight = EditorGUILayout.IntField("Leg Right", selectedConfig.legRight);
        selectedConfig.legLeft = EditorGUILayout.IntField("Leg Left", selectedConfig.legLeft);

        selectedConfig.chestAttachment = EditorGUILayout.IntField("Chest Attachment", selectedConfig.chestAttachment);
        selectedConfig.backAttachment = EditorGUILayout.IntField("Back Attachment", selectedConfig.backAttachment);
        selectedConfig.shoulderRight = EditorGUILayout.IntField("Shoulder Right", selectedConfig.shoulderRight);
        selectedConfig.shoulderLeft = EditorGUILayout.IntField("Shoulder Left", selectedConfig.shoulderLeft);
        selectedConfig.elbowRight = EditorGUILayout.IntField("Elbow Right", selectedConfig.elbowRight);
        selectedConfig.elbowLeft = EditorGUILayout.IntField("Elbow Left", selectedConfig.elbowLeft);
        selectedConfig.hipsAttachment = EditorGUILayout.IntField("Hips Attachment", selectedConfig.hipsAttachment);
        selectedConfig.kneeRight = EditorGUILayout.IntField("Knee Right", selectedConfig.kneeRight);
        selectedConfig.kneeLeft = EditorGUILayout.IntField("Knee Left", selectedConfig.kneeLeft);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);

        selectedConfig.skin = DrawOptionalColor("Skin", selectedConfig.skin);
        selectedConfig.hairColor = DrawOptionalColor("Hair", selectedConfig.hairColor);
        selectedConfig.stubble = DrawOptionalColor("Stubble", selectedConfig.stubble);
        selectedConfig.scar = DrawOptionalColor("Scar", selectedConfig.scar);
        selectedConfig.primary = DrawOptionalColor("Primary", selectedConfig.primary);
        selectedConfig.secondary = DrawOptionalColor("Secondary", selectedConfig.secondary);
        selectedConfig.metalPrimary = DrawOptionalColor("Metal Primary", selectedConfig.metalPrimary);
        selectedConfig.metalSecondary = DrawOptionalColor("Metal Secondary", selectedConfig.metalSecondary);
        selectedConfig.leatherPrimary = DrawOptionalColor("Leather Primary", selectedConfig.leatherPrimary);
        selectedConfig.leatherSecondary = DrawOptionalColor("Leather Secondary", selectedConfig.leatherSecondary);
        selectedConfig.bodyArt = DrawOptionalColor("Body Art", selectedConfig.bodyArt);

        selectedConfig.bodyArtAmount = EditorGUILayout.Slider("Body Art Amount", selectedConfig.bodyArtAmount, 0f, 1f);

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(selectedConfig);
        }
    }

    private Color? DrawOptionalColor(string label, Color? current)
    {
        bool hasColor = current.HasValue;
        bool newHasColor = EditorGUILayout.ToggleLeft($"Enable {label}", hasColor);

        if (newHasColor)
        {
            Color colorValue = current ?? Color.white;
            colorValue = EditorGUILayout.ColorField(label, colorValue);
            return colorValue;
        }
        else
        {
            return null;
        }
    }
}
