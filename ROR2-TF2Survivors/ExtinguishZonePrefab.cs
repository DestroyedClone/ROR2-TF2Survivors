using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace RoR2
{
	// LITERALLY A BUFFWARD WITH MORE DETAILS FUCKING 3AM BRAIN


	// Token: 0x020001AD RID: 429
	[RequireComponent(typeof(TeamFilter))]
	public class ExtinguishZone : NetworkBehaviour
	{
		// Token: 0x06000833 RID: 2099 RVA: 0x0001FE63 File Offset: 0x0001E063
		private void Awake()
		{
			this.teamFilter = base.GetComponent<TeamFilter>();
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0001FE71 File Offset: 0x0001E071
		private void OnEnable()
		{
			if (this.rangeIndicator)
			{
				this.rangeIndicator.gameObject.SetActive(true);
			}
		}

		private void OnDisable()
		{
			if (this.rangeIndicator)
			{
				this.rangeIndicator.gameObject.SetActive(false);
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x0001FEB4 File Offset: 0x0001E0B4
		private void Start()
		{
			if (this.removalTime > 0f)
			{
				this.needsRemovalTime = true;
			}
            if (this.floorWard && Physics.Raycast(base.transform.position, Vector3.down, out RaycastHit raycastHit, 500f, LayerIndex.world.mask))
            {
                base.transform.position = raycastHit.point;
                base.transform.up = raycastHit.normal;
            }
            if (this.rangeIndicator && this.expires)
			{
				ScaleParticleSystemDuration component = this.rangeIndicator.GetComponent<ScaleParticleSystemDuration>();
				if (component)
				{
					component.newDuration = this.expireDuration;
				}
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0001FF68 File Offset: 0x0001E168
		private void Update()
		{
			this.calculatedRadius = (this.animateRadius ? (this.radius * this.radiusCoefficientCurve.Evaluate(this.stopwatch / this.expireDuration)) : this.radius);
			this.stopwatch += Time.deltaTime;
			if (this.expires && NetworkServer.active)
			{
				if (this.needsRemovalTime)
				{
					if (this.stopwatch >= this.expireDuration - this.removalTime)
					{
						this.needsRemovalTime = false;
						Util.PlaySound(this.removalSoundString, base.gameObject);
						this.onRemoval.Invoke();
					}
				}
				else if (this.expireDuration <= this.stopwatch)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
			if (this.rangeIndicator)
			{
				float num = Mathf.SmoothDamp(this.rangeIndicator.localScale.x, this.calculatedRadius, ref this.rangeIndicatorScaleVelocity, 0.2f);
				this.rangeIndicator.localScale = new Vector3(num, num, num);
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00020070 File Offset: 0x0001E270
		private void FixedUpdate()
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

		/*BuffIndex[] buffIndices =
		{
			BuffIndex.OnFire,
		}; */

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
						//characterBody.AddTimedBuff(this.buffType, this.buffDuration);

						/*foreach (BuffIndex buffIndex in buffIndices)
						{
							characterBody.ClearTimedBuffs(buffIndex);
						}*/ // we only have one so its a waste

						characterBody.ClearTimedBuffs(BuffIndex.OnFire);

                        if (DotController.dotControllerLocator.TryGetValue(characterBody.gameObject.GetInstanceID(), out DotController dotController))
                        {
							var burnEffectController = dotController.burnEffectController;
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

							Destroy(dotController.gameObject);
                        }
                    }
				}
			}
		}

		private void UNetVersion()
		{
		}
		public float Networkradius
		{
			get
			{
				return this.radius;
			}
			[param: In]
			set
			{
				base.SetSyncVar<float>(value, ref this.radius, 1U);
			}
		}

		public override bool OnSerialize(NetworkWriter writer, bool forceAll)
		{
			if (forceAll)
			{
				writer.Write(this.radius);
				return true;
			}
			bool flag = false;
			if ((base.syncVarDirtyBits & 1U) != 0U)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(base.syncVarDirtyBits);
					flag = true;
				}
				writer.Write(this.radius);
			}
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
			}
			return flag;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if (initialState)
			{
				this.radius = reader.ReadSingle();
				return;
			}
			int num = (int)reader.ReadPackedUInt32();
			if ((num & 1) != 0)
			{
				this.radius = reader.ReadSingle();
			}
		}

		[Tooltip("The area of effect.")]
		[SyncVar]
		public float radius;

		[Tooltip("How long between buff pulses in the area of effect.")]
		public float interval = 1f;

		[Tooltip("The child range indicator object. Will be scaled to the radius.")]
		public Transform rangeIndicator;

		[Tooltip("The buff type to grant")]
		public BuffIndex buffType;

		[Tooltip("The buff duration")]
		public float buffDuration;

		[Tooltip("Should the ward be floored on start")]
		public bool floorWard;

		[Tooltip("Does the ward disappear over time?")]
		public bool expires;

		[Tooltip("If set, applies to all teams BUT the one selected.")]
		public bool invertTeamFilter;

		public float expireDuration;

		public bool animateRadius;

		public AnimationCurve radiusCoefficientCurve;

		[Tooltip("If set, the ward will give you this amount of time to play removal effects.")]
		public float removalTime;

		private bool needsRemovalTime;

		public string removalSoundString = "";

		public UnityEvent onRemoval;

		private TeamFilter teamFilter;

		private float buffTimer;

		private float rangeIndicatorScaleVelocity;

		private float stopwatch;

		private float calculatedRadius;
	}
}
