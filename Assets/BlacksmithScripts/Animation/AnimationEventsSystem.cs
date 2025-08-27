using BlacksmithCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimatorMoveEvent : UnityEvent<Vector3, Quaternion> { }

public class AnimationEventsSystem : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    public void OnHitStart(int handVal)
    {
        // This method can be used to trigger any logic when the hit animation starts
        // For example, you might want to play a sound or spawn a visual effect
        playerCombat.AttackStart(handVal);
    }

    public void OnHitEnd(int handVal)
    {
        // This method can be used to trigger any logic when the hit animation ends
        // For example, you might want to reset some state or play a different sound
        playerCombat.AttackEnd(handVal);
    }

    public void EndAttack()
    {
        playerCombat.ResetAttacking();
    }

    public void ResetAttackCombos()
    {
        
    }

    public void Shoot()
    {

    }

    public void Hit()
    {

    }

    public void FootL()
    {

    }

    public void FootR()
    {

    }

    public void WeaponSwitch()
    {

    }

    public void Land()
    {

    }
}
