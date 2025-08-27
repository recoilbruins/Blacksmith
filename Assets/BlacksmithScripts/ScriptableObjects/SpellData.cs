using UnityEngine;

[CreateAssetMenu(fileName = "NewSpell", menuName = "Abilities/Spell")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public Sprite icon;
    public GameObject visualEffect;
    public float manaCost;
    public float castTime;
    public float cooldown;
    public float power;
    public DamageType damageType;
}
