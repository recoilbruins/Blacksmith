using UnityEngine;

[CreateAssetMenu(fileName = "MagicAttackData", menuName = "Scriptable Objects/MagicAttackData")]
public class MagicAttackData : ScriptableObject
{
    public string animationTriggerName;
    public float damage;
    public float manaCost;
    public float attackDuration;
    public float comboWindow;
    public AudioClip sfx;
}
