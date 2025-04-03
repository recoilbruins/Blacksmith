using BlackSmithInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackSmithAnimator
{
    public class AnimationManager : MonoBehaviour
    {

        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private float dodgeDistanceMultiplier;

        public Animator animator;
        public AnimatorOverrideController currentAOC;

        private int velocityX;
        private int velocityZ;

        private bool isDodge = false;


        private void Awake()
        {
            velocityX = Animator.StringToHash("Velocity X");
            velocityZ = Animator.StringToHash("Velocity Z");
        }

        public void PlayAnimation(string animationName, bool isAnimationLocked, bool isUsingRootMotion = false, bool isDodging = false, bool isBlocking = false)
        {
            animator.SetBool("isAnimationLocked", isAnimationLocked);
            animator.SetBool("isUsingRootMotion", isUsingRootMotion);
            animator.SetBool("isDodging", isDodging);
            animator.SetBool("isBlocking", isBlocking);
            animator.CrossFade(animationName, 0.1f);
            isDodge = isDodging;
        }

        public void PlayAttackAnimations(string attackName, int comboVal, bool isAnimationLocked = true, bool isUsingRootMotion = true)
        {
            animator.SetTrigger(attackName);
            animator.SetInteger("ComboCounter", comboVal);
            animator.SetBool("isAnimationLocked", isAnimationLocked);
            animator.SetBool("isUsingRootMotion", isUsingRootMotion);

            //animator.CrossFade(attackName, 0.25f);
        } 

        private void OnAnimatorMove()
        {
            if (playerMovement.isUsingRootMotion)
            {
                playerMovement.rb.linearDamping = 0f;
                Vector3 deltaPos = animator.deltaPosition;
                deltaPos.y = 0f;
                Vector3 velocity = deltaPos / Time.deltaTime;
                playerMovement.rb.linearVelocity = isDodge ? velocity*dodgeDistanceMultiplier : velocity;
            }
        }

        public void UpdateAnimatorValues(float xMovement, float zMovement, bool isSprinting)
        {
            float setZMovement;

            #region Set Z Movement
            if (zMovement > 0 && zMovement < 0.55f)
            {
                setZMovement = 0.5f;
            }
            else if (zMovement > 0.55f)
            {
                setZMovement = 1;
            }
            else if (zMovement < 0 && zMovement > -0.55f)
            {
                setZMovement = -0.5f;
            }
            else if (zMovement < -0.55f)
            {
                setZMovement = -1;
            }
            else
            {
                setZMovement = 0;
            }
            #endregion

            if (isSprinting && zMovement > 0)
            {
                setZMovement = 2;
            }

            animator.SetFloat(velocityX, xMovement, 0.1f, Time.deltaTime);
            animator.SetFloat(velocityZ, setZMovement, 0.1f, Time.deltaTime);
        }
    }
}

