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

		public static GameObject arcVisualizerPrefab;

		public static float arcVisualizerSimulationLength;

		public static int arcVisualizerVertexCount;

		[SerializeField]
		public float baseChargeDuration = 1f;

		public static float minChargeForChargedAttack;

		public static GameObject chargeVfxPrefab;

		public static string chargeVfxChildLocatorName;

		public static GameObject crosshairOverridePrefab;

		public static float walkSpeedCoefficient;

		public static string startChargeLoopSFXString;

		public static string endChargeLoopSFXString;

		public static string enterSFXString;

		private GameObject defaultCrosshairPrefab;

		private Transform chargeVfxInstanceTransform;

		private int gauntlet;

		private uint soundID;

		private class ArcVisualizer : IDisposable
		{
			public ArcVisualizer(GameObject arcVisualizerPrefab, float duration, int vertexCount)
			{
				this.arcVisualizerInstance = UnityEngine.Object.Instantiate<GameObject>(arcVisualizerPrefab);
				this.lineRenderer = this.arcVisualizerInstance.GetComponent<LineRenderer>();
				this.lineRenderer.positionCount = vertexCount;
				this.points = new Vector3[vertexCount];
				this.duration = duration;
			}

			public void Dispose()
			{
				EntityState.Destroy(this.arcVisualizerInstance);
			}

			public void SetParameters(Vector3 origin, Vector3 initialVelocity, float characterMaxSpeed, float characterAcceleration)
			{
				this.arcVisualizerInstance.transform.position = origin;
				if (!this.lineRenderer.useWorldSpace)
				{
					Vector3 eulerAngles = Quaternion.LookRotation(initialVelocity).eulerAngles;
					eulerAngles.x = 0f;
					eulerAngles.z = 0f;
					Quaternion rotation = Quaternion.Euler(eulerAngles);
					this.arcVisualizerInstance.transform.rotation = rotation;
					origin = Vector3.zero;
					initialVelocity = Quaternion.Inverse(rotation) * initialVelocity;
				}
				else
				{
					this.arcVisualizerInstance.transform.rotation = Quaternion.LookRotation(Vector3.Cross(initialVelocity, Vector3.up));
				}
				float y = Physics.gravity.y;
				float num = this.duration / (float)this.points.Length;
				Vector3 vector = origin;
				Vector3 vector2 = initialVelocity;
				float num2 = num;
				float num3 = y * num2;
				float maxDistanceDelta = characterAcceleration * num2;
				for (int i = 0; i < this.points.Length; i++)
				{
					this.points[i] = vector;
					Vector2 vector3 = Util.Vector3XZToVector2XY(vector2);
					vector3 = Vector2.MoveTowards(vector3, Vector3.zero, maxDistanceDelta);
					vector2.x = vector3.x;
					vector2.z = vector3.y;
					vector2.y += num3;
					vector += vector2 * num2;
				}
				this.lineRenderer.SetPositions(this.points);
			}

			private readonly Vector3[] points;

			private readonly float duration;

			private readonly GameObject arcVisualizerInstance;

			private readonly LineRenderer lineRenderer;
		}
		public override void OnEnter()
        {
            base.OnEnter();
            this.chargeDuration = this.baseChargeDuration / this.attackSpeedStat;
            Util.PlaySound(ChargeSuperJump.enterSFXString, base.gameObject);
            this.soundID = Util.PlaySound(ChargeSuperJump.startChargeLoopSFXString, base.gameObject);
        }

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.charge = Mathf.Clamp01(base.fixedAge / this.chargeDuration);
			//AkSoundEngine.SetRTPCValueByPlayingID("loaderShift_chargeAmount", this.charge * 100f, this.soundID);
			base.characterBody.SetSpreadBloom(this.charge, true);
			base.characterBody.SetAimTimer(3f);
			if (this.charge >= ChargeSuperJump.minChargeForChargedAttack && !this.chargeVfxInstanceTransform && ChargeSuperJump.chargeVfxPrefab)
			{
				this.defaultCrosshairPrefab = base.characterBody.crosshairPrefab;
				if (ChargeSuperJump.crosshairOverridePrefab)
				{
					base.characterBody.crosshairPrefab = ChargeSuperJump.crosshairOverridePrefab;
				}
				Transform transform = base.FindModelChild(ChargeSuperJump.chargeVfxChildLocatorName);
				if (transform)
				{
					this.chargeVfxInstanceTransform = UnityEngine.Object.Instantiate<GameObject>(ChargeSuperJump.chargeVfxPrefab, transform).transform;
					ScaleParticleSystemDuration component = this.chargeVfxInstanceTransform.GetComponent<ScaleParticleSystemDuration>();
					if (component)
					{
						component.newDuration = (1f - ChargeSuperJump.minChargeForChargedAttack) * this.chargeDuration;
					}
				}
				base.PlayCrossfade("Gesture, Additive", "ChargePunchIntro", "ChargePunchIntro.playbackRate", this.chargeDuration, 0.1f);
				base.PlayCrossfade("Gesture, Override", "ChargePunchIntro", "ChargePunchIntro.playbackRate", this.chargeDuration, 0.1f);
			}
			if (this.chargeVfxInstanceTransform)
			{
				base.characterMotor.walkSpeedPenaltyCoefficient = ChargeSuperJump.walkSpeedCoefficient;
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
