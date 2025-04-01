using Ravonix.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Ravonix.WorldGen
{
    [RequireComponent(typeof(Collider))]
    public class Enchantment : MonoBehaviour
    {
        // INSPECTABLE VARS

        [Range(0,30)] public int ticksPerSecond = 5;

        public LayerMask detectionLayers;

        //public float effectRadius = 5.0f; // Radius of the enchantment's effect.
        public float upwardForce = 0f; // Force applied to push the player upwards.

        // LOCAL VARS
        List<Lifeform> affectedLifeforms;

        float timer;

        private void OnEnable()
        {
            affectedLifeforms = new List<Lifeform>();
        }

        private void Update()
        {
            if (affectedLifeforms.Count > 0)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    foreach(Lifeform lf in affectedLifeforms)
                    {
                        LifeformTick(lf);
                    }

                    ResetTimer();
                }
            }
        }

        // BURST ENTER
        private void OnTriggerEnter(Collider other)
        {
            if (Utilities.IsInLayerMask(detectionLayers, other) &&
                !other.isTrigger)
            {
                Lifeform target = Utilities.GetScriptOnObject<Lifeform>(other);

                // IF WASNT BEING AFFECETD
                if (!affectedLifeforms.Contains(target))
                {
                    LifeformEnter(target);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Utilities.IsInLayerMask(detectionLayers, other) &&
                !other.isTrigger)
            {
                Lifeform target = Utilities.GetScriptOnObject<Lifeform>(other);

                // IF WAS BEING AFFECTED
                if (affectedLifeforms.Contains(target))
                {
                    LifeformExit(target);
                }
            }
        }

        void LifeformTick(Lifeform lifeform)
        {
            DoRigidbodyEffect(lifeform);
        }

        void LifeformEnter(Lifeform lifeform)
        {
            ResetTimer();
            affectedLifeforms.Add(lifeform);

            DoRigidbodyEffect(lifeform);
        }

        void LifeformExit(Lifeform lifeform)
        {
            affectedLifeforms.Remove(lifeform);

            DoRigidbodyEffect(lifeform);

            //MasterAudio.PlaySound3DAtVector3AndForget("WindExit", lifeform.transform.position);
        }

        void DoRigidbodyEffect(Lifeform lifeform)
        {
            if (upwardForce > 0)
            {
                Rigidbody rb = Utilities.GetScriptOnObject<Rigidbody>(lifeform);
                if (rb != null)
                {
                    TriggerUpwardForce(rb);
                }
            }
        }

        void TriggerUpwardForce(Rigidbody rb)
        {
            rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
        }

        void ResetTimer()
        {
            timer = 1f / (float)ticksPerSecond;
        }
    }
}