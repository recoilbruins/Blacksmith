using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public WeaponSO weaponSO;

    [SerializeField] private bool isUnarmed = false;

    [SerializeField] private Collider[] weaponColliders;

    [SerializeField] private WeaponCollision[] weaponCollisions;

    private bool isAttacking = false;

    // Start is called before the first frame update
    void Awake()
    {
        weaponSO.defaultDamage = weaponSO.weaponDamage;
        DisableWeaponColliders();
    }

    public void EnableWeaponColliders(int weaponIndex)
    {
       weaponColliders[weaponIndex].enabled = true;
    }

    public void StartAttack(int weaponIndex)
    {
        EnableWeaponColliders(weaponIndex);
        isAttacking = true;
    }

    public void EndAttack()
    {
        DisableWeaponColliders();
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;


        
    }

    public void DisableWeaponColliders()
    {
        foreach (Collider collider in weaponColliders)
        {
            collider.enabled = false;
        }
    }

    public void ResetDamage()
    {
        weaponSO.weaponDamage = weaponSO.defaultDamage;
    }
}
