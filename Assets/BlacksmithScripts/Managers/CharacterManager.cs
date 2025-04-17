using UnityEngine;

namespace BlacksmithCharacter
{
    public class CharacterManager : LifeForm
    {
        public float characterLevel { get; set; } = 1;
        public float strength { get; set; } = 1;
        public float dexterity { get; set; } = 1;
        public float endurance { get; set; } = 1;
        public float vitality { get; set; } = 1;
        public float intelligence { get; set; } = 1;
        public float luck { get; set; } = 1;

        public float moveSpeedMultiplier { get; set; } = 1;
        public float attackSpeedMultiplier { get; set; } = 1;

        public EquippedArmor equippedArmor;
        public EquippedWeapons equippedWeapons;

        public Animator animator;
        public Rigidbody rb;
        public CapsuleCollider capsuleCollider;

        public override void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }

        public override void Die()
        {
            throw new System.NotImplementedException();
        }
    }
}

