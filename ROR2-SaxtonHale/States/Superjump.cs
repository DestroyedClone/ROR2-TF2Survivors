using RoR2;
using UnityEngine;
using EntityStates;

namespace ROR2_SaxtonHale.States
{
    public class SuperJump : BaseSkillState
    {
        public static float jumpDuration = 1.4f;
        public static float dropForce = 80f;

        public static float slamRadius = 20f;
        public static float slamDamageCoefficient = 32f;
        public static float slamProcCoefficient = 1f;
        public static float slamForce = 8000f;

        private bool hasDropped;
        private Vector3 flyVector = Vector3.zero;
        private Transform modelTransform;
        private Transform slamIndicatorInstance;
        private Ray downRay;

        public override void OnEnter()
        {
            base.OnEnter();
            this.modelTransform = base.GetModelTransform();
            this.flyVector = Vector3.up;
            this.hasDropped = false;

            base.PlayAnimation("FullBody, Override", "HighJump", "HighJump.playbackRate", SuperDededeJump.jumpDuration);
            Util.PlaySound(Croco.Leap.leapSoundString, base.gameObject);

            base.characterMotor.Motor.ForceUnground();
            base.characterMotor.velocity = Vector3.zero;

            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
        }

        public override void HandleMovements()
        {
            if (!this.hasDropped)
            {
                base.HandleMovements();

                base.characterMotor.rootMotion += this.flyVector * ((0.8f * this.moveSpeedStat) * Mage.FlyUpState.speedCoefficientCurve.Evaluate(base.fixedAge / SuperDededeJump.jumpDuration) * Time.fixedDeltaTime);
                base.characterMotor.velocity.y = 0f;
            }
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.slamIndicatorInstance) EntityState.Destroy(this.slamIndicatorInstance.gameObject);

            base.PlayAnimation("FullBody, Override", "BufferEmpty");

            base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
        }
    }
}