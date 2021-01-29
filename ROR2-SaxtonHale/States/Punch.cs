using System;
using RoR2;
using UnityEngine;
using EntityStates;
using EntityStates.Loader;
using RoR2.Skills;
using UnityEngine.Networking;

namespace ROR2_SaxtonHale.States
{
	// SwingCombo Fist
	public class Punch : BasicMeleeAttack, SteppedSkillDef.IStepSetter
	{
		public int gauntlet;

        public override void OnEnter()
        {
            base.OnEnter();
			base.damageCoefficient = 2f;
        }

		void SteppedSkillDef.IStepSetter.SetStep(int i)
		{
			this.gauntlet = i;
		}

		public override void PlayAnimation()
		{
			string animationStateName = (this.gauntlet == 0) ? "SwingFistRight" : "SwingFistLeft";
			float duration = Mathf.Max(this.duration, 0.2f);
			base.PlayCrossfade("Gesture, Additive", animationStateName, "SwingFist.playbackRate", duration, 0.1f);
			base.PlayCrossfade("Gesture, Override", animationStateName, "SwingFist.playbackRate", duration, 0.1f);
		}

		public override void BeginMeleeAttackEffect()
		{
			this.swingEffectMuzzleString = ((this.gauntlet == 0) ? "SwingRight" : "SwingLeft");
			base.BeginMeleeAttackEffect();
		}

		public override void OnSerialize(NetworkWriter writer)
		{
			base.OnSerialize(writer);
			writer.Write((byte)this.gauntlet);
		}

		public override void OnDeserialize(NetworkReader reader)
		{
			base.OnDeserialize(reader);
			this.gauntlet = (int)reader.ReadByte();
		}

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.Skill;
		}

	}
}
