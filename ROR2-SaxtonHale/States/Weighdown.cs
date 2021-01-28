using EntityStates;
using RoR2;
using UnityEngine;
using System;
using UnityEngine.Networking;
using RoR2.Projectile;
using EntityStates.Merc;

namespace ROR2_SaxtonHale.States
{
    public class Weighdown : BaseSkillState //uppercut
    {
        //private float downwardForceScale = 15f;
		public static float baseDuration;
		public static AnimationCurve yVelocityCurve;
		protected float duration;
		protected bool hasSwung = false;

		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = Uppercut.baseDuration / this.attackSpeedStat;
			if (base.characterDirection && base.inputBank)
			{
				base.characterDirection.forward = base.inputBank.aimDirection;
			}
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority)
			{
				if (!this.hasSwung)
				{
					this.hasSwung = true;
					base.characterMotor.Motor.ForceUnground();
				}
				if (this.hasSwung)
				{
					if (base.characterMotor && base.characterDirection)
					{
						Vector3 velocity = base.characterDirection.forward * this.moveSpeedStat * Mathf.Lerp(Uppercut.moveSpeedBonusCoefficient, 0f, base.age / this.duration);
						velocity.y = -Uppercut.yVelocityCurve.Evaluate(base.fixedAge / this.duration);
						base.characterMotor.velocity = velocity;
					}
				}
				else
				{
					base.fixedAge -= Time.fixedDeltaTime;
				}
				if (base.fixedAge >= this.duration)
				{
					this.outer.SetNextStateToMain();
				}
			}
		}
	}
}
