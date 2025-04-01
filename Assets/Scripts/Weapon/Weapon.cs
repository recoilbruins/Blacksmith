using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponSO weaponSO;

    [SerializeField] private bool isUnarmed = false;

    /*public AttackSO[] lightAttackCombo;
    public AttackSO[] heavyAttackCombo;*/

    public int lightAttackMaxCombo;

    [SerializeField] private Collider weaponCollider;
    [SerializeField] private Collider leftHandCollider;
    [SerializeField] private Collider rightHandCollider;

    [SerializeField] private WeaponCollision[] weaponCollisions;

    // Start is called before the first frame update
    void Awake()
    {
        weaponSO.defaultDamage = weaponSO.weaponDamage;
    }

    public void EnableWeaponColliders()
    {
        if(isUnarmed)
        {
            leftHandCollider.enabled = true;
            rightHandCollider.enabled = true;
        }
        else
        {
            weaponCollider.enabled = true;
        }
    }

    public void DisableWeaponColliders()
    {
        if (isUnarmed)
        {
            leftHandCollider.enabled = false;
            rightHandCollider.enabled = false;
        }
        else
        {
            weaponCollider.enabled = false;
        }
    }

    public void ResetDamage()
    {
        weaponSO.weaponDamage = weaponSO.defaultDamage;
    }
}
