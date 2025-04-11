using BlackSmithInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapWeapons : MonoBehaviour
{
    [SerializeField] private EquippedWeapons equippedWeapons;
    [SerializeField] private CharacterModelWeaponController characterModelWeaponController;
    public Transform mainHandTransform;
    public Transform offHandTransform;

    private int weaponVal = 0;
    private bool timeToSwapWeapons = false;

    private void Start()
    {
        timeToSwapWeapons = InputManager.instance.isSwapWeaponPressed;
    }

    private void Update()
    {
        
    }

    public void SwapMainWeapon(Weapon newWeapon)
    {
        //Sheath Current Weapon if it isn't already and then unsheath new one

        if (newWeapon.weaponSO.weaponType == WeaponSO.WeaponType.TWOHANDWEAPON && equippedWeapons.currentWeapons.Length > 1)
        {
            characterModelWeaponController.
            //Destroy(equippedWeapons.currentWeapons[1]);
        }
        Destroy(equippedWeapons.currentWeapons[0]);
        equippedWeapons.currentWeapons[0] = newWeapon;
        Instantiate(equippedWeapons.currentWeapons[0], mainHandTransform);
    }

    public void SwapOffHandWeapon(Weapon newWeapon)
    {
        // Sheath Current off hand Weapon and unsheath new one
        Destroy(equippedWeapons.currentWeapons[1]);
        equippedWeapons.currentWeapons[1] = newWeapon;
        Instantiate(equippedWeapons.currentWeapons[1], offHandTransform);
    }
}
