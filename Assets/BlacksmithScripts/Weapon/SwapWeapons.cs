using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapWeapons : MonoBehaviour
{
    [SerializeField] EquippedWeapons currentEquippedWeapons;
    public Transform mainHandTransform;
    public Transform offHandTransform;
    public void SwapMainWeapon(Weapon newWeapon)
    {
        //Sheath Current Weapon if it isn't already and then unsheath new one

        if (newWeapon.weaponSO.weaponType == WeaponSO.WeaponType.TWOHANDWEAPON && currentEquippedWeapons.currentWeapons.Length > 1)
        {
            Destroy(currentEquippedWeapons.currentWeapons[1]);
        }
        Destroy(currentEquippedWeapons.currentWeapons[0]);
        currentEquippedWeapons.currentWeapons[0] = newWeapon;
        Instantiate(currentEquippedWeapons.currentWeapons[0], mainHandTransform);
    }

    public void SwapOffHandWeapon(Weapon newWeapon)
    {
        // Sheath Current off hand Weapon and unsheath new one
        Destroy(currentEquippedWeapons.currentWeapons[1]);
        currentEquippedWeapons.currentWeapons[1] = newWeapon;
        Instantiate(currentEquippedWeapons.currentWeapons[1], offHandTransform);
    }
}
