using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandActionController : MonoBehaviour
{
    [SerializeField] private EquippedWeapons currentEquippedWeapons;
    [field:SerializeField] public bool isBlocking { get; set; } = false;
    [field:SerializeField] public bool leftHandAttack { get; set; } = false;
    [field:SerializeField] public bool rightHandAttack { get; set; } = false;
    [field:SerializeField] public bool castLeftHand { get; set; } = false;
    [field:SerializeField] public bool castRightHand { get; set; } = false;
    [field:SerializeField] public bool twoHandedAttack { get; set; } = false;
    public void PrimaryHandPress()
    {
        ActivateHandAction(true);
    }
    public void PrimaryHandRelease()
    {
        DeactivateHandAction();
    }
    public void SecondaryHandPress()
    {
        ActivateHandAction(false);
    }
    public void SecondaryHandRelease()
    {
        DeactivateHandAction();
    }

    private void ActivateHandAction(bool isRightHand)
    {
        if(isRightHand)
        {
            if (currentEquippedWeapons.currentWeapons.Length < 1) return;

            Weapon weapon = currentEquippedWeapons.currentWeapons[0];
            if (weapon == null) { Debug.LogError("No Weapon Equipped in main hand"); return; }

            switch (weapon.weaponSO.weaponType)
            {
                case WeaponSO.WeaponType.ONEHANDWEAPON:
                    rightHandAttack = true;
                    break;
                case WeaponSO.WeaponType.TWOHANDWEAPON:
                    twoHandedAttack = true;
                    break;
                case WeaponSO.WeaponType.SPELL:
                    castRightHand = true;
                    break;
            }
        }
        else
        {
            if (currentEquippedWeapons.currentWeapons.Length < 2) 
            {
                isBlocking = true;
                Debug.LogWarning("No Weapon Equipped in off hand, so block with current primary weapon");
                return;
            }
            Weapon weapon = currentEquippedWeapons.currentWeapons[1];
            switch (weapon.weaponSO.weaponType)
            {
                case WeaponSO.WeaponType.ONEHANDWEAPON:
                    leftHandAttack = true;
                    break;
                case WeaponSO.WeaponType.TWOHANDWEAPON:
                    isBlocking = true;
                    break;
                case WeaponSO.WeaponType.SPELL:
                    castLeftHand = true;
                    break;
                case WeaponSO.WeaponType.SHIELD:
                    isBlocking = true;
                    break;

            }
        }
    }

    private void DeactivateHandAction()
    {
        if(rightHandAttack)
        {
            rightHandAttack = false;
        }
        if (twoHandedAttack)
        {
            twoHandedAttack = false;
        }
        if (castRightHand)
        {
            castRightHand = false;
        }
        if(castLeftHand)
        {
            castLeftHand = false;
        }
        if(leftHandAttack)
        {
            leftHandAttack = false;
        }
        if(isBlocking)
        {
            isBlocking = false;
        }
    }

}
