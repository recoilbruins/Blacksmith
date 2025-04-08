using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack")]
public class AttackSO : ScriptableObject
{
    public float damageMultiplier = 1f;
    public AnimationClip animation;
}
