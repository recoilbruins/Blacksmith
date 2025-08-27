using BlackSmithAnimator;
using BlacksmithCharacter;
using BlacksmithCombat;
using BlackSmithInput;
using UnityEngine;

namespace BlacksmithPlayer
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class PlayerManager : CharacterManager
    {
        
        public static PlayerManager instance;
        private AnimationManager animationManager;
        private PlayerMovement playerMovement;
        private PlayerCombat playerCombat;

        [Header("Player States")]
        public bool isAnimationLocked;
        public bool isUsingRootMotion;
        public bool isJumping;
        public bool isDodging;
        public bool isGrounded;
        public bool isPrimaryButtonPressed;
        public bool isSecondaryButtonPressed;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            DontDestroyOnLoad(instance);


            capsuleCollider = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            playerMovement = GetComponent<PlayerMovement>();
            animator = GetComponent<Animator>();
            animationManager = GetComponent<AnimationManager>();
            playerCombat = GetComponent<PlayerCombat>();
            equippedWeapons = GetComponent<EquippedWeapons>();
            animationManager.animator = animator;
        }

        private void Start()
        {
            if(equippedWeapons != null && equippedWeapons.currentWeapons.Length > 0)
            {
                if(equippedWeapons.currentWeapons.Length < 2)
                {
                    animationManager.currentAOC = equippedWeapons.currentWeapons[0].weaponSO.animatorOverrideController;
                }
                else
                {
                    animationManager.currentAOC = equippedWeapons.currentWeapons[1].weaponSO.animatorOverrideController;
                }
                animationManager.animator.runtimeAnimatorController = animationManager.currentAOC;
                if (equippedWeapons.currentWeapons.Length > 1)
                {
                    if (equippedWeapons.currentWeapons[1].weaponSO.weaponType == WeaponSO.WeaponType.SHIELD) 
                    {
                        SetupShieldAttackAnimations.myInstance.UpdateShieldAttackAnimationsToMainHandWeaponAttacks();
                    }

                }
            }
        }


        private void FixedUpdate()
        {
            playerMovement.UpdateAllMovement(moveSpeedMultiplier);
            if (playerMovement.isJumping || !playerMovement.isGrounded /*|| playerMovement.isDodging*/) { return; }
            playerCombat.PlayerCombatFunctions();
        }

        private void Update()
        {
            InputManager.instance.MovementInput();
            animationManager.UpdateAnimatorValues(0, InputManager.instance.moveAmount, InputManager.instance.isSprintPressed);
            InputManager.instance.UpdateEscapePressed();
        }

        private void LateUpdate()
        {
            playerMovement.UpdateAnimationBools();
            UpdateManagerBools();
        }

        private void UpdateManagerBools()
        {
            isAnimationLocked = animationManager.animator.GetBool("isAnimationLocked");
            isUsingRootMotion = playerMovement.isUsingRootMotion;
            isJumping = playerMovement.isJumping;
            isGrounded = playerMovement.isGrounded;
            isDodging = playerMovement.isDodging;
            isPrimaryButtonPressed = InputManager.instance.isPrimaryButtonPressed;
            isSecondaryButtonPressed = InputManager.instance.isSecondaryButtonPressed;
            /*sBlocking = InputManager.instance.isBlockPressed;
            isAttacking = inputManager.isLightAttackPressed;*/
        }
    }
}

