using Ravonix.Utility;
using UnityEngine;

namespace Ravonix.Combat
{
    public class Ability_Projectile : Ability_Detonate
    {
        [Header("PROJECTILE")]

        [Range(1, 20)] public float speed = 10;
        [Range(0, 10)] public float selfDestructTime = 5;
        [Tooltip("Projectile will not turn off after collision (will only effect first hit)")]
        public bool projectileCanRichochet;
        [Tooltip("Projectile can hit multiple targets")]
        public bool canRicochetMultiHit;

        public Effect_Offensive projectileEffect;

        [Header("PROJECTILE - DETONATION")]

        public bool detonateOnSelfDestruct;
        public bool detonateOnHit;
        public bool detonationCanHitAgain;

        [Header("PROJECTILE - CHARGE")]

        [Range(0, 20)] public float chargeProjectileSpeedIncrease = 0;
        [Range(0, 10)] public int chargeProjectileDamageIncrease = 0;
        [Range(0, 10)] public float chargeStunIncrease = 0;

        [Header("PROJECTILE - AUDIO")]

        public string impactSoundID;

        [Header("PROJECTILE - FX")]

        public bool dontUseCastFXOnBasics;
        public GameObject detonationFX;
        public GameObject projectileFX;
        public GameObject impactFX;

        [Header("PROJECTILE - REFERENCES")]

        // LOCAL VARS

        float selfDestructTimer;

        // Update is called once per frame
        void Update()
        {
            if (active)
            {
                if (!detonated)
                {
                    if ((selfDestructTimer -= Time.deltaTime) <= 0)
                    {
                        if (detonateOnSelfDestruct)
                        {
                            ProjectileDetonate();
                        }
                        else
                        {
                            //Debug.Log("DESTROY PROJECTILE : " + name);
                            Destroy(gameObject);
                        }
                    }
                }

                if (shouldDestroy)
                {
                    if ((autoDestroyTimer -= Time.deltaTime) <= 0)
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }

        public override void OnCast(Lifeform caster = null)
        {
            OnCastCore(caster);

            projectileFX.SetActive(true);

            // PUSH FORWARD
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Extrapolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rb.AddForce(LaunchDirection());

            selfDestructTimer = selfDestructTime;
        }

        internal override void ApplyChargeBuff()
        {
            base.ApplyChargeBuff();

            projectileEffect.stunTime += chargedRatio * chargeStunIncrease;
            projectileEffect.damage += (int)(chargedRatio * chargeProjectileDamageIncrease);
            speed += (chargedRatio * chargeProjectileSpeedIncrease);
        }

        void ProjectileDetonate()
        {
            // TURN OFF PROJECTILE FX
            if (projectileFX != null) projectileFX.SetActive(false);

            // VFX & SOUND
            if (detonationFX != null)
            {
                GameObject castGO = Instantiate(detonationFX, transform.position, transform.rotation);
                castGO.transform.rotation = transform.rotation;
                castGO.SetActive(true);
                castGO.AddComponent<ParticleDestroyer>();
            }

            Detonate(detonationCanHitAgain);

            //Debug.Log("DETONATE PROJECTILE : " + name);
        }

        void Impact(Vector3 collisionLocation)
        {
            // CREATE IMPACT EFFECT
            if (impactFX != null)
            {
                GameObject impactGO = Instantiate(impactFX, collisionLocation, transform.rotation);
                impactGO.transform.rotation = transform.rotation;
                impactGO.SetActive(true);
                impactGO.AddComponent<ParticleDestroyer>();

                if (impactSoundID != "") { };
                    //MasterAudio.PlaySound3DAtVector3AndForget(impactSoundID, transform.position);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!active) return;

            // IF NOT COLLIDING WITH LAYER MASK
            if (((1 << collision.gameObject.layer) & targetMask) == 0)
            {
                return;
            }

            if (!canRicochetMultiHit)
            {
                // IF YOU CANT HIT MORE THAN ONE AND ALREADY HIT SOMETHING
                if (lifeformsHit.Count > 0)
                {
                    return;
                }
            }

            Lifeform target;

            // DOES IT HIT LIFEFORM
            if ((target = collision.gameObject.GetComponent<Lifeform>()) != null)
            {
                if (target == caster)
                {
                    //Debug.LogWarning("HIT ITSELF");
                    return;
                }

                // IF HASNT HIT YET
                if (!lifeformsHit.Contains(target))
                {
                    lifeformsHit.Add(target);
                    CombatSystem.TryEffectLifeform(caster, projectileEffect, target);
                }
            }

            Impact(collision.GetContact(0).point);

            if (detonateOnHit)
            {
                ProjectileDetonate();
            }
            else if (!projectileCanRichochet)
            {
                //Debug.Log(name + " COLLISION WITH : " + collision.gameObject.name);
                //Debug.Log("DESTROY PROJECTILE : " + name);
                Destroy(gameObject);
            }
        }

        public Vector3 LaunchDirection()
        {
            return (transform.forward * speed * PROJECTILE_SPEED_MULTIPLIER);
        }
    }
}