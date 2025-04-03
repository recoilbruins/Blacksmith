using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EquippedWeapons : MonoBehaviour
{
    public Weapon[] currentWeapons;
    public bool areWeaponsSheathed { get; private set; } = false;
    public bool isDualWield { get; private set; } = false;

    public bool isTwoHandedWeapon { get; private set; } = false;
    public AnimatorOverrideController dualWieldingAOC;

    public float weaponDamage { get; private set; } = 0;


    public void Awake()
    {
        
    }

    private void Start()
    {
        WhatTypeOfWeaponsAreEquipped();
    }



    private bool IsWieldingTwoWeapons()
    {
        if(currentWeapons.Length > 1)
        {
            if (currentWeapons[0] == null || currentWeapons[1] == null) { Debug.LogError("currently one of the two weapons equipped are null"); return false; }
            
            if (currentWeapons[0].weaponSO.weaponType == WeaponSO.WeaponType.ONEHANDWEAPON && currentWeapons[1].weaponSO.weaponType == WeaponSO.WeaponType.ONEHANDWEAPON)
            {
                return true;
            }
        }
        return false;
    }

    private bool isTwoHandedWeaponEquipped()
    {
        if (currentWeapons.Length < 1) { Debug.LogError("currently there is no weapon equipped"); return false; }
        
        if (currentWeapons[0].weaponSO.weaponType == WeaponSO.WeaponType.TWOHANDWEAPON || currentWeapons[0].weaponSO.weaponType == WeaponSO.WeaponType.UNARMED)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   
    private void WhatTypeOfWeaponsAreEquipped()
    {
        
        if (IsWieldingTwoWeapons())
        {
            isDualWield = true;
        }
        else
        {
            isDualWield = false;
        }

        if (isTwoHandedWeaponEquipped())
        {
            isTwoHandedWeapon = true;
        }
        else
        {
            isDualWield = false;
        }
    }

    private void CalculateWeaponDamage()
    {
        if (isDualWield)
        {
            weaponDamage = (currentWeapons[0].weaponSO.weaponDamage + currentWeapons[1].weaponSO.weaponDamage) / 2;
        }
        else
        {
            weaponDamage = currentWeapons[0].weaponSO.weaponDamage;
        }
    }

}
