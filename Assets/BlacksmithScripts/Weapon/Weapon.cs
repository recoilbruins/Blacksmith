using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public WeaponSO weaponSO;

    [SerializeField] private bool isUnarmed = false;

    /*public AttackSO[] lightAttackCombo;
    public AttackSO[] heavyAttackCombo;*/

    [SerializeField] private Collider[] weaponColliders;

    [SerializeField] private WeaponCollision[] weaponCollisions;

    private bool isAttacking = false;

    // Start is called before the first frame update
    void Awake()
    {
        weaponSO.defaultDamage = weaponSO.weaponDamage;
    }

    public void EnableWeaponColliders()
    {
        foreach (Collider collider in weaponColliders)
        {
            collider.enabled = true;
        }
    }

    public void StartAttack()
    {
        EnableWeaponColliders();
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
