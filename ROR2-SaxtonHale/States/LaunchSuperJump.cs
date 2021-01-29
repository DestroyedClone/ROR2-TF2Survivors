using System;
using RoR2;
using UnityEngine;
using EntityStates;

namespace ROR2_SaxtonHale.States
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
	public class LaunchSuperjump : BaseSkillState
    {
        public float punchSpeed { get; private set; }
		public float charge;
		[SerializeField]
		public float minLungeSpeed;
		[SerializeField]
		public float maxLungeSpeed;
		[SerializeField]
		public float minPunchForce;
		[SerializeField]
		public float maxPunchForce;
		[SerializeField]
		public float minDuration;
		[SerializeField]
		public float maxDuration;
		public static bool disableAirControlUntilCollision;
		public static float speedCoefficientOnExit;
		protected Vector3 punchVelocity;

		public override void OnEnter()
		{
			base.OnEnter();
			if (base.isAuthority)
			{
				base.characterMotor.Motor.ForceUnground();
				base.characterMotor.disableAirControlUntilCollision |= LaunchSuperjump.disableAirControlUntilCollision;
				var punchVelocity = LaunchSuperjump.CalculateLungeVelocity(base.characterMotor.velocity, base.GetAimRay().direction, this.charge, this.minLungeSpeed, this.maxLungeSpeed);
				base.characterMotor.velocity = punchVelocity;
				base.characterDirection.forward = base.characterMotor.velocity.normalized;
				this.punchSpeed = base.characterMotor.velocity.magnitude;
			}
		}

		public float CalcDuration()
		{
			return Mathf.Lerp(this.minDuration, this.maxDuration, this.charge);
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x000EC81F File Offset: 0x000EAA1F
		public void PlayAnimation()
		{
			base.PlayAnimation("FullBody, Override", "ChargePunch", "ChargePunch.playbackRate", 3f);
		}

		public override void OnExit()
		{
			base.OnExit();
			base.characterMotor.Motor.ForceUnground();
			base.characterMotor.velocity *= LaunchSuperjump.speedCoefficientOnExit;
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x000EC9C6 File Offset: 0x000EABC6
		public static Vector3 CalculateLungeVelocity(Vector3 currentVelocity, Vector3 aimDirection, float charge, float minLungeSpeed, float maxLungeSpeed)
		{
			currentVelocity = ((Vector3.Dot(currentVelocity, aimDirection) < 0f) ? Vector3.zero : Vector3.Project(currentVelocity, aimDirection));
			return currentVelocity + aimDirection * Mathf.Lerp(minLungeSpeed, maxLungeSpeed, charge);
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x0000D742 File Offset: 0x0000B942
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}


	}
}
