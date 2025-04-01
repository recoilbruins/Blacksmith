using BlackSmithAnimator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupShieldAttackAnimations : MonoBehaviour
{
    [SerializeField] private CurrentEquippedWeapons currentEquippedWeapons;
    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private string[] animationNames;

    public static SetupShieldAttackAnimations myInstance;

    private Weapon mainHandWeapon;
    private Weapon offHandWeapon;

    private AnimationClip currentAnimationClip;

    private void Awake()
    {
        if(myInstance == null)
        {
            myInstance = this;
        }
    }

    private void GetWeapons()
    {
        if (currentEquippedWeapons.currentWeapons[0] != null && currentEquippedWeapons.currentWeapons[1] != null)
        {
            mainHandWeapon = currentEquippedWeapons.currentWeapons[0];
            offHandWeapon = currentEquippedWeapons.currentWeapons[1];
        }
    }

    public void UpdateShieldAttackAnimationsToMainHandWeaponAttacks()
    {
        GetWeapons();
        ChangeAnimationClip(animationNames, offHandWeapon.weaponSO.animatorOverrideController, mainHandWeapon.weaponSO.animatorOverrideController);
    }

    // This is for replacing the animation in the weapons animator override
    private void ChangeAnimationClip(string[] originalAnimationName, AnimatorOverrideController shieldAnimatorOverrideController, AnimatorOverrideController mainHandWeaponAOC)
    {
        // Create a new dictionary to store the overrides
        var shieldOverrideController = shieldAnimatorOverrideController;
        var mainOverrideController = mainHandWeaponAOC;

        var shieldOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(shieldAnimatorOverrideController.overridesCount);
        var mainOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(mainHandWeaponAOC.overridesCount);

        shieldOverrideController.GetOverrides(shieldOverrides);
        mainOverrideController.GetOverrides(mainOverrides);

        // Find the index of the animation clip by name
        for (int i = 0; i < mainOverrides.Count; i++)
        {
            for(int count = 0; count < originalAnimationName.Length; count++)
            {
                // check if its the attack animation clip 
                if (mainOverrides[i].Key.name == originalAnimationName[count])
                {
                    currentAnimationClip = mainOverrides[i].Value;
                    // Replace he empty animation clip with the main hand attack animation
                    shieldOverrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(mainOverrides[i].Key, currentAnimationClip);
                    break;
                }
            }
        }

        // Apply the modified overrides back to the controller
        shieldOverrideController.ApplyOverrides(shieldOverrides);
        animationManager.animator.runtimeAnimatorController = shieldOverrideController;
    }
}
