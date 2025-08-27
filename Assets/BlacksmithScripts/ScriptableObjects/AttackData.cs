using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    public string animationTriggerName;
    public float damageMultiplier;
    public float staminaCost;
    public float attackDuration;
    public float comboWindow;
    public AudioClip sfx;
}
