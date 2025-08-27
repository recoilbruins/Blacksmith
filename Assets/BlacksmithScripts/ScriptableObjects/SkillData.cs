using UnityEngine;

public enum SkillType { Blacksmithing, Swords, Magic, Shield, DualWield }

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public SkillType skillType;
    public Sprite icon;
    public string description;

    public int levelCap = 100;
    public AnimationCurve xpCurve; // For XP needed per level
}
