using BlackSmithAnimator;
using BlackSmithInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlacksmithCombat
{
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Current Equipped Weapon")]
        [SerializeField] EquippedWeapons equippedWeapons;

        [SerializeField] private HandActionController handActionController;

        [Header("Combat State")]
        public bool isAttacking = false;
        public bool isBlocking = false;

        [Header("Weapon Combo")]
        [SerializeField] private int leftComboCounter = 0;
        [SerializeField] private int rightComboCounter = 0;
        [SerializeField] private float comboResetTime = 1.25f;
        [SerializeField] private float comboTimer = 0f;
/*        [SerializeField] private float rightComboTimer = 0f;*/

        [Header("Input Buffering")]
        private bool bufferedPrimaryAttack = false;
        private bool bufferedSecondaryAttack = false;
        private float bufferWindow = 0.5f; // Time in seconds to buffer the input
        private float rightBufferTimer = 0f;
        private float leftBufferTimer = 0f;

        [Header("Attack Failure")]
        private float attackTimeout = 1.0f; // Time in seconds to wait before allowing another attack
        private float attackTimer = 0f;

        [Header("Script References")]
        AnimationManager animationManager;
        PlayerMovement playerMovement;

        [Header("Strings")]
        //private static string PRIMARY_ATTACK = "primaryAttack";
        //private static string SECONDARY_ATTACK = "secondaryAttack";
        private static string BLOCKING_ATTACK = "isBlocking";

        private static string BLOCKING_ANIMATION_NAME = "Blocking";
        private static string SHIELD_BLOCKING_ANIMATION_NAME = "ShieldBlockingStateMachine";

        private static int ATTACKANIMATIONLAYER = 2;
        private static int BLOCKANIMATIONLAYER = 1;

        private Weapon rightHandWeapon;
        private Weapon leftHandWeapon;

        //AnimationClipOverrides clipOverrides;
        private void Awake()
        {
            animationManager = GetComponent<AnimationManager>();
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            if (equippedWeapons.currentWeapons.Length > 0)
            {
                rightHandWeapon = equippedWeapons.currentWeapons[0];
            }
            if (equippedWeapons.currentWeapons.Length > 1)
            {
                leftHandWeapon = equippedWeapons.currentWeapons[1];
            }
            
        }

        private void OnEnable()
        {
            EventSubscriptions();
        }
        private void OnDisable()
        {
            EventUnsubscription();
        }

        private void Update()
        {
            UpdateComboTimers();
            HandleInputBuffer();
            AttackFailureTimer();
        }

        private void EventSubscriptions()
        {
            EventManager.Instance.OnRightHandAttack += handActionController.PrimaryHandPress;
            EventManager.Instance.OnRightHandAttack += PrimaryActionInput;
            EventManager.Instance.OnLeftHandAttack += handActionController.SecondaryHandPress;
            EventManager.Instance.OnLeftHandAttack += SecondaryActionInput;
        }

        private void EventUnsubscription()
        {
            EventManager.Instance.OnRightHandAttack -= handActionController.PrimaryHandPress;
            EventManager.Instance.OnRightHandAttack -= PrimaryActionInput;
            EventManager.Instance.OnLeftHandAttack -= handActionController.SecondaryHandPress;
            EventManager.Instance.OnLeftHandAttack -= SecondaryActionInput;
        }


        public void CombatInputController()
        {
            
            // return if player is in the air or jumping
            if (playerMovement.isJumping || !playerMovement.isGrounded /*|| playerMovement.isDodging*/) { return; }

            if(InputManager.instance.isPrimaryButtonPressed)
            {
                EventManager.Instance.TriggerRightHandAttack();
            }
            else
            {
                handActionController.PrimaryHandRelease();
            }


            if (InputManager.instance.isSecondaryButtonPressed)
            {
                EventManager.Instance.TriggerLeftHandAttack();
            }
            else
            {
                StopBlocking();
                handActionController.SecondaryHandRelease();
            }
        }

        private void PrimaryActionInput()
        {
            //isAttacking = true;
            if(Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }


            if(handActionController.rightHandAttack)
            {
                LightAttack(rightHandWeapon, isRightHand: true);
                // light Attack right hand
            }
            else if (handActionController.castRightHand)
            {
                // light cast right hand
            }
            else if(handActionController.twoHandedAttack)
            { 
                // light two handed attack
            }
            
        }

        private void SecondaryActionInput()
        {
            //if(isAttacking) { return; }
            
            if(handActionController.leftHandAttack)
            {
                if(leftHandWeapon != null)
                {
                    LightAttack(leftHandWeapon, isRightHand: false);
                }
            }
            else if(handActionController.castLeftHand)
            {

            }
            else if(handActionController.isBlocking)
            {
                // Blocking Code

                Block();
            }

        }

        

        private void Block()
        {
            if (BlockingAnimationIsCurrentlyPlaying()) { return; }
            isBlocking = true;
            animationManager.animator.SetBool(BLOCKING_ATTACK, true);
            Debug.Log("Block");
        }

        private void StopBlocking()
        {
            if(isBlocking)
            {
                isBlocking = false;
                animationManager.animator.SetBool(BLOCKING_ATTACK, false);
            }
        }

        public void LightAttack(Weapon weapon, bool isRightHand)
        {
            if (isRightHand) InputManager.instance.isPrimaryButtonPressed = false;
            else InputManager.instance.isSecondaryButtonPressed = false;


            if (isAttacking)
            {
                BufferAttack(isRightHand);
                return;
            }

            if (weapon == null || weapon.weaponData == null) return;

            int comboIndex = isRightHand ? rightComboCounter : leftComboCounter;
            AttackData attackData = weapon.GetLightAttack(isRightHand, comboIndex);
            if (attackData == null || string.IsNullOrEmpty(attackData.animationTriggerName)) return;

            animationManager.PlayAttackAnimations(attackData.animationTriggerName, comboIndex, isRightHand);

            isAttacking = true;
            attackTimer = attackTimeout;

            if (isRightHand)
            {
                rightComboCounter = (rightComboCounter + 1) % weapon.weaponData.rightHandLightAttackCombo.Count;
                comboTimer = comboResetTime;
            }
            else
            {
                leftComboCounter = (leftComboCounter + 1) % weapon.weaponData.leftHandLightAttackCombo.Count;
                comboTimer = comboResetTime;
            }
        }


        private bool BlockingAnimationIsCurrentlyPlaying()
        {
            if(animationManager.animator.GetCurrentAnimatorStateInfo(BLOCKANIMATIONLAYER).IsName(BLOCKING_ANIMATION_NAME) ||
                animationManager.animator.GetCurrentAnimatorStateInfo(BLOCKANIMATIONLAYER).IsName(SHIELD_BLOCKING_ANIMATION_NAME))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void UpdateComboTimers()
        {
            if(comboTimer > 0f)
            {
                comboTimer -= Time.deltaTime;
                if (comboTimer <= 0f) EndCurrentCombo();
            }
        }

        private void BufferAttack(bool isRightHand)
        {
            if (isRightHand)
            {
                bufferedPrimaryAttack = true;
                rightBufferTimer = bufferWindow;
            }
            else
            {
                bufferedSecondaryAttack = true;
                leftBufferTimer = bufferWindow;
            }
        }

        private void HandleInputBuffer()
        {
            if (bufferedPrimaryAttack)
            {
                rightBufferTimer -= Time.deltaTime;
                if (rightBufferTimer <= 0f)
                {
                    bufferedPrimaryAttack = false;
                    LightAttack(rightHandWeapon, isRightHand: true);
                }
            }
            if (bufferedSecondaryAttack)
            {
                leftBufferTimer -= Time.deltaTime;
                if (leftBufferTimer <= 0f)
                {
                    bufferedSecondaryAttack = false;
                    LightAttack(leftHandWeapon, isRightHand: false);
                }
            }
        }

        private void AttackFailureTimer()
        {
            if (isAttacking)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0f)
                {
                    Debug.LogWarning("Attack timeout hit, resetting state.");
                    ResetAttacking();
                }
            }
        }


        public void ResetAttacking()
        {
            isAttacking = false;
        }

        private void EndCurrentCombo()
        {
            rightComboCounter = 0;
            leftComboCounter = 0;
            animationManager.animator.SetInteger("rightComboCounter", rightComboCounter);
            animationManager.animator.SetInteger("leftComboCounter", leftComboCounter);
        }

        public void AttackStart(int handVal)
        {
            if(handVal == 0 && rightHandWeapon != null)
            {
                rightHandWeapon.EnableWeaponCollider();
            }
            else if(handVal == 1 && leftHandWeapon != null)
            {
                leftHandWeapon.EnableWeaponCollider();
            }
        }

        public void AttackEnd(int handVal)
        {
            if (handVal == 0 && rightHandWeapon != null)
            {
                rightHandWeapon.DisableWeaponCollider();
            }
            else if (handVal == 1 && leftHandWeapon != null)
            {
                leftHandWeapon.DisableWeaponCollider();
            }
        }
    }
}

