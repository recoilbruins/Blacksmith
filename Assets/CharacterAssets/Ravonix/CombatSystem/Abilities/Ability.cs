using Ravonix.Utility;
using System.Collections.Generic;
using UnityEngine;



namespace Ravonix.Combat
{
    public enum AbilityType
    {
        MELEE, PROJECTILE_HIT, PROJECTILE_AOE, HOLD, SELF_CAST
    }

    public abstract class Ability : MonoBehaviour
    {
        // INSPECTABLE VARS
        [Header("CORE")]

        public Sprite icon;
        public Color color;
        [Range(0, 120)] public float cooldownTime = 0;

        [Header("CORE - CHARGE")]

        [Range(0.01f, 0.5f)] public float chargeMinSize = 0.01f;
        [Range(0.1f, 1f)] public float chargeMaxSize = 0.25f;

        public bool canCharge = false;
        [Range(1, 10)] public float chargeMaxTime = 1;

        [Header("CORE - TARGETING")]

        public LayerMask targetMask;

        [Header("CORE - AUDIO")]

        public string castSoundID;

        [Header("CORE - FX")]

        public GameObject chargeFX;
        public GameObject castFX;

        // PUBLIC VARS

        [HideInInspector] public Lifeform caster;
        [HideInInspector] public float cooldownTimer;

        // CONST VARS

        internal const float AUTO_DESTROY_TIME = 1;
        internal const float PROJECTILE_SPEED_MULTIPLIER = 300;

        // LOCAL VARS

        internal bool isCharging = false;
        internal bool active = false;
        internal bool shouldDestroy = false;

        internal float autoDestroyTimer;
        float timeChargeBegin;
        internal float chargedRatio;

        internal List<Lifeform> lifeformsHit = new List<Lifeform>();

        internal new Collider collider;

        public virtual void OnChargeBegin(Lifeform caster)
        {
            this.caster = caster;

            collider = GetComponent<Collider>();
            if (collider != null) collider.enabled = false;

            if (castFX != null) castFX.SetActive(false);
            if (chargeFX != null) chargeFX.SetActive(true);

            timeChargeBegin = Time.time;

            SetChargeSize(chargeMinSize);

            isCharging = true;

            //Debug.Log("CHARGING BEGIN : " + name);
        }

        void SetChargeSize(float size)
        {
            //Debug.Log(size);
            chargeFX.transform.localScale = new Vector3(size, size, size);
        }

        public virtual void OnChargeUpdate()
        {
            chargedRatio = Mathf.Clamp01((Time.time - timeChargeBegin) / chargeMaxTime);

            SetChargeSize(Mathf.Lerp(chargeMinSize, chargeMaxSize, chargedRatio));
        }

        public virtual void OnChargeComplete()
        {
            if (collider != null) collider.enabled = true;
            if (chargeFX != null) chargeFX.SetActive(false);

            chargedRatio = Mathf.Clamp01((Time.time - timeChargeBegin) / chargeMaxTime);

            OnCast();

            isCharging = false;
            //Debug.Log("CHARGING COMPLETE : " + name);
        }

        public virtual void OnChargeCanceled()
        {
            active = true;

            if (chargeFX != null) chargeFX.SetActive(false);

            //Debug.Log("CHARGING CANCELED : " + name);
        }

        public bool Cast(Lifeform caster, Vector3 position, Quaternion rotation)
        {
            if (cooldownTimer <= 0)
            {
                // CREATE ABILITY & CAST
                Ability a = Instantiate(gameObject, position, rotation).GetComponent<Ability>();
                a.gameObject.SetActive(true);
                a.OnCast(caster);

                cooldownTimer = cooldownTime;

                return true;
            }
            else
            {
                return false;
            }
        }

        internal void OnCastCore(Lifeform caster)
        {
            // SET VARS
            active = true;

            lifeformsHit = new List<Lifeform>();

            if (caster != null) 
                this.caster = caster;

            ApplyChargeBuff();

            // VFX & SOUND
            if (castFX != null)
            {
                GameObject castGO = Instantiate(castFX, transform.position, transform.rotation);
                castGO.transform.rotation = transform.rotation;
                castGO.SetActive(true);
                castGO.AddComponent<ParticleDestroyer>();
            }
            
            //MasterAudio.PlaySound3DAtVector3AndForget(castSoundID, transform.position);

            //Debug.Log("CAST : " + name);
        }

        public virtual void OnCast(Lifeform caster = null)
        {
            OnCastCore(caster);
        }

        internal virtual void ApplyChargeBuff()
        {

        }

        public float GetCooldownRatio()
        {
            return 1f - (cooldownTimer / cooldownTime);
        }
    }
}