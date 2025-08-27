using UnityEngine;

namespace Ravonix.Combat
{
    public enum EffectDamageType
    {
        PHYSICAL, MAGICAL, EMPOWERED
    }

    [System.Serializable]
    public class Effect_Offensive
    {
        public EffectDamageType effectDamageType;
        [Range(0, 20)] public int damage = 0;
        [Range(0, 10)] public float stunTime = 0;
        [Range(-10, 10)] public float pushForce = 0;
    }

    [System.Serializable]
    public class Effect_Utility
    {
        [Range(0, 20)] public int heal = 0;

        [Range(0, 0.5f)] public float limitlessTime = 0;
        [Range(0, 20)] public float forceForward = 0;
        [Range(0, 20)] public float forceUp = 0;
    }
}