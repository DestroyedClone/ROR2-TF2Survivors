﻿using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;

namespace ROR2_Scout.States
{
    public class FireForceNature : FireShotgunBase
    {
        readonly float pushForce = 100f;

        public FireForceNature()
        {
            bulletCount = 8;
            recoilAmplitude = 3f;
            baseMaxDuration = 1.8f; //0.2 lower
            force = 300f;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority && base.characterMotor && !base.characterMotor.isGrounded)
            {
                Vector3 newVector = -GetAimRay().direction * pushForce;
                base.characterMotor.ApplyForce(newVector, true, false);
            }
        }
    }
}
