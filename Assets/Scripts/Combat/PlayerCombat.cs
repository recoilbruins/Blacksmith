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
        [SerializeField] CurrentEquippedWeapons currentEquippedWeapons;

        [SerializeField] private HandActionController handActionController;

        [Header("Combat State")]
        public bool isAttacking = false;
        public bool isBlocking = false;

        [Header("Weapon Combo")]
        [SerializeField] private int comboCounter = 0;
        [SerializeField] private float comboResetTimer = 1f;
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
            if (currentEquippedWeapons.currentWeapons.Length > 0)
            {
                mainHandWeapon = currentEquippedWeapons.currentWeapons[0];
            }
            if (currentEquippedWeapons.currentWeapons.Length > 1)
            {
                offHandWeapon = currentEquippedWeapons.currentWeapons[1];
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
            if (playerMovement.isJumping || !playerMovement.isGrounded || playerMovement.isDodging) { return; }

            if(InputManager.instance.isPrimaryButtonPressed)
            {
                EventManager.instance.Right_Hand_Attack.Invoke();
            }
            else
            {
                handActionController.PrimaryHandRelease();
            }
            if (InputManager.instance.isSecondaryButtonPressed)
            {
                EventManager.instance.Left_Hand_Attack.Invoke();
            }
            else
            {
                Debug.Log("Stop Blocking");
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
                LightAttack(mainHandWeapon);
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
                LightAttack(offHandWeapon);
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
            animationManager.animator.SetBool(BLOCKING_ATTACK, false);
            isBlocking = false;
        }
        
        private void LightAttack(Weapon weapon)
        {
            if(isBlocking) { InputManager.instance.isPrimaryButtonPressed = false; return; }

            InputManager.instance.isPrimaryButtonPressed = false;

            float previousTime = lastClickTime;
            lastClickTime = Time.time;

            if (comboCounter >= weapon.lightAttackMaxCombo || lastClickTime - previousTime > comboResetTimer)
            {
                EndCurrentCombo();
            }

            comboCounter++;
            Debug.Log("ComboCounter: " + comboCounter);

            animationManager.PlayAttackAnimations(PRIMARY_ATTACK, comboCounter);

            isAttacking = true;

            
        }

        private bool AttackAnimationIsCurrentlyPlaying()
        {
            string animationName;
            
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
            

        private void ExitLightAttack()
        {
            string animationName;

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
            }
        }


        private void EndCurrentCombo()
        {
            comboCounter = 0;
            animationManager.animator.SetInteger("AttackValue", comboCounter);
        }

    }

    

}

