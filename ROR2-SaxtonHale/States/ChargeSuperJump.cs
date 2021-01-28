using System;
using RoR2;
using UnityEngine;
using EntityStates;

namespace ROR2_TF2Survivors
{
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
	public class ChargeSuperJump : BaseSkillState
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
			//Util.PlaySound(BaseChargeFist.endChargeLoopSFXString, gameObject);
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
			if (this.charge >= BaseChargeFist.minChargeForChargedAttack && !this.chargeVfxInstanceTransform && BaseChargeFist.chargeVfxPrefab)
			{
				this.defaultCrosshairPrefab = base.characterBody.crosshairPrefab;
				if (BaseChargeFist.crosshairOverridePrefab)
				{
					base.characterBody.crosshairPrefab = BaseChargeFist.crosshairOverridePrefab;
				}
				Transform transform = base.FindModelChild(BaseChargeFist.chargeVfxChildLocatorName);
				if (transform)
				{
					this.chargeVfxInstanceTransform = UnityEngine.Object.Instantiate<GameObject>(BaseChargeFist.chargeVfxPrefab, transform).transform;
					ScaleParticleSystemDuration component = this.chargeVfxInstanceTransform.GetComponent<ScaleParticleSystemDuration>();
					if (component)
					{
						component.newDuration = (1f - BaseChargeFist.minChargeForChargedAttack) * this.chargeDuration;
					}
				}
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
			return new LaunchSuperJump
			{
				charge = this.charge
			};
		}
	}
}
