using BlackSmithInput;
using Ravonix.Combat;
using TMPro;
using UnityEngine;

namespace Ravonix.Player
{
    public class PlayerCombat : Lifeform
    {
        public static PlayerCombat instance;

        // INSPECTABLE VARS

        [Header("PLAYER : ABILITIES")]
        public Ability basicAttack, magicAttack;

        public LayerMask focusMask;

        [Header("PLAYER : REFERENCES")]
        public TextMeshProUGUI textHealth;
        public Transform abilityTransform;
        public GameObject canvasFocus;

        // LOCAL VARS

        bool hidden;
        int chargingIndex;
        bool charging;
        bool focusing;

        Lifeform focusingTarget;

        Ability abilityCharging;

        public override void AwakeTrigger()
        {
            instance = this;

            hidden = false;
            focusing = false;

           /* InputManager.instance.playerControls.PlayerActions.LightAttack.performed += playerControls => OnLightAttack();
            InputManager.instance.playerControls.PlayerActions.Block.performed += playerControls => OnMagicAttck();*/
        }

        public override void UpdateTrigger()
        {
            UpdateHealth();
            UpdateCDs();
            DoCharging();
            if (focusing) DoFocus();
        }

        void UpdateHealth()
        {
            //playerOrbHandler.UpdateFill(GetHealthRemainingRatio());
            textHealth.text = $"{GetCurrentHealth()}/{GetMaxHealth()}";
        }

        void UpdateCDs()
        {
            if (basicAttack?.cooldownTimer > 0) basicAttack.cooldownTimer -= Time.deltaTime;
            if (magicAttack?.cooldownTimer > 0) magicAttack.cooldownTimer -= Time.deltaTime;
        }

        void DoFocus()
        {
            //RaycastHit hit;

            //if (Physics.Raycast(
            //    PlayerCam.instance.castTransform.position, 
            //    PlayerCam.instance.castTransform.TransformDirection(Vector3.forward), 
            //    out hit, 
            //    30, 
            //    focusMask,
            //    QueryTriggerInteraction.Ignore))
            //{
            //    Lifeform newTarget;

            //    // AIMING AT LIFEFORM
            //    if (newTarget = hit.collider.GetComponent<Lifeform>())
            //    {
            //        // TARGET IS NEW
            //        if (focusingTarget != newTarget)
            //        {
            //            focusingTarget = newTarget;
            //            enemyHealthBar.SelectTarget(focusingTarget);
            //        }
            //    }
            //    // NOT AIMING AT LIFEFORM
            //    else
            //    {
            //        // IF WAS FOCUSING
            //        if (focusingTarget != null)
            //        {
            //            focusingTarget = null;
            //            enemyHealthBar.Deselect();
            //        }
            //    }
            //}
            //else
            //{
            //    focusingTarget = null;
            //    enemyHealthBar.Deselect();
            //}
        }

        void OnLightAttack()
        {
            CastAbility(basicAttack, abilityTransform);
        }

        void OnMagicAttck()
        {
            CastAbility(magicAttack, abilityTransform);
        }

        void DoCharging()
        {
            if (abilityCharging != null)
                abilityCharging.OnChargeUpdate();
        }

        void CastAbility(Ability ability, Transform castTransform)
        {
            // OFF COOLDOWN ?
            if (ability.cooldownTimer <= 0)
            {
                // CREATE ABILITY & CAST
                Ability a = Instantiate(
                ability.gameObject,
                castTransform.position,
                castTransform.rotation).
                    GetComponent<Ability>();

                a.OnCast(this);

                TriggerCooldown(ability);
            }
        }

        void HoldAbilityBegin(Ability ability, Transform castTransform)
        {
            // IF STILL ON COOLDOWN
            if (ability.cooldownTimer > 0) return;

            // IF WAS CHARGING
            if (abilityCharging != null)
            {
                // IF WAS CHARGING SOMETHING ELSE ; SHOULD ALWAYS HAPPEN
                if (abilityCharging.name != ability.name)
                {
                    CancelCharging();
                }
            }

            // CREATE ABILITY
            Ability a = Instantiate(
            ability.gameObject,
            castTransform.position,
            castTransform.rotation,
            castTransform).
                GetComponent<Ability>();

            a.name = ability.name;

            a.OnChargeBegin(this);

            abilityCharging = a;
        }

        void ReleaseAbility(Ability ability, Transform castTransform)
        {
            if (abilityCharging == null) return;
            
            //Debug.Log("PLAYER COMBAT SHOULD RELEASE " + ability.name);
            
            // IF RELEASED CURRENT CHARGING
            if (ability.name == abilityCharging.name)
            {
                abilityCharging.transform.parent = null;

                abilityCharging.transform.position = castTransform.position;

                abilityCharging.OnChargeComplete();

                TriggerCooldown(ability);

                abilityCharging = null;
            }
        }

        void CancelCharging()
        {
            abilityCharging.OnChargeCanceled();
            Destroy(abilityCharging.gameObject);
            abilityCharging = null;
        }

        void TriggerCooldown(Ability ability)
        {
            ability.cooldownTimer = ability.cooldownTime;
        }

        protected override void SetupTeamID()
        {
            teamID = 1;
        }

        public override void DieTrigger()
        {

        }

        protected override void OnHit(Lifeform attacker)
        {
            
        }

        public override Vector3 GetPredictionTargetPoint()
        {
            //if (PlayerMovement.instance.GetScriptOnObject<Rigidbody>() != null)
            //{
            //    return GetTargetPoint() + (PlayerMovement.instance.GetScriptOnObject<Rigidbody>().velocity / 3);
            //}
            //else
            //{
            //    return GetTargetPoint();
            //}

            return GetTargetPoint();
        }
    }
}