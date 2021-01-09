using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using UnityEngine;
using RoR2.Projectile;
using RoR2;

namespace ROR2_Scout.States
{
    public class FireMarkFan : BaseSkillState
    {
        public float baseDuration = 0.5f;
        private float duration;
        public float damageCoefficient = 0.3f;
        public bool skillUsed = false;
        public DamageType DamageTypeValue = DamageType.CrippleOnHit;
        public GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/critspark");
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / base.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            if (base.isAuthority)
            {
                PlayAnim(0.5f);
                new BlastAttack
                {
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    position = aimRay.origin,
                    procCoefficient = 1f,
                    losType = BlastAttack.LoSType.NearestHit,
                    falloffModel = BlastAttack.FalloffModel.None,
                    baseDamage = base.characterBody.damage * damageCoefficient,
                    damageType = DamageTypeValue,
                    crit = base.RollCrit(),
                    radius = 2.5f,
                    teamIndex = base.GetTeam()
                }.Fire();
                skillUsed = true;
            }
        }
        private void PlayAnim(float duration) //from FireFMJ
        {
            PlayAnimation("Gesture, Additive", "ThrowGrenade", "FireFMJ.playbackRate", duration * 2f);
            PlayAnimation("Gesture, Override", "ThrowGrenade", "FireFMJ.playbackRate", duration * 2f);
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                skillUsed = false;
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
