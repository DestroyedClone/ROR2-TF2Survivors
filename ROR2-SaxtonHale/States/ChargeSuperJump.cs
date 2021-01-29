using System;
using RoR2;
using UnityEngine;
using EntityStates;

namespace ROR2_SaxtonHale.States
{
    public class ChargeSuperJump : BaseSkillState
    {
        public float chargeDuration { get; set; }
        public float charge { get; set; }

		[SerializeField]
		public float baseChargeDuration = 1f;

		public static float minChargeForChargedAttack;

		public override void OnEnter()
        {
            base.OnEnter();
            this.chargeDuration = this.baseChargeDuration / this.attackSpeedStat;
        }

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.charge = Mathf.Clamp01(base.fixedAge / this.chargeDuration);
			base.characterBody.SetSpreadBloom(this.charge, true);
			base.characterBody.SetAimTimer(3f);
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
