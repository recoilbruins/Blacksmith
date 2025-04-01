using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(instance);
    }

    public delegate void Weapon_Equip_Delegate();
    public Weapon_Equip_Delegate Weapon_Equip;
        
    public delegate void Weapon_Unequip_Delegate();
    public Weapon_Unequip_Delegate Weapon_Unequip;

    public delegate void Left_Hand_Attack_Delegate();
    public Left_Hand_Attack_Delegate Left_Hand_Attack;

    public delegate void Right_Hand_Attack_Delegate();
    public Right_Hand_Attack_Delegate Right_Hand_Attack;

    public delegate void Sheath_Weapon_Delegate();
    public Sheath_Weapon_Delegate sheath_Weapon;

    public delegate void Unsheath_Weapon_Delegate();
    public Weapon_Equip_Delegate unsheath_weapon;

}
