using System.Collections.Generic;
using UnityEngine;

namespace Ravonix.Combat
{
    public enum WeaponType { ONEHANDWEAPON, TWOHANDWEAPON, SHIELD, SPELL };

    [RequireComponent(typeof(Collider))]
    public class Weapon : MonoBehaviour
    {
        [Header("WEAPON")]
        public WeaponType weaponType;
        [Range(0, 11)] public int lightAttackMaxCombo = 3;
        [Range(0.1f, 5f)] public float attackSpeed = 1;
        public Effect_Offensive effect;

        [Header("TARGETING")]
        public LayerMask targetMask;

        [Header("REFERENCES")]
        public AnimatorOverrideController animatorOverrideController;

        bool attacking;
        new Collider collider;

        HashSet<Lifeform> lifeformsHit;
        Lifeform owner;

        private void Awake()
        {
            collider = GetComponent<Collider>();
            attacking = false;
        }

        public void Equip(Lifeform owner)
        {
            this.owner = owner;
        }

        public void StartAttack()
        {
            collider.enabled = true;
            attacking = true;
            lifeformsHit = new HashSet<Lifeform>();
        }

        public void EndAttack()
        {
            attacking = false;
            CombatSystem.TryEffectLifeforms(owner, effect, lifeformsHit);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!attacking) return;

            if (Utilities.IsInLayerMask(targetMask, other))
            {
                Lifeform hit = Utilities.GetScriptOnObject<Lifeform>(other);

                if (hit != null)
                {
                    lifeformsHit.Add(hit);
                }
            }
        }
    }
}