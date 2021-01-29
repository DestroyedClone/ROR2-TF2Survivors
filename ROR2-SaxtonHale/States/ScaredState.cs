using System;
using RoR2;
using UnityEngine;
using EntityStates;

namespace ROR2_SaxtonHale.States
{
    public class ScaredState : BaseState
	{
		private float duration;
		public float scareDuration = 0.35f;

		public override void OnEnter()
		{
			base.OnEnter();
			if (base.sfxLocator && base.sfxLocator.barkSound != "")
			{
				Util.PlaySound(base.sfxLocator.barkSound, base.gameObject);
			}
			if (base.rigidbody && !base.rigidbody.isKinematic)
			{
				base.rigidbody.velocity = Vector3.zero;
				if (base.rigidbodyMotor)
				{
					base.rigidbodyMotor.moveVector = Vector3.zero;
				}
			}
			base.healthComponent.isInFrozenState = true;
			this.duration = this.scareDuration;
		}

		public override void OnExit()
		{
			EffectManager.SpawnEffect(FrozenState.frozenEffectPrefab, new EffectData
			{
				origin = base.characterBody.corePosition,
				scale = (base.characterBody ? base.characterBody.radius : 1f)
			}, false);
			base.healthComponent.isInFrozenState = false;
			base.OnExit();
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (base.isAuthority && base.fixedAge >= this.duration)
			{
				this.outer.SetNextStateToMain();
			}
		}
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Frozen;
		}
	}
}
