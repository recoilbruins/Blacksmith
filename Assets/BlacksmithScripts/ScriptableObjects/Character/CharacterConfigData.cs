using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterConfig", menuName = "Character/Character Configuration")]
public class CharacterConfigData : ScriptableObject
{
    public Gender gender;
    public Race race;
    public SkinColor skinColor;
    public HeadCovering headCovering;
    public Elements elements;
    public FacialHair facialHair;

    public bool isWearingHelmet;
    public bool isWearingArmor;

    public int head, eyebrows, facialHairIndex, hair, headCoveringIndex, elfEar;
    public int torso, upperArmRight, upperArmLeft, lowerArmRight, lowerArmLeft;
    public int handRight, handLeft, hips, legRight, legLeft;
    public int chestAttachment, backAttachment, shoulderRight, shoulderLeft;
    public int elbowRight, elbowLeft, hipsAttachment, kneeRight, kneeLeft;

    public Color? skin, hairColor, stubble, scar;
    public Color? primary, secondary, metalPrimary, metalSecondary;
    public Color? leatherPrimary, leatherSecondary, bodyArt;
    public float bodyArtAmount;
}