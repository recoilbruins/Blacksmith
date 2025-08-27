using UnityEngine;
using System;

[DefaultExecutionOrder(-10)]
public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public event Action OnWeaponEquip;
    public event Action OnWeaponUnequip;
    public event Action OnLeftHandAttack;
    public event Action OnRightHandAttack;
    public event Action OnSheathWeapon;
    public event Action OnUnsheathWeapon;

    // These can be triggered safely within this class
    public void TriggerWeaponEquip() => OnWeaponEquip?.Invoke();
    public void TriggerWeaponUnequip() => OnWeaponUnequip?.Invoke();
    public void TriggerLeftHandAttack() => OnLeftHandAttack?.Invoke();
    public void TriggerRightHandAttack() => OnRightHandAttack?.Invoke();
    public void TriggerSheathWeapon() => OnSheathWeapon?.Invoke();
    public void TriggerUnsheathWeapon() => OnUnsheathWeapon?.Invoke();
}

