using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace BlacksmithCharacter
{
    public class CharacterManager : LifeForm, IDamageable
    {
        public float strength;
        public float dexterity;
        public float intelligence;
        public float endurance;
        public float vitality;
        public float luck;
        public float characterLevel;
        public float physicalDefense;
        public float magicDefense;
        public float fireResistance;

        public float moveSpeedMultiplier { get; set; } = 1;
        public float attackSpeedMultiplier { get; set; } = 1;

        public EquippedArmor equippedArmor;
        public EquippedWeapons equippedWeapons;

        public Animator animator;
        public Rigidbody rb;
        public CapsuleCollider capsuleCollider;

        private void Start()
        {
            IgnoreMyColliders();
        }

        public CharacterManager GetCharacterStats()
        {
            return this;
        }

        public override void Die()
        {
            Debug.Log($"{gameObject.name} died.");

            // Handle death logic here, such as playing an animation, disabling controls, etc.
        }

        public void TakeDamage(float amount, DamageType type, GameObject source)
        {
            currentHealth -= amount;
            Debug.Log($"{gameObject.name} took {amount} {type} damage!");

            if (currentHealth <= 0)
                Die();
        }
        protected virtual void IgnoreMyColliders()
        {

            Collider[] colliders = GetComponentsInChildren<Collider>();
            List<Collider> colliderList = new List<Collider>();

            foreach (Collider collider in colliders)
            {
                colliderList.Add(collider);
            }

            colliderList.Add(capsuleCollider);

            foreach(Collider collider in colliderList)
            {
                foreach (Collider otherCollider in colliderList)
                {
                    if (collider != otherCollider)
                    {
                        Physics.IgnoreCollision(collider, otherCollider);
                    }
                }
            }
        }
    }
}

