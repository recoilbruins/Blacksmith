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

        public Animator animator;
        public Rigidbody rb;
        public CapsuleCollider capsuleCollider;

    }
}

