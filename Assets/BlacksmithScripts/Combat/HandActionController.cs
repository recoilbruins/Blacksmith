using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandActionController : MonoBehaviour
{
    [SerializeField] private EquippedWeapons equippedWeapons;
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
            if (equippedWeapons.currentWeapons.Length < 1) return;

            Weapon weapon = equippedWeapons.currentWeapons[0];
            if (weapon == null) { Debug.LogError("No Weapon Equipped in main hand"); return; }

            switch (weapon.weaponSO.weaponType)
            {
                case WeaponSO.WeaponType.ONEHANDWEAPON:
                    rightHandAttack = true;
                    break;
                case WeaponSO.WeaponType.TWOHANDWEAPON:
                    twoHandedAttack = true;
                    break;
                case WeaponSO.WeaponType.UNARMED:
                    rightHandAttack = true;
                    break;
                case WeaponSO.WeaponType.SPELL:
                    castRightHand = true;
                    break;
            }
        }
        else
        {
            if(equippedWeapons.currentWeapons.Length < 2)
            {
                if(equippedWeapons.currentWeapons[0].weaponSO.weaponType == WeaponSO.WeaponType.UNARMED)
                {
                    leftHandAttack = true;
                }
                else
                {
                    Debug.LogWarning("No Weapon Equipped in off hand or your weapon is twohanded, so block with current primary weapon");
                    isBlocking = true;
                }
                return;
            }
            Weapon weapon = equippedWeapons.currentWeapons[1];
            switch (weapon.weaponSO.weaponType)
            {
                case WeaponSO.WeaponType.ONEHANDWEAPON:
                    leftHandAttack = true;
                    break;
                case WeaponSO.WeaponType.TWOHANDWEAPON:
                    isBlocking = true;
                    break;
                case WeaponSO.WeaponType.UNARMED:
                    Debug.Log("light Attack");
                    leftHandAttack = true;
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
