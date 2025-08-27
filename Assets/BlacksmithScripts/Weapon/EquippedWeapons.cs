using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
 public enum WeaponStatus { Sheathed, Unsheathed}

public class EquippedWeapons : MonoBehaviour
{
    public Weapon[] currentWeapons;

    public WeaponType weaponType;

    public WeaponStatus weaponStatus;

    public AnimatorOverrideController dualWieldingAOC;

    public float weaponDamage { get; private set; } = 0;

    public Weapon MainHandWeapon => currentWeapons.Length > 0 ? currentWeapons[0] : null;
    public Weapon OffHandWeapon => currentWeapons.Length > 1 ? currentWeapons[1] : null;


    private void Start()
    {
        WhatTypeOfWeaponsAreEquipped();
    }



    private bool IsWieldingTwoWeapons()
    {
        if (currentWeapons.Length < 1 || MainHandWeapon == null || OffHandWeapon == null) { Debug.Log("currently there is not more than 1 weapon equipped"); return false; }
         
        if (MainHandWeapon.weaponData.weaponType == WeaponType.OneHanded && OffHandWeapon.weaponData.weaponType == WeaponType.OneHanded)
        {
            return true;
        }
        
        return false;
    }

    private bool isTwoHandedWeaponEquipped()
    {
        if (WeaponListEmpty()) { Debug.LogError("currently there is no weapon equipped"); return false; }

        if (MainHandWeapon.weaponData.weaponType == WeaponType.TwoHanded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isUnarmed()
    {
        if (WeaponListEmpty()) { Debug.LogError("currently there is no weapon equipped"); return false; }

        if (MainHandWeapon.weaponData.weaponType == WeaponType.Unarmed)
        {
            return true;
        }
        return false;
    }

    public bool WeaponListEmpty()
    {
        return currentWeapons == null || currentWeapons.Length == 0 || currentWeapons.All(w => w == null);
    }

   
    private void WhatTypeOfWeaponsAreEquipped()
    {
        
        if (IsWieldingTwoWeapons())
        {
            weaponType = WeaponType.DualWield;
        }

        else if (isTwoHandedWeaponEquipped())
        {
            weaponType = WeaponType.TwoHanded;
        }
        else if (isUnarmed())
        {
            weaponType = WeaponType.Unarmed;
        }
    }

}
