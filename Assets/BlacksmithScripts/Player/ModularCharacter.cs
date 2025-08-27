using UnityEngine;
using System.Collections.Generic;

public class ModularCharacter : MonoBehaviour
{
    public CharacterObjectGroups male;
    public CharacterObjectGroups female;
    public CharacterObjectListsAllGender allGender;

    public Material mat;

    [SerializeField] private CharacterConfigData characterConfig;

    [SerializeField] private List<GameObject> enabledObjects = new List<GameObject>();


    private void Start()
    {
        //BuildLists();
        ClearEnabledObjects();

        ApplyCharacterConfig(characterConfig);

    }

    public void ApplyCharacterConfig(CharacterConfigData config)
    {
        if (config == null)
        {
            Debug.LogWarning("Config is null.");
            return;
        }

        ApplyCharacterConfiguration(
            config.gender,
            config.race,
            config.skinColor,
            config.headCovering,
            config.elements,
            config.facialHair,
            config
        );
    }

    public void ApplyCharacterConfiguration(
        Gender gender,
        Race race,
        SkinColor skinColor,
        HeadCovering headCovering,
        Elements elements,
        FacialHair facialHair,
        CharacterConfigData config)
    {
        ClearEnabledObjects();
        CharacterObjectGroups cog = gender == Gender.Male ? male : female;

        ApplyHead(cog, allGender, gender, race, skinColor, headCovering, elements, facialHair, config);
        ApplyBody(cog, allGender, config);
        ApplyColors(config);
    }

    void ClearEnabledObjects()
    {
        foreach (var obj in enabledObjects)
        {
            if (obj) obj.SetActive(false);
        }
        enabledObjects.Clear();
    }

    void ActivateItem(GameObject go)
    {
        if (go)
        {
            go.SetActive(true);
            enabledObjects.Add(go);
            if (!mat && go.TryGetComponent(out SkinnedMeshRenderer smr))
                mat = smr.material;
        }
    }

    void ApplyHead(CharacterObjectGroups cog, CharacterObjectListsAllGender allGender, Gender gender, Race race,
        SkinColor skinColor, HeadCovering headCovering, Elements elements, FacialHair facialHair, CharacterConfigData config)
    {
        if (elements == Elements.Yes)
        {
            ActivateItem(GetByIndexOrDefault(cog.eyebrow, config.eyebrows));

            if (gender == Gender.Male && facialHair == FacialHair.Yes && headCovering != HeadCovering.HeadCoverings_No_FacialHair)
            {
                ActivateItem(GetByIndexOrDefault(cog.facialHair, config.facialHairIndex));
            }
            if(config.isWearingHelmet)
            {
                switch (headCovering)
                {
                    case HeadCovering.HeadCoverings_Base_Hair:
                        ActivateItem(GetByIndexOrDefault(allGender.all_Hair, config.hair));
                        ActivateItem(GetByIndexOrDefault(allGender.headCoverings_Base_Hair, config.headCoveringIndex));
                        break;
                    case HeadCovering.HeadCoverings_No_FacialHair:
                        ActivateItem(GetByIndexOrDefault(allGender.all_Hair, config.hair));
                        ActivateItem(GetByIndexOrDefault(allGender.headCoverings_No_FacialHair, config.headCoveringIndex));
                        break;
                    case HeadCovering.HeadCoverings_No_Hair:
                        ActivateItem(GetByIndexOrDefault(allGender.headCoverings_No_Hair, config.headCoveringIndex));
                        if (race != Race.Human)
                            ActivateItem(GetByIndexOrDefault(allGender.elf_Ear, config.elfEar));
                        break;
                }
            }
            else
            {
                ActivateItem(GetByIndexOrDefault(cog.headAllElements, config.head));
                ActivateItem(GetByIndexOrDefault(allGender.all_Hair, config.hair));
                if (race == Race.Elf) ActivateItem(GetByIndexOrDefault(allGender.elf_Ear, config.elfEar));
            }
        }
        else
        {
            ActivateItem(GetByIndexOrDefault(cog.headNoElements, config.head));
        }
    }

    void ApplyArms()
    {

    }

    void ApplyLegs()
    {

    }


    void ApplyBody(CharacterObjectGroups cog, CharacterObjectListsAllGender allGender, CharacterConfigData config)
    {
        ActivateItem(GetByIndexOrDefault(cog.torso, config.torso));
        ActivateItem(GetByIndexOrDefault(cog.arm_Upper_Right, config.upperArmRight));
        ActivateItem(GetByIndexOrDefault(cog.arm_Upper_Left, config.upperArmLeft));
        ActivateItem(GetByIndexOrDefault(cog.arm_Lower_Right, config.lowerArmRight));
        ActivateItem(GetByIndexOrDefault(cog.arm_Lower_Left, config.lowerArmLeft));
        ActivateItem(GetByIndexOrDefault(cog.hand_Right, config.handRight));
        ActivateItem(GetByIndexOrDefault(cog.hand_Left, config.handLeft));
        ActivateItem(GetByIndexOrDefault(cog.hips, config.hips));
        ActivateItem(GetByIndexOrDefault(cog.leg_Right, config.legRight));
        ActivateItem(GetByIndexOrDefault(cog.leg_Left, config.legLeft));

        if(config.isWearingArmor)
        {
            ActivateItem(GetByIndexOrDefault(allGender.chest_Attachment, config.chestAttachment));
            ActivateItem(GetByIndexOrDefault(allGender.back_Attachment, config.backAttachment));
            ActivateItem(GetByIndexOrDefault(allGender.shoulder_Attachment_Right, config.shoulderRight));
            ActivateItem(GetByIndexOrDefault(allGender.shoulder_Attachment_Left, config.shoulderLeft));
            ActivateItem(GetByIndexOrDefault(allGender.elbow_Attachment_Right, config.elbowRight));
            ActivateItem(GetByIndexOrDefault(allGender.elbow_Attachment_Left, config.elbowLeft));
            ActivateItem(GetByIndexOrDefault(allGender.hips_Attachment, config.hipsAttachment));
            ActivateItem(GetByIndexOrDefault(allGender.knee_Attachement_Right, config.kneeRight));
            ActivateItem(GetByIndexOrDefault(allGender.knee_Attachement_Left, config.kneeLeft));
        }
    }

    void ApplyColors(CharacterConfigData config)
    {
        if (!mat) return;

        if (config.skin.HasValue) mat.SetColor("_Color_Skin", config.skin.Value);
        if (config.hairColor.HasValue) mat.SetColor("_Color_Hair", config.hairColor.Value);
        if (config.stubble.HasValue) mat.SetColor("_Color_Stubble", config.stubble.Value);
        if (config.scar.HasValue) mat.SetColor("_Color_Scar", config.scar.Value);

        if (config.primary.HasValue) mat.SetColor("_Color_Primary", config.primary.Value);
        if (config.secondary.HasValue) mat.SetColor("_Color_Secondary", config.secondary.Value);
        if (config.metalPrimary.HasValue) mat.SetColor("_Color_Metal_Primary", config.metalPrimary.Value);
        if (config.metalSecondary.HasValue) mat.SetColor("_Color_Metal_Secondary", config.metalSecondary.Value);
        if (config.leatherPrimary.HasValue) mat.SetColor("_Color_Leather_Primary", config.leatherPrimary.Value);
        if (config.leatherSecondary.HasValue) mat.SetColor("_Color_Leather_Secondary", config.leatherSecondary.Value);
        if (config.bodyArt.HasValue) mat.SetColor("_Color_BodyArt", config.bodyArt.Value);

        mat.SetFloat("_BodyArt_Amount", config.bodyArtAmount);
    }

    GameObject GetByIndexOrDefault(List<GameObject> list, int index)
    {
        return (list != null && index >= 0 && index < list.Count) ? list[index] : null;
    }

    // build all item lists for use in randomization
    private void BuildLists()
    {
        //build out male lists
        BuildList(male.headAllElements, "Male_Head_All_Elements");
        BuildList(male.headNoElements, "Male_Head_No_Elements");
        BuildList(male.eyebrow, "Male_01_Eyebrows");
        BuildList(male.facialHair, "Male_02_FacialHair");
        BuildList(male.torso, "Male_03_Torso");
        BuildList(male.arm_Upper_Right, "Male_04_Arm_Upper_Right");
        BuildList(male.arm_Upper_Left, "Male_05_Arm_Upper_Left");
        BuildList(male.arm_Lower_Right, "Male_06_Arm_Lower_Right");
        BuildList(male.arm_Lower_Left, "Male_07_Arm_Lower_Left");
        BuildList(male.hand_Right, "Male_08_Hand_Right");
        BuildList(male.hand_Left, "Male_09_Hand_Left");
        BuildList(male.hips, "Male_10_Hips");
        BuildList(male.leg_Right, "Male_11_Leg_Right");
        BuildList(male.leg_Left, "Male_12_Leg_Left");

        //build out female lists
        BuildList(female.headAllElements, "Female_Head_All_Elements");
        BuildList(female.headNoElements, "Female_Head_No_Elements");
        BuildList(female.eyebrow, "Female_01_Eyebrows");
        BuildList(female.facialHair, "Female_02_FacialHair");
        BuildList(female.torso, "Female_03_Torso");
        BuildList(female.arm_Upper_Right, "Female_04_Arm_Upper_Right");
        BuildList(female.arm_Upper_Left, "Female_05_Arm_Upper_Left");
        BuildList(female.arm_Lower_Right, "Female_06_Arm_Lower_Right");
        BuildList(female.arm_Lower_Left, "Female_07_Arm_Lower_Left");
        BuildList(female.hand_Right, "Female_08_Hand_Right");
        BuildList(female.hand_Left, "Female_09_Hand_Left");
        BuildList(female.hips, "Female_10_Hips");
        BuildList(female.leg_Right, "Female_11_Leg_Right");
        BuildList(female.leg_Left, "Female_12_Leg_Left");

        // build out all gender lists
        BuildList(allGender.all_Hair, "All_01_Hair");
        BuildList(allGender.all_Head_Attachment, "All_02_Head_Attachment");
        BuildList(allGender.headCoverings_Base_Hair, "HeadCoverings_Base_Hair");
        BuildList(allGender.headCoverings_No_FacialHair, "HeadCoverings_No_FacialHair");
        BuildList(allGender.headCoverings_No_Hair, "HeadCoverings_No_Hair");
        BuildList(allGender.chest_Attachment, "All_03_Chest_Attachment");
        BuildList(allGender.back_Attachment, "All_04_Back_Attachment");
        BuildList(allGender.shoulder_Attachment_Right, "All_05_Shoulder_Attachment_Right");
        BuildList(allGender.shoulder_Attachment_Left, "All_06_Shoulder_Attachment_Left");
        BuildList(allGender.elbow_Attachment_Right, "All_07_Elbow_Attachment_Right");
        BuildList(allGender.elbow_Attachment_Left, "All_08_Elbow_Attachment_Left");
        BuildList(allGender.hips_Attachment, "All_09_Hips_Attachment");
        BuildList(allGender.knee_Attachement_Right, "All_10_Knee_Attachement_Right");
        BuildList(allGender.knee_Attachement_Left, "All_11_Knee_Attachement_Left");
        BuildList(allGender.elf_Ear, "Elf_Ear");
    }

    // called from the BuildLists method
    void BuildList(List<GameObject> targetList, string characterPart)
    {
        Transform[] rootTransform = gameObject.GetComponentsInChildren<Transform>();

        // declare target root transform
        Transform targetRoot = null;

        // find character parts parent object in the scene
        foreach (Transform t in rootTransform)
        {
            if (t.gameObject.name == characterPart)
            {
                targetRoot = t;
                break;
            }
        }

        // clears targeted list of all objects
        targetList.Clear();

        // cycle through all child objects of the parent object
        for (int i = 0; i < targetRoot.childCount; i++)
        {
            // get child gameobject index i
            GameObject go = targetRoot.GetChild(i).gameObject;

            // disable child object
            go.SetActive(false);

            // add object to the targeted object list
            targetList.Add(go);

            // collect the material for the random character, only if null in the inspector;
            if (!mat)
            {
                if (go.GetComponent<SkinnedMeshRenderer>())
                    mat = go.GetComponent<SkinnedMeshRenderer>().material;
            }
        }
    }
}

public enum Gender { Male, Female }
public enum Race { Human, Elf }
public enum SkinColor { White, Brown, Black, Elf }
public enum HeadCovering { HeadCoverings_Base_Hair, HeadCoverings_No_FacialHair, HeadCoverings_No_Hair }
public enum Elements { Yes, No }
public enum FacialHair { Yes, No }

[System.Serializable]
public class CharacterObjectGroups
{
    public List<GameObject> headAllElements, headNoElements, eyebrow, facialHair, torso;
    public List<GameObject> arm_Upper_Right, arm_Upper_Left, arm_Lower_Right, arm_Lower_Left;
    public List<GameObject> hand_Right, hand_Left, hips, leg_Right, leg_Left;
}

[System.Serializable]
public class CharacterObjectListsAllGender
{
    public List<GameObject> headCoverings_Base_Hair, headCoverings_No_FacialHair, headCoverings_No_Hair;
    public List<GameObject> all_Hair, all_Head_Attachment, chest_Attachment, back_Attachment;
    public List<GameObject> shoulder_Attachment_Right, shoulder_Attachment_Left;
    public List<GameObject> elbow_Attachment_Right, elbow_Attachment_Left;
    public List<GameObject> hips_Attachment, knee_Attachement_Right, knee_Attachement_Left;
    public List<GameObject> elf_Ear;
}
