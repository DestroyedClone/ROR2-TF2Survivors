using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ROR2_Scout
{
    public class ExtinguishZonePrefabAlt : BuffWard
    {
		private void ExtinguishTeam(IEnumerable<TeamComponent> recipients, float radiusSqr, Vector3 currentPosition)
		{
			if (!NetworkServer.active)
			{
				return;
			}
			foreach (TeamComponent teamComponent in recipients)
			{
				if ((teamComponent.transform.position - currentPosition).sqrMagnitude <= radiusSqr)
				{
					CharacterBody characterBody = teamComponent.GetComponent<CharacterBody>();
					if (characterBody)
					{
						characterBody.ClearTimedBuffs(BuffIndex.OnFire);

						if (DotController.dotControllerLocator.TryGetValue(characterBody.gameObject.GetInstanceID(), out DotController dotController))
						{
							//var burnEffectController = dotController.burnEffectController;
							var dotStacks = dotController.dotStackList;

							int i = 0;
							int count = dotStacks.Count;
							while (i < count)
							{
								if (dotStacks[i].dotIndex == DotController.DotIndex.Burn
									|| dotStacks[i].dotIndex == DotController.DotIndex.Helfire
									|| dotStacks[i].dotIndex == DotController.DotIndex.PercentBurn)
								{
									dotStacks[i].damage = 0f;
									dotStacks[i].timer = 0f;
								}
								i++;
							}
							//Destroy(dotController.gameObject);
						}
					}
				}
			}
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "UnityEngine")]
        private new void FixedUpdate()
		{
			if (NetworkServer.active)
			{
				this.buffTimer -= Time.fixedDeltaTime;
				if (this.buffTimer <= 0f)
				{
					this.buffTimer = this.interval;
					float radiusSqr = this.calculatedRadius * this.calculatedRadius;
					Vector3 position = base.transform.position;
					if (this.invertTeamFilter)
					{
						for (TeamIndex teamIndex = TeamIndex.Neutral; teamIndex < TeamIndex.Count; teamIndex += 1)
						{
							if (teamIndex != this.teamFilter.teamIndex)
							{
								this.ExtinguishTeam(TeamComponent.GetTeamMembers(teamIndex), radiusSqr, position);
							}
						}
						return;
					}
					this.ExtinguishTeam(TeamComponent.GetTeamMembers(this.teamFilter.teamIndex), radiusSqr, position);
				}
			}
		}
	}
}
