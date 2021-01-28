using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using System.Collections.ObjectModel;

namespace ROR2_SaxtonHale.States
{
	public class Rage : BaseState //from EntityStates.BeetleGuardMonster.DefenseUp
	{
		public static float baseDuration = 0.5f;
		public static float buffDuration = 8f;
		private float duration = 8f;
		private bool hasCastBuff;
		private readonly BuffIndex buffIndex = Modules.Buffs.scaredDebuff;


		// Token: 0x0600401F RID: 16415 RVA: 0x0010D9D0 File Offset: 0x0010BBD0
		public override void OnEnter()
		{
			base.OnEnter();
			this.duration = Rage.baseDuration / this.attackSpeedStat;
		}

		// Token: 0x06004020 RID: 16416 RVA: 0x0010DA30 File Offset: 0x0010BC30
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!this.hasCastBuff)
			{
				this.hasCastBuff = true;
				if (NetworkServer.active)
				{
					if (teamComponent)
					{
						BuffTeam(GetEnemyTeam(teamComponent.teamIndex), buffIndex);
					}
				}
			}
			if (base.fixedAge >= this.duration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		private void BuffTeam(TeamIndex teamIndex, BuffIndex buffIndex)
		{
			ReadOnlyCollection<TeamComponent> teamComponents = TeamComponent.GetTeamMembers(teamIndex);
			foreach (var teamComponent in teamComponents)
			{
				CharacterBody body = teamComponent.body;
				if (body)
				{
					body.AddTimedBuff(buffIndex, buffDuration);
				}
			}
		}

		private TeamIndex GetEnemyTeam(TeamIndex teamIndex)
        {
			if (teamIndex == TeamIndex.Monster) return TeamIndex.Player;
			else if (teamIndex == TeamIndex.Player) return TeamIndex.Monster;
			else return TeamIndex.Neutral;
        }

		// Token: 0x06004021 RID: 16417 RVA: 0x0000D742 File Offset: 0x0000B942
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			return InterruptPriority.PrioritySkill;
		}
	}
}
