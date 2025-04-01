using UnityEngine;

namespace Ravonix.Combat
{
    public class Ability_SelfCast : Ability
    {
        [Header("SELF CAST")]
        public Effect_Utility ability;

        public bool ignorePlayerForward;

        public override void OnCast(Lifeform caster = null)
        {
            base.OnCast(caster);

            CombatSystem.TryEffectLifeform(caster, ability, caster);
        }
    }
}