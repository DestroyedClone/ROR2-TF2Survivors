using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;

namespace ROR2_Scout.States
{
    public class ActivateRage : BaseSkillState
    {
		public static float baseDuration = 1f;
		public static float buffDuration = 16f;
		public static float preShieldAnimDuration = 1f;
		private bool readyToActivate;
		private float duration;

		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = baseDuration / this.attackSpeedStat;
			base.PlayCrossfade("Gesture, Additive", "PreShield", 0.2f);
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.fixedAge >= preShieldAnimDuration && !this.readyToActivate)
			{
				this.readyToActivate = true;
				base.characterBody.AddTimedBuff(Buffs.bonkBoyBuff, buffDuration);
			}
			if (base.fixedAge >= this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		public override void OnExit()
		{
			base.OnExit();
		}
	}
}
