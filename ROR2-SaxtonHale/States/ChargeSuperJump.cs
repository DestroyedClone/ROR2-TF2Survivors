using System;
using RoR2;
using UnityEngine;
using EntityStates;
using EntityStates.Loader;

namespace ROR2_SaxtonHale.States
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
	public class ChargeSuperjump : BaseSkillState
	{
        private float chargeDuration { get; set; }
		private float charge { get; set; }

		[SerializeField]
		public float baseChargeDuration = 1f;
		public static float minChargeForChargedAttack;

		readonly string[] jumpMessages =
		{
			"Brave jump!",
			"Up!",
			"HIGHER!",
			"Screw gravity!"
		};

		public override void OnEnter()
		{
			base.OnEnter();
			chargeDuration = baseChargeDuration;
		}

		public override void OnExit()
		{
			PlaySoundRandom();
			base.OnExit();
		}

		public void PlaySoundRandom()
		{
			var randint = UnityEngine.Random.Range(0, 4);
			if (randint < 4)
				Chat.AddMessage("Saxton Hale: "+jumpMessages[randint]);
			Util.PlaySound(BaseChargeFist.endChargeLoopSFXString, gameObject);
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.charge = Mathf.Clamp01(base.fixedAge / this.chargeDuration);
			base.characterBody.SetAimTimer(3f);
			if (this.charge >= minChargeForChargedAttack)
			{
				base.PlayCrossfade("Gesture, Additive", "ChargePunchIntro", "ChargePunchIntro.playbackRate", this.chargeDuration, 0.1f);
				base.PlayCrossfade("Gesture, Override", "ChargePunchIntro", "ChargePunchIntro.playbackRate", this.chargeDuration, 0.1f);
			}
			if (base.isAuthority)
			{
				this.AuthorityFixedUpdate();
			}
		}
		public override void Update()
		{
			base.Update();
			Mathf.Clamp01(base.age / this.chargeDuration);
		}

		private void AuthorityFixedUpdate()
		{
			if (!this.ShouldKeepChargingAuthority())
			{
				this.outer.SetNextState(this.GetNextStateAuthority());
			}
		}

		protected virtual bool ShouldKeepChargingAuthority()
		{
			return base.IsKeyDownAuthority();
		}

		protected virtual EntityState GetNextStateAuthority()
		{
			return new LaunchSuperjump
			{
				charge = this.charge
			};
		}
	}
}
