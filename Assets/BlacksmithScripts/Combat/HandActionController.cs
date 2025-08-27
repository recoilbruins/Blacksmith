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

            switch (weapon.weaponData.weaponType)
            {
                case WeaponType.OneHanded:
                    rightHandAttack = true;
                    break;
                case WeaponType.TwoHanded:
                    twoHandedAttack = true;
                    break;
                case WeaponType.Unarmed:
                    rightHandAttack = true;
                    break;
                case WeaponType.Spell:
                    castRightHand = true;
                    break;
            }
        }
        else
        {
            if(equippedWeapons.currentWeapons.Length < 2)
            {
                if(equippedWeapons.currentWeapons[0].weaponData.weaponType == WeaponType.Unarmed)
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
            switch (weapon.weaponData.weaponType)
            {
                case WeaponType.OneHanded:
                    leftHandAttack = true;
                    break;
                case WeaponType.TwoHanded:
                    isBlocking = true;
                    break;
                case WeaponType.Unarmed:
                    Debug.Log("light Attack");
                    leftHandAttack = true;
                    break;
                case WeaponType.Spell:
                    castLeftHand = true;
                    break;
                case WeaponType.Shield:
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
