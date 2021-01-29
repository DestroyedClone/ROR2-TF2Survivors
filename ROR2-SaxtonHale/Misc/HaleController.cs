﻿using RoR2;
using R2API;
using UnityEngine;

namespace ROR2_SaxtonHale.Misc
{
    public class HaleController : MonoBehaviour
    {
        public SkillLocator skillLocator;
        public float airborneStopwatch = 0f;
        readonly float minTimeForWeighdown = 6f;

        public void Start()
        {
            if (!skillLocator)
            {
                skillLocator = gameObject.GetComponent<CharacterBody>()?.skillLocator;
            }
            if (!skillLocator) Destroy(this);
        }

        public void FixedUpdate()
        {
            var body = gameObject.GetComponent<CharacterBody>();
            if (body && body.characterMotor)
            {
                if (body.characterMotor.isGrounded)
                {
                    airborneStopwatch = 0f;
                    skillLocator.utility.SetSkillOverride(body, Modules.Skills.crouchSkillDef, GenericSkill.SkillOverridePriority.Replacement);
                }
                else
                {
                    airborneStopwatch += Time.deltaTime;
                    if (airborneStopwatch >= minTimeForWeighdown)
                    {
                        skillLocator.utility.SetSkillOverride(body, Modules.Skills.weighdownSkillDef, GenericSkill.SkillOverridePriority.Replacement);
                    }
                }
            }
        }
    }
}
