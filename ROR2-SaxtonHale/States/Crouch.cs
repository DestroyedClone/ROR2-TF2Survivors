using EntityStates;
using RoR2;
using UnityEngine;
using System;
using UnityEngine.Networking;
using RoR2.Projectile;
using System.Collections.ObjectModel;

namespace ROR2_SaxtonHale.States
{
    public class Crouch : BaseSkillState
    {
        private readonly float crouchMuliplier = 0.5f;
        private float cachedCapsuleHeight;
        private bool isCrouching = false;

        public override void OnEnter()
        {
            base.OnEnter();
            cachedCapsuleHeight = this.characterMotor.Motor.CapsuleHeight;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                if (ShouldKeepChargingAuthority())
                {
                    ReduceCapsuleHeight(crouchMuliplier);
                } else
                {
                    ReduceCapsuleHeight(1f);
                    this.outer.SetNextStateToMain();
                }
            }
        }

        public void ReduceCapsuleHeight(float multiplier)
        {
            this.characterMotor.Motor.CapsuleHeight = cachedCapsuleHeight * multiplier;
        }

        public override void OnExit()
        {
            base.OnExit();
        }
        protected virtual bool ShouldKeepChargingAuthority()
        {
            return base.IsKeyDownAuthority();
        }
    }
}
