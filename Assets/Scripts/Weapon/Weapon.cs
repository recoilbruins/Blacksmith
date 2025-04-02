using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public WeaponSO weaponSO;

    [SerializeField] private bool isUnarmed = false;

    /*public AttackSO[] lightAttackCombo;
    public AttackSO[] heavyAttackCombo;*/

    public int lightAttackMaxCombo;

    [SerializeField] private Collider weaponCollider;

    [SerializeField] private WeaponCollision[] weaponCollisions;

    private bool isAttacking = false;

    // Start is called before the first frame update
    void Awake()
    {
        weaponSO.defaultDamage = weaponSO.weaponDamage;
    }

    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
    }

    public void StartAttack()
    {
        EnableWeaponCollider();
        isAttacking = true;
    }

    public void EndAttack()
    {
        DisableWeaponCollider();
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;


        
    }

    public void DisableWeaponCollider()
    { 
        weaponCollider.enabled = false;
    }

    public void ResetDamage()
    {
        weaponSO.weaponDamage = weaponSO.defaultDamage;
    }
}
