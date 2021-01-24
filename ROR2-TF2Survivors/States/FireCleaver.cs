using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using UnityEngine;
using RoR2.Projectile;
using RoR2;

namespace ROR2_Scout.States
{
    public class FireCleaver : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            base.PlayAnimation("Gesture", "FireFMJ", "FireFMJ.playbackRate", this.duration);
            if (base.isAuthority)
            {
                ProjectileManager.instance.FireProjectile(projectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageStat * damageCoefficient, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Bleed, null, -1f);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

		public static GameObject effectPrefab;
        public static GameObject projectilePrefab = Modules.Projectiles.cleaverProjectile;
		public static float damageCoefficient;
		public static float force;
		public static float selfForce;
		public static float baseDuration = 2f;
		private float duration;
		public int bulletCountCurrent = 1;
	}
}
