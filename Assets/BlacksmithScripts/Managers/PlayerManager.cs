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
        private EquippedWeapons currentEquippedWeapons;
        private PlayerCombat playerCombat;

        [Header("Player States")]
        public bool isAnimationLocked;
        public bool isUsingRootMotion;
        public bool isJumping;
        public bool isDodging;
        public bool isGrounded;
        public bool isPrimaryButtonPressed;
        public bool isSecondaryButtonPressed;
        public bool isLockedOn;

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
            currentEquippedWeapons = GetComponent<EquippedWeapons>();
            animationManager.animator = animator;
        }

        private void Start()
        {
            if(currentEquippedWeapons != null && currentEquippedWeapons.currentWeapons.Length > 0)
            {
                if(currentEquippedWeapons.currentWeapons.Length < 2)
                {
                    animationManager.currentAOC = currentEquippedWeapons.currentWeapons[0].weaponData.animatorOverrideController;
                }
                else
                {
                    animationManager.currentAOC = currentEquippedWeapons.currentWeapons[1].weaponData.animatorOverrideController;
                }
                animationManager.animator.runtimeAnimatorController = animationManager.currentAOC;
                if (currentEquippedWeapons.currentWeapons.Length > 1)
                {
                    if (currentEquippedWeapons.currentWeapons[1].weaponData.weaponType == WeaponType.Shield) 
                    {
                        SetupShieldAttackAnimations.myInstance.UpdateShieldAttackAnimationsToMainHandWeaponAttacks();
                    }

                }
            }
        }


        private void FixedUpdate()
        {
            playerCombat.CombatInputController();
            playerMovement.UpdateAllMovement(moveSpeedMultiplier);
        }

        private void Update()
        {
            InputManager.instance.MovementInput();
            InputManager.instance.CameraInput();
            InputManager.instance.CameraTargetInput();
            InputManager.instance.UpdateEscapePressed();
            if(isLockedOn)
            {
                animationManager.UpdateAnimatorValues(InputManager.instance.horizontal, InputManager.instance.vertical, InputManager.instance.isSprintPressed, isLockedOn);
            }
            else
            {
                animationManager.UpdateAnimatorValues(0, InputManager.instance.moveAmount, InputManager.instance.isSprintPressed, isLockedOn);
            }
            //playerMovement.UpdateAllMovement(moveSpeedMultiplier);
        }

        private void LateUpdate()
        {
            playerMovement.UpdateAnimationBools();
            UpdateManagerBools();
        }

        public void UpdateManagerBools()
        {
            isAnimationLocked = animationManager.animator.GetBool("isAnimationLocked");
            isUsingRootMotion = playerMovement.isUsingRootMotion;
            isJumping = playerMovement.isJumping;
            isGrounded = playerMovement.isGrounded;
            isDodging = playerMovement.isDodging;
            isPrimaryButtonPressed = InputManager.instance.isPrimaryButtonPressed;
            isSecondaryButtonPressed = InputManager.instance.isSecondaryButtonPressed;
            isLockedOn = playerMovement.isLockedOn;
            /*sBlocking = InputManager.instance.isBlockPressed;
            isAttacking = inputManager.isLightAttackPressed;*/
        }
    }
}

