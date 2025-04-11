using UnityEngine;

public class CharacterModelWeaponController : MonoBehaviour
{
    [SerializeField] private GameObject[] mainHandWeapons;
    [SerializeField] private GameObject[] offHandWeapons;
    
    private EquippedWeapons equippedWeapons;

    private void Awake()
    {
        equippedWeapons = GetComponent<EquippedWeapons>();
    }

    public void EquipWeapon(int weaponIndex, int weaponColliderIndex, bool isMainHand = true)
    {
        if(isMainHand)
        {
            mainHandWeapons[weaponIndex].SetActive(true);
            //weaponColliders[weaponColliderIndex].SetActive(true);
        }
        else
        {
            offHandWeapons[weaponIndex].SetActive(true);
            //weaponColliders[weaponColliderIndex].SetActive(true);
        }
    }

    public void DeactivateMainHandWeapon()
    {
        foreach (var weapon in mainHandWeapons)
        {
            if (weapon.activeInHierarchy)
            {
                weapon.SetActive(false);
            }
        }
    }
}
