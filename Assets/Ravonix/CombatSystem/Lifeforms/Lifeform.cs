using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ravonix.Combat
{
    [System.Serializable]
    public class Stats
    {
        // NATURE
        [Range(1, 100)] 
        [Tooltip("Health Total")]
        public int stamina;

        // WATER
        [Range(1, 100)]
        [Tooltip("Increases Mobility")]
        public int dexterity;

        // CHAOS
        [Range(1, 100)] 
        [Tooltip("Increases Physical Damage")] 
        public int strength;

        // FIRE
        [Range(1, 100)]
        [Tooltip("Increases Magical Damage")]
        public int wisdom;

        // LIGHT
        [Range(1, 100)]
        [Tooltip("Increases Physical Resistance")]
        public int armour;

        // VOID
        [Range(1, 100)]
        [Tooltip("Increases Magical Resistance")]
        public int resistance;
    }

    public class Lifeform : MonoBehaviour
    {
        const float SHRINK_DURATION = 2.0f;
        const float SHRINK_FINAL_SCALE = 0.01f;

        public new string name;

        [Header("LIFEFORM : STATS")]

        [Range(0, 100)] public int healthMax;
        [Range(0, 100)] public int armour = 0;
        [Range(0, 100)] public int magicResistance = 0;

        int currentHealth;

        [Header("LIFEFORM : DEATH")]
        [Space(5)]

        public GameObject dieEffect;
        public List<GameObject> dieEnables;

        [Header("LIFEFORM - SOUNDS")]
        public string soundStunned;
        public string soundDamage;
        public string soundDie;

        internal bool dead = false;

        [HideInInspector] public int teamID;

        // ABSTRACT METHODS

        public virtual void DieTrigger() { }
        public virtual void UpdateTrigger() { }
        public virtual void AwakeTrigger() { }

        protected virtual void SetupTeamID() 
        {
            teamID = 0;
        }
        protected virtual void OnHit(Lifeform attacker = null) { }
        
        // ENGINE FUNCTIONS

        private void Awake()
        {
            InitVars();
            SetupTeamID();
            AwakeTrigger();
        }

        private void Update()
        {
            UpdateTrigger();
        }

        internal void InitVars()
        {
            foreach (GameObject item in dieEnables)
            {
                //Debug.Log(item.name);
                item.SetActive(false);
            }

            currentHealth = healthMax;
        }

        void Die()
        {
            dead = true;

            DieTrigger();

            DropLoot();

            // CREATE DIE EFFECT
            if (dieEffect != null)
                Instantiate(dieEffect, position: transform.position + Vector3.up, Quaternion.identity);

            if (dieEnables.Count > 0)
            {
                foreach (GameObject item in dieEnables)
                {
                    item.SetActive(true);
                }
            }
            
            StartCoroutine(ShrinkAndDeleteCoroutine());
        }

        public bool TakeDamage(int damage, Lifeform attacker = null)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Die();

                if (soundDie.Length > 0)
                {
                    //MasterAudio.PlaySound3DAtVector3AndForget(soundDie, transform.position);
                }
            }

            OnHit(attacker);

            return dead;
        }

        public void Heal(int amount)
        {
            currentHealth += amount;
            if (currentHealth > healthMax) currentHealth = healthMax;
        }

        public virtual void OnStun(float amount, Lifeform attacker = null)
        {
            if (soundStunned.Length > 0)
            {
                //MasterAudio.PlaySound3DAtVector3AndForget(soundStunned, transform.position);
            }
        }

        public virtual Vector3 GetForward()
        {
            return transform.forward;
        }

        public virtual Vector3 GetTargetPoint()
        {
            return transform.position;
        }

        public virtual Vector3 GetPredictionTargetPoint()
        {
            return GetTargetPoint();
        }

        void DropLoot()
        {
            //if (lootInfo != null)
            //{
            //    LootManager.instance.SpawnLoot(transform.position + Vector3.up, lootInfo);
            //}
            //else Debug.LogError("LOOT INFO IS NULL : " + name);
        }

        public int GetCurrentHealth()
        {
            return currentHealth;
        }

        public int GetMaxHealth()
        {
            return healthMax;
        }

        public float GetHealthRemainingRatio()
        {
            return (float)currentHealth / healthMax;
        }

        private IEnumerator ShrinkAndDeleteCoroutine()
        {
            // IF DOESNT ALREADY HAVE RB ; ADD PHYSICS
            if (!GetComponent<Rigidbody>())
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
                rb.angularDamping = 0;
            }
            
            yield return null;

            // DISABLE LOD
            LODGroup lod;
            if (lod = GetComponent<LODGroup>()) DisableLOD(lod);
            else if (lod = GetComponentInChildren<LODGroup>()) DisableLOD(lod);

            // Calculate the scale step for each frame
            float smallestScale = Mathf.Min(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            float scaleStep = (smallestScale - SHRINK_FINAL_SCALE) / SHRINK_DURATION;

            // Shrink the object gradually
            while (smallestScale > SHRINK_FINAL_SCALE)
            {
                smallestScale -= scaleStep * Time.deltaTime;
                Vector3 newScale = transform.localScale - new Vector3(scaleStep, scaleStep, scaleStep) * Time.deltaTime;
                transform.localScale = newScale;
                yield return null;
            }

            // Delete the object
            Destroy(gameObject);
        }

        void DisableLOD(LODGroup lod)
        {
            lod.enabled = false;

            for (int i = 0; i < lod.transform.childCount; i++)
            {
                lod.transform.GetChild(i).gameObject.SetActive(i == 0);
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}