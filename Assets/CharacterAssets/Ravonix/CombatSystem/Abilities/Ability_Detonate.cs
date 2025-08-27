using UnityEngine;

namespace Ravonix.Combat
{
    public class Ability_Detonate : Ability
    {
        [Header("DETONATION")]

        public float areaOfEffectInTiles = 1;
        public Effect_Offensive detonationEffect;

        [Header("DETONATION - CHARGE")]

        [Range(0, 10)] public int chargeDetonationDamageIncrease = 0;
        [Range(0, 10)] public float chargeDetonationRadiusIncrease = 0;

        [Header("DETONATION - AUDIO")]

        public string detonationSoundID;

        // LOCAL VARS

        internal bool detonated = false;

        private void Update()
        {
            // IF BLEW UP, DESTROY OBEJCT IN TIME
            if (shouldDestroy)
            {
                if ((autoDestroyTimer -= Time.deltaTime) <= 0)
                    Destroy(gameObject);
            }
        }

        public override void OnCast(Lifeform caster = null)
        {
            base.OnCast(caster);
            Detonate();
        }

        internal override void ApplyChargeBuff()
        {
            base.ApplyChargeBuff();

            detonationEffect.damage += (int)(chargedRatio * chargeDetonationDamageIncrease);
            areaOfEffectInTiles += (chargedRatio * chargeDetonationRadiusIncrease);
        }

        internal void Detonate(bool clearHitColliders = false)
        {
            shouldDestroy = true;
            detonated = true;
            autoDestroyTimer = AUTO_DESTROY_TIME;

            if (detonationSoundID != "") { };
                //MasterAudio.PlaySound3DAtVector3AndForget(detonationSoundID, transform.position);

            if (clearHitColliders)
            {
                lifeformsHit.Clear();
            }

            Lifeform target;

            // COLLISION SPHERE
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaOfEffectInTiles, targetMask);

            //Debug.Log("Targets Hit: " + hitColliders.Length);

            // HIT LIFEFORMS IN AREA
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.isTrigger) continue;

                // TARGET HIT
                if ((target = hitCollider.GetComponent<Lifeform>()) &&
                    !lifeformsHit.Contains(target))
                {
                    lifeformsHit.Add(target);
                    CombatSystem.TryEffectLifeform(caster, detonationEffect, target);
                }
                // PARENT HIT
                else if ((target = hitCollider.GetComponentInParent<Lifeform>()) &&
                    !lifeformsHit.Contains(target))
                {
                    //Debug.Log("PARENT HIT");
                    lifeformsHit.Add(target);
                    CombatSystem.TryEffectLifeform(caster, detonationEffect, target);
                }
            }
        }
    }
}