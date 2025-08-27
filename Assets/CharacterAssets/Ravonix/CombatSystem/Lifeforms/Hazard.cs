using UnityEngine;

namespace Ravonix.Combat
{
    public class Hazard : MonoBehaviour
    {
        public LayerMask hitLayers;

        public Effect_Offensive hitEffect;

        void OnCollisionEnter(Collision collision)
        {
            if (Utilities.IsInLayerMask(hitLayers, collision.collider) &&
                !collision.collider.isTrigger)
            {
                Lifeform target = Utilities.GetScriptOnObject<Lifeform>(collision.collider);

                if (target != null)
                {
                    CollideWithLifeform(collision, target);
                }
            }
        }

        void CollideWithLifeform(Collision col, Lifeform lifeform)
        {
            var rb = Utilities.GetScriptOnObject<Rigidbody>(lifeform);
            
            Vector3 reflectionDirection = Vector3.Reflect(col.relativeVelocity, col.GetContact(0).normal);

            rb.linearVelocity = reflectionDirection;

            // Apply the reflection direction to the colliding object
            rb.AddForce(reflectionDirection.normalized * hitEffect.pushForce * 100);

            CombatSystem.EffectLifeform(hitEffect, lifeform);
        }
    }
}

