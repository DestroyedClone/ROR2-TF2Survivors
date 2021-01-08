using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using EntityStates;
using RoR2;
using RoR2.Skills;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using KinematicCharacterController;
using EntityStates.Commando;
using EntityStates.Commando.CommandoWeapon;

namespace ROR2_TF2Survivors.Scout.States
{
	public class FireShotgun : BaseState
	{
		// Token: 0x06003E21 RID: 15905 RVA: 0x001045F0 File Offset: 0x001027F0
		public override void OnEnter()
		{
			base.OnEnter();
			base.AddRecoil(-1f * FireShotgun.recoilAmplitude, -2f * FireShotgun.recoilAmplitude, -0.5f * FireShotgun.recoilAmplitude, 0.5f * FireShotgun.recoilAmplitude);
			this.maxDuration = FireShotgun.baseMaxDuration / this.attackSpeedStat;
			this.minDuration = FireShotgun.baseMinDuration / this.attackSpeedStat;
			Ray aimRay = base.GetAimRay();
			base.StartAimMode(aimRay, 2f, false);
			Util.PlaySound(FireShotgun.attackSoundString, base.gameObject);
			base.PlayAnimation("Gesture, Additive", "FireShotgun", "FireShotgun.playbackRate", this.maxDuration * 1.1f);
			base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.maxDuration * 1.1f);
			string muzzleName = "MuzzleShotgun";
			if (FireShotgun.effectPrefab)
			{
				EffectManager.SimpleMuzzleFlash(FireShotgun.effectPrefab, base.gameObject, muzzleName, false);
			}
			if (base.isAuthority)
			{
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0f,
					maxSpread = base.characterBody.spreadBloomAngle,
					bulletCount = (uint)((FireShotgun.bulletCount > 0) ? FireShotgun.bulletCount : 0),
					procCoefficient = 1f / (float)FireShotgun.bulletCount,
					damage = FireShotgun.damageCoefficient * this.damageStat / (float)FireShotgun.bulletCount,
					force = FireShotgun.force,
					falloffModel = BulletAttack.FalloffModel.DefaultBullet,
					tracerEffectPrefab = FireShotgun.tracerEffectPrefab,
					muzzleName = muzzleName,
					hitEffectPrefab = FireShotgun.hitEffectPrefab,
					isCrit = Util.CheckRoll(this.critStat, base.characterBody.master),
					HitEffectNormal = false,
					radius = 0f
				}.Fire();
			}
			base.characterBody.AddSpreadBloom(FireShotgun.spreadBloomValue);
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x00032EEB File Offset: 0x000310EB
		public override void OnExit()
		{
			base.OnExit();
		}

		// Token: 0x06003E23 RID: 15907 RVA: 0x001047F8 File Offset: 0x001029F8
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			this.buttonReleased |= !base.inputBank.skill1.down;
			if (base.fixedAge >= this.maxDuration && base.isAuthority)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x0010484D File Offset: 0x00102A4D
		public override InterruptPriority GetMinimumInterruptPriority()
		{
			if (this.buttonReleased && base.fixedAge >= this.minDuration)
			{
				return InterruptPriority.Any;
			}
			return InterruptPriority.Skill;
		}

		// Token: 0x040039B7 RID: 14775
		public static GameObject effectPrefab;

		// Token: 0x040039B8 RID: 14776
		public static GameObject hitEffectPrefab;

		// Token: 0x040039B9 RID: 14777
		public static GameObject tracerEffectPrefab;

		// Token: 0x040039BA RID: 14778
		public static float damageCoefficient = 1f;

		// Token: 0x040039BB RID: 14779
		public static float force;

		// Token: 0x040039BC RID: 14780
		public static int bulletCount;

		// Token: 0x040039BD RID: 14781
		public static float baseMaxDuration = 2f;

		// Token: 0x040039BE RID: 14782
		public static float baseMinDuration = 0.5f;

		// Token: 0x040039BF RID: 14783
		public static string attackSoundString;

		// Token: 0x040039C0 RID: 14784
		public static float recoilAmplitude;

		// Token: 0x040039C1 RID: 14785
		public static float spreadBloomValue = 0.3f;

		// Token: 0x040039C2 RID: 14786
		private float maxDuration;

		// Token: 0x040039C3 RID: 14787
		private float minDuration;

		// Token: 0x040039C4 RID: 14788
		private bool buttonReleased;
	}
}
