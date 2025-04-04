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
        [SerializeField] private int comboCounter = 0;
        [SerializeField] private float comboResetTimer;
        private float lastClickTime = 0f;

        [Header("Script References")]
        AnimationManager animationManager;
        PlayerMovement playerMovement;

        [Header("Strings")]
        private static string PRIMARY_ATTACK = "primaryAttack";
        private static string SECONDARY_ATTACK = "secondaryAttack";
        private static string BLOCKING_ATTACK = "isBlocking";

        private static string BLOCKING_ANIMATION_NAME = "Blocking";
        private static string SHIELD_BLOCKING_ANIMATION_NAME = "ShieldBlockingStateMachine";

        private static int ATTACKANIMATIONLAYER = 2;
        private static int BLOCKANIMATIONLAYER = 1;

        private Weapon mainHandWeapon;
        private Weapon offHandWeapon;

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
                mainHandWeapon = equippedWeapons.currentWeapons[0];
            }
            if (equippedWeapons.currentWeapons.Length > 1)
            {
                offHandWeapon = equippedWeapons.currentWeapons[1];
            }
            EventSubscriptions();
        }

        private void EventSubscriptions()
        {
            EventManager.instance.Right_Hand_Attack += handActionController.PrimaryHandPress;
            EventManager.instance.Right_Hand_Attack += PrimaryActionInput;
            EventManager.instance.Left_Hand_Attack += handActionController.SecondaryHandPress;
            EventManager.instance.Left_Hand_Attack += SecondaryActionInput;
        }

        public void PlayerCombatFunctions()
        {
            
            // return if player is in the air or jumping
            if (playerMovement.isJumping || !playerMovement.isGrounded /*|| playerMovement.isDodging*/) { return; }

            if(InputManager.instance.isPrimaryButtonPressed)
            {
                Debug.Log("Primary Button Pressed");
                EventManager.instance.Right_Hand_Attack.Invoke();
            }
            else
            {
                handActionController.PrimaryHandRelease();
            }
            if (InputManager.instance.isSecondaryButtonPressed)
            {
                Debug.Log("Secondary Button Pressed");
                EventManager.instance.Left_Hand_Attack.Invoke();
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
                Debug.Log("Time to Attack");
                LightAttack(mainHandWeapon, isRightHand: true);
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
                if(offHandWeapon != null)
                {
                    LightAttack(offHandWeapon, isRightHand: false);
                }
                else
                {
                    LightAttack(mainHandWeapon, isRightHand: false);
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
        
        private void LightAttack(Weapon weapon, bool isRightHand)
        {
            if (isRightHand)
            {
                InputManager.instance.isPrimaryButtonPressed = false;
            }
            else
            {
                InputManager.instance.isSecondaryButtonPressed = false;
            }

            if (isBlocking || isAttacking) return;

            isAttacking = true;

            float lastAttackTime = Time.time - lastClickTime;
            
            Debug.Log("lastAttackTime: " + lastAttackTime + " comboResetTimer: " + comboResetTimer);

            if (comboCounter >= weapon.weaponSO.lightAttackMaxCombo || lastAttackTime > comboResetTimer)
            {
                EndCurrentCombo();
            }


            if (isRightHand)
            {
                animationManager.PlayAttackAnimations(PRIMARY_ATTACK, comboCounter);

            }
            else
            {
                animationManager.PlayAttackAnimations(SECONDARY_ATTACK, comboCounter);
            }

            comboCounter = (comboCounter + 1) % weapon.weaponSO.lightAttackHalfCombo;

            lastClickTime = Time.time;

            Invoke(nameof(ResetAttack), weapon.weaponSO.attackSpeed);
        }
        private bool AttackAnimationIsCurrentlyPlaying()
        {
            /*string animationName;
            
            if(comboCounter == 0)
            {
                animationName = "LightAttack" + 1;
            }
            else
            {
                animationName = "LightAttack" + comboCounter;
            }


            if (animationManager.animator.GetCurrentAnimatorStateInfo(ATTACKANIMATIONLAYER).IsName(animationName) && animationManager.animator.GetCurrentAnimatorStateInfo(ATTACKANIMATIONLAYER).normalizedTime < mainHandWeapon.weaponSO.attackSpeed)
            {
                return true;
            }
            else
            {
                return false;
            }*/
            return false;
            
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
            

        private void ExitLightAttack()
        {
            /*string animationName;

            if (comboCounter == 0)
            {
                animationName = "LightAttack" + 1;
            }
            else
            {
                animationName = "LightAttack" + comboCounter;
            }

            if (animationManager.animator.GetCurrentAnimatorStateInfo(ATTACKANIMATIONLAYER).normalizedTime > mainHandWeapon.weaponSO.attackSpeed
                && animationManager.animator.GetCurrentAnimatorStateInfo(ATTACKANIMATIONLAYER).IsName(animationName))
            {
                Debug.Log("Exit Attack");
                isAttacking = false;
                Invoke("EndLightAttackCombo", 0.75f);
            }*/
        }

        void ResetAttack()
        {
            isAttacking = false;
        }


        private void EndCurrentCombo()
        {
            comboCounter = 0;
            animationManager.animator.SetInteger("ComboCounter", comboCounter);
        }

    }

    

}

