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

namespace ROR2_Scout.States
{
	public class FireShotgunBase : BaseState
	{
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

		public override void OnExit()
		{
			base.OnExit();
		}

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

		public override InterruptPriority GetMinimumInterruptPriority()
		{
			if (this.buttonReleased && base.fixedAge >= this.minDuration)
			{
				return InterruptPriority.Any;
			}
			return InterruptPriority.Skill;
		}

		public static GameObject effectPrefab;

		public static GameObject hitEffectPrefab;

		public static GameObject tracerEffectPrefab;

		public static float damageCoefficient = 1f;

		public static float force;

		public static int bulletCount;

		public static float baseMaxDuration = 2f;

		public static float baseMinDuration = 0.5f;

		public static string attackSoundString;

		public static float recoilAmplitude;

		public static float spreadBloomValue = 0.3f;

		private float maxDuration;

		private float minDuration;

		private bool buttonReleased;
	}
}
