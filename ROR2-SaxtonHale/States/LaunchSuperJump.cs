using System;
using RoR2;
using UnityEngine;
using EntityStates;
using EntityStates.Loader;

namespace ROR2_SaxtonHale.States
{
    public class LaunchSuperJump : BaseSkillState
	{
		public float punchSpeed { get; private set; }

		public float charge;

		[SerializeField]
		public float minLungeSpeed;

		[SerializeField]
		public float maxLungeSpeed;

		[SerializeField]
		public float minDuration;

		[SerializeField]
		public float maxDuration;

		public static bool disableAirControlUntilCollision;

		public static float speedCoefficientOnExit;

		public static float velocityDamageCoefficient;

		protected Vector3 punchVelocity;

		public override void OnEnter()
		{
			base.OnEnter();
			if (base.isAuthority)
			{
				base.characterMotor.Motor.ForceUnground();
				base.characterMotor.disableAirControlUntilCollision |= LaunchSuperJump.disableAirControlUntilCollision;
				this.punchVelocity = LaunchSuperJump.CalculateLungeVelocity(base.characterMotor.velocity, base.GetAimRay().direction, this.charge, this.minLungeSpeed, this.maxLungeSpeed);
				base.characterMotor.velocity = this.punchVelocity;
				base.characterDirection.forward = base.characterMotor.velocity.normalized;
				this.punchSpeed = base.characterMotor.velocity.magnitude;
				this.characterBody.AddTimedBuff(Modules.Buffs.weighdownBuff, 1f);
			}
		}

		public float CalcDuration()
		{
			return Mathf.Lerp(this.minDuration, this.maxDuration, this.charge);
		}

		public void PlayAnimation()
		{
			base.PlayAnimation();
			base.PlayAnimation("FullBody, Override", "ChargePunch", "ChargePunch.playbackRate", this.duration);
		}

		public override void OnExit()
		{
			base.OnExit();
			base.characterMotor.velocity *= LaunchSuperJump.speedCoefficientOnExit;
			this.outer.SetNextStateToMain();
		}

		public static Vector3 CalculateLungeVelocity(Vector3 currentVelocity, Vector3 aimDirection, float charge, float minLungeSpeed, float maxLungeSpeed)
		{
			currentVelocity = ((Vector3.Dot(currentVelocity, aimDirection) < 0f) ? Vector3.zero : Vector3.Project(currentVelocity, aimDirection));
			return currentVelocity + aimDirection * Mathf.Lerp(minLungeSpeed, maxLungeSpeed, charge);
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
