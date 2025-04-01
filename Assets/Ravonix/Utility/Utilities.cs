using Ravonix.Combat;
using UnityEngine;

namespace Ravonix
{
    public static class Utilities
    {
        public static bool IsInLayerMask(LayerMask layer, int layerID)
        {
            return (layer.value & (1 << layerID)) != 0;
        }

        public static bool IsInLayerMask(LayerMask layer, Collider target)
        {
            return IsInLayerMask(layer, target.gameObject.layer);
        }

        public static bool IsInLayerMask(LayerMask layer, GameObject target)
        {
            return IsInLayerMask(layer, target.layer);
        }

        public static T GetScriptOnObject<T>(GameObject target)
        {
            T rb = target.GetComponent<T>();

            if (rb == null)
            {
                target.GetComponentInParent<T>();

                if (rb == null)
                {
                    target.GetComponentInChildren<T>();
                }
            }

            return rb;
        }

        public static T GetScriptOnObject<T>(Collider target)
        {
            return GetScriptOnObject<T>(target.gameObject);
        }

        public static T GetScriptOnObject<T>(Lifeform target)
        {
            return GetScriptOnObject<T>(target.gameObject);
        }
    }
}