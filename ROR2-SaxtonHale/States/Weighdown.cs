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
        private float downwardForceScale = 1f;
		protected bool hasSwung = false;

		public override void OnEnter()
		{
			base.OnEnter();
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
				}
				if (this.hasSwung)
				{
					if (base.characterMotor && base.characterDirection)
					{
						base.characterMotor.velocity.y = 0;
						base.characterMotor.velocity.y -= downwardForceScale;
					}
				}
				if (base.isGrounded)
				{
					this.outer.SetNextStateToMain();
				}
			}
		}
	}
}
