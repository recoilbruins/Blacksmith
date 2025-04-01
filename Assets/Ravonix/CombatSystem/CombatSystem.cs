using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Ravonix.Combat
{
    public static class CombatSystem
    {
        public static Action OnAiTakedown;

        public static void TryEffectLifeform(Lifeform caster, Effect_Offensive effect, Lifeform target)
        {
            //Debug.Log(caster + "/" + target);

            // IF NOT ON SAME TEAM
            if (caster.teamID != target.teamID)
            {
                EffectLifeform(effect, target, caster);
            }
        }

        public static void EffectLifeform(Effect_Offensive effect, Lifeform target, Lifeform caster = null)
        {
            TryDamage(target, effect.damage, caster);

            if (effect.stunTime > 0)
                Stun(target, caster, effect.stunTime);
        }

        public static void TryEffectLifeform(Lifeform caster, Effect_Utility effect, Lifeform target)
        {
            //Debug.Log(caster + "/" + target);
            // IF ON SAME TEAM
            if (caster.teamID == target.teamID)
            {
                EffectLifeform(effect, target);
            }
        }

        public static void EffectLifeform(Effect_Utility effect, Lifeform target)
        {
            if (effect.heal > 0)
                Heal(target, effect.heal);

            if (effect.forceUp > 0 || effect.forceForward > 0)
                DoRigidbody(target, effect);
        }

        public static void TryEffectLifeforms(Lifeform caster, Effect_Offensive effect, ICollection<Lifeform> targets)
        {
            foreach (var target in targets)
            {
                TryEffectLifeform(caster, effect, target);
            }
        }

        public static void TryEffectLifeforms(Lifeform caster, Effect_Utility effect, ICollection<Lifeform> targets)
        {
            foreach (var target in targets)
            {
                TryEffectLifeform(caster, effect, target);
            }
        }

        static void DoRigidbody(Lifeform lifeform, Effect_Utility effect)
        {
            Rigidbody rb = Utilities.GetScriptOnObject<Rigidbody>(lifeform);

            if (rb != null)
            {
                if (effect.forceForward > 0)
                    rb.AddForce(lifeform.GetForward() * effect.forceForward, ForceMode.Impulse);

                if (effect.forceUp > 0)
                    rb.AddForce(Vector3.up * effect.forceUp, ForceMode.Impulse);
            }
        }

        static void TryDamage(Lifeform defender, int damage, Lifeform attacker = null)
        {
            if (defender.dead) return;

            damage = DoArmour(defender, damage);

            if (defender.TakeDamage(damage, attacker))
                OnAiTakedown?.Invoke();

            DoEffects(defender, damage);
        }

        private static void DoEffects(Lifeform defender, int damage)
        {
            if (damage > 0)
                //PopupManager.instance.CreateDamage(defender.GetTargetPoint(), damage);

            if (defender.soundDamage.Length > 0)
            {
                //MasterAudio.PlaySound3DAtVector3AndForget(defender.soundDamage, defender.GetTargetPoint());
            }
        }

        private static int DoArmour(Lifeform defender, int damage)
        {
            if (defender.armour > 0)
            {
                if ((damage -= defender.armour) < 0)
                {
                    damage = 0;
                }

                //PopupManager.instance.CreateArmourHit(defender.GetTargetPoint(), defender.armour);
            }

            return damage;
        }

        static void Stun(Lifeform target, Lifeform caster, float amount)
        {
            target.OnStun(amount, caster);
            //PopupManager.instance.CreateStun(target.GetTargetPoint());
        }

        static void Heal(Lifeform target, int amount)
        {
            target.Heal(amount);
            //PopupManager.instance.CreateHeal(target.GetTargetPoint(), amount);
        }
    }
}