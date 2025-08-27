using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public WeaponData weaponData;

    [SerializeField] private bool isUnarmed = false;

    [SerializeField] private Collider hitCollider;

    [SerializeField] private WeaponCollision[] weaponCollisions;

   

    private bool isAttacking = false;

    // Start is called before the first frame update
    void Awake()
    {
        //weaponSO.defaultDamage = weaponSO.weaponDamage;
        DisableWeaponCollider();
    }

    

/*    public void StartAttack(int weaponIndex)
    {
        EnableWeaponColliders(weaponIndex);
        isAttacking = true;
    }

    public void EndAttack()
    {
        DisableWeaponColliders();
        isAttacking = false;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (!isAttacking) return;


        
    }
    public void EnableWeaponCollider()
    {
        hitCollider.enabled = true;
    }
    public void DisableWeaponCollider()
    {
        hitCollider.enabled = false;
    }

    public AttackData GetLightAttack(bool isRightHand, int comboIndex)
    {
        var attackData = isRightHand ? weaponData.rightHandLightAttackCombo[comboIndex % weaponData.rightHandLightAttackCombo.Count] :
                         weaponData.leftHandLightAttackCombo[comboIndex % weaponData.leftHandLightAttackCombo.Count];
        return attackData;
    }

}
