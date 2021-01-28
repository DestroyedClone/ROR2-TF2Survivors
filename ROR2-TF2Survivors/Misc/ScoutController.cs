using System;
using System.Linq;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Networking;
using KinematicCharacterController;

using static RoR2.SkillLocator;

namespace ROR2_Scout.Misc
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "UnityEngine")]
    public class ScoutController : MonoBehaviour
    {
        CharacterBody scoutBody;
        SkillLocator scoutSkillLocator;
        bool currentlyBuffed = false;
        readonly float[] rechargeStopwatches = //rd
        {
                0f,0f,0f,0f
            };
        readonly int[] stocks = //rd
{
                0,0,0,0
            };

        void Awake()
        {
            scoutBody = gameObject.GetComponent<CharacterBody>();
            scoutSkillLocator = scoutBody?.masterObject.GetComponent<SkillLocator>();
            if (!scoutBody || !scoutSkillLocator)
            {
                Chat.AddMessage("ScoutController failed: missing objects");
                enabled = false;
            }
        }

        void FixedUpdate()
        {
            if (scoutBody)
            {
                if (scoutBody.HasBuff(Modules.Buffs.bonkBuff))
                {
                    if (!currentlyBuffed) // so it happens once
                    {
                        // TODO: Network this
                        // Referenced from SkillLocator's DeductCooldownFromAllSkillsAuthority
                        for (int i = 0; i < scoutSkillLocator.allSkills.Length; i++)
                        {
                            GenericSkill genericSkill = scoutSkillLocator.allSkills[i];

                            rechargeStopwatches[i] = genericSkill.rechargeStopwatch;
                            stocks[i] = genericSkill.stock;

                            genericSkill.rechargeStopwatch = -1000f;
                            genericSkill.stock = -1000;
                            currentlyBuffed = true;
                        }
                    }
                }
                else
                {
                    if (currentlyBuffed)
                    { //reset the stocks back to normal
                        for (int i = 0; i < scoutSkillLocator.allSkills.Length; i++)
                        {
                            GenericSkill genericSkill = scoutSkillLocator.allSkills[i];

                            genericSkill.rechargeStopwatch = rechargeStopwatches[i];
                            genericSkill.stock = stocks[i];
                            currentlyBuffed = false;
                        }
                    }
                }
            }
        }
    }
}
