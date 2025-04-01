﻿using System.Collections;
using UnityEngine;

namespace RPGCharacterAnims
{
    public class IKHands : MonoBehaviour
    {
        private Animator animator;
        public Transform leftHandObj;
        public Transform attachLeft;
        public bool canBeUsed;
		public bool isUsed;
        [Range(0, 1)] public float leftHandPositionWeight;
        [Range(0, 1)] public float leftHandRotationWeight;
        private Transform blendToTransform;
		private Coroutine co;
		private CurrentEquippedWeapons currentEquippedWeapons;

        private void Awake()
        {
            animator = GetComponent<Animator>();
			currentEquippedWeapons = GetComponent<CurrentEquippedWeapons>();
        }

		/// <summary>
		/// If there is movement and/or rotation data in the animation for the Left Hand, use IK to
		/// set the position of the Left Hand of the character.
		/// </summary>
		private void OnAnimatorIK(int layerIndex)
		{
			if (!leftHandObj) { return; }
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandPositionWeight);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotationWeight);
			if (!attachLeft) { return; }
			animator.SetIKPosition(AvatarIKGoal.LeftHand, attachLeft.position);
			animator.SetIKRotation(AvatarIKGoal.LeftHand, attachLeft.rotation);
		}

		/// <summary>
		/// Smoothly blend IK on and off so there's no snapping into position.
		/// </summary>
		public void BlendIK(bool blendOn, float delay, float timeToBlend)
		{
            // If using 2 handed weapon.
			if (currentEquippedWeapons.currentWeapons[0].weaponSO.weaponType == WeaponSO.WeaponType.TWOHANDWEAPON) {
				if (blendOn) { isUsed = true; }
			}
			if (canBeUsed & isUsed) {
				StopAllCoroutines();
				co = StartCoroutine(_BlendIK(blendOn, delay, timeToBlend, currentEquippedWeapons.currentWeapons[0]));
			}
			if (!blendOn) { isUsed = false; }
		}

		private IEnumerator _BlendIK(bool blendOn, float delay, float timeToBlend, Weapon weapon)
        {
            GetCurrentWeaponAttachPoint(weapon);
			yield return new WaitForSeconds(delay);
			var t = 0f;
			var blendTo = 0;
			var blendFrom = 0;

			if (blendOn) { blendTo = 1; }
			else { blendFrom = 1; }
			while (t < 1) {
				t += Time.deltaTime / timeToBlend;
				attachLeft = blendToTransform;
				leftHandPositionWeight = Mathf.Lerp(blendFrom, blendTo, t);
				leftHandRotationWeight = Mathf.Lerp(blendFrom, blendTo, t);
				yield return null;
			}
        }

		/// <summary>
		/// Pauses IK while character uses Left Hand during an animation.
		/// </summary>
		public void SetIKPause(float pauseTime)
		{
			if (!canBeUsed || !isUsed) { return; }
			StopAllCoroutines();
			co = StartCoroutine(_SetIKPause(pauseTime));
		}

		private IEnumerator _SetIKPause(float pauseTime)
		{
			var t = 0f;
			while (t < 1) {
				t += Time.deltaTime / 0.1f;
				leftHandPositionWeight = Mathf.Lerp(1, 0, t);
				leftHandRotationWeight = Mathf.Lerp(1, 0, t);
				yield return null;
			}
			yield return new WaitForSeconds(pauseTime - 0.2f);
			t = 0f;
			while (t < 1) {
				t += Time.deltaTime / 0.1f;
				leftHandPositionWeight = Mathf.Lerp(0, 1, t);
				leftHandRotationWeight = Mathf.Lerp(0, 1, t);
				yield return null;
			}
		}

		private void GetCurrentWeaponAttachPoint(Weapon weapon)
		{
			blendToTransform = weapon.transform.GetChild(0).transform;
		}
    }
}