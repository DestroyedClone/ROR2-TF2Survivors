using EntityStates;
using R2API;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ROR2_SaxtonHale.Modules
{
    public static class Skills
    {
        private static SkillLocator skillLocator;
        public static SkillDef NoAttackSkillDef;
        public static SkillDef crouchSkillDef;
        public static SkillDef weighdownSkillDef;

        public static void SetupSkills(GameObject bodyPrefab)
        {
            foreach (GenericSkill obj in bodyPrefab.GetComponentsInChildren<GenericSkill>())
            {
                SaxtonHalePlugin.DestroyImmediate(obj);
            }

            skillLocator = bodyPrefab.GetComponent<SkillLocator>();

            PassiveSetup();
            PrimarySetup(bodyPrefab);
            SecondarySetup(bodyPrefab);
            UtilitySetup(bodyPrefab);
            SpecialSetup(bodyPrefab);
            AdditionalSetup();
        }

        private static void PassiveSetup()
        {
            skillLocator.passiveSkill.enabled = true;
            skillLocator.passiveSkill.skillNameToken = "SAXTONHALE_PASSIVE_CLASSIC_NAME";
            skillLocator.passiveSkill.skillDescriptionToken = "SAXTONHALE_PASSIVE_CLASSIC_DESCRIPTION";
            //skillLocator.passiveSkill.icon = Assets.iconP;
        }

        private static void PrimarySetup(GameObject bodyPrefab)
        {
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(EntityStates.Loader.SwingComboFist));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("textures/bufficons/texBuffPulverizeIcon");
            mySkillDef.skillDescriptionToken = "SAXTONHALE_PRIMARY_FISTS_DESCRIPTION";
            mySkillDef.skillName = "SAXTONHALE_PRIMARY_FISTS_NAME";
            mySkillDef.skillNameToken = "SAXTONHALE_PRIMARY_FISTS_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.primary = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.primary._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private static void SecondarySetup(GameObject bodyPrefab)
        {
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ROR2_SaxtonHale.States.ChargeSuperJump));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 10f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Death;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = true;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("textures/bufficons/texWarcryBuffIcon");
            mySkillDef.skillDescriptionToken = "SAXTONHALE_SECONDARY_SUPERJUMP_DESCRIPTION";
            mySkillDef.skillName = "SAXTONHALE_SECONDARY_SUPERJUMP_NAME";
            mySkillDef.skillNameToken = "SAXTONHALE_SECONDARY_SUPERJUMP_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.secondary = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.secondary._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private static void UtilitySetup(GameObject bodyPrefab)
        {
            crouchSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            crouchSkillDef.activationState = new SerializableEntityStateType(typeof(ROR2_SaxtonHale.States.Crouch));
            crouchSkillDef.activationStateMachineName = "Weapon";
            crouchSkillDef.baseMaxStock = 1;
            crouchSkillDef.baseRechargeInterval = 0f;
            crouchSkillDef.beginSkillCooldownOnSkillEnd = true;
            crouchSkillDef.canceledFromSprinting = false;
            crouchSkillDef.fullRestockOnAssign = true;
            crouchSkillDef.interruptPriority = InterruptPriority.Death;
            crouchSkillDef.isBullets = false;
            crouchSkillDef.isCombatSkill = false;
            crouchSkillDef.mustKeyPress = true;
            crouchSkillDef.noSprint = true;
            crouchSkillDef.rechargeStock = 1;
            crouchSkillDef.requiredStock = 1;
            crouchSkillDef.shootDelay = 0.5f;
            crouchSkillDef.stockToConsume = 1;
            crouchSkillDef.icon = Resources.Load<Sprite>("textures/bufficons/texBuffSlow80Icon");
            crouchSkillDef.skillDescriptionToken = "SAXTONHALE_UTILITY_CROUCH_DESCRIPTION";
            crouchSkillDef.skillName = "SAXTONHALE_UTILITY_CROUCH_NAME";
            crouchSkillDef.skillNameToken = "SAXTONHALE_UTILITY_CROUCH_NAME";

            weighdownSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            weighdownSkillDef.activationState = new SerializableEntityStateType(typeof(ROR2_SaxtonHale.States.Weighdown));
            weighdownSkillDef.activationStateMachineName = "Weapon";
            weighdownSkillDef.baseMaxStock = 1;
            weighdownSkillDef.baseRechargeInterval = 0f;
            weighdownSkillDef.beginSkillCooldownOnSkillEnd = true;
            weighdownSkillDef.canceledFromSprinting = false;
            weighdownSkillDef.fullRestockOnAssign = true;
            weighdownSkillDef.interruptPriority = InterruptPriority.Death;
            weighdownSkillDef.isBullets = false;
            weighdownSkillDef.isCombatSkill = false;
            weighdownSkillDef.mustKeyPress = true;
            weighdownSkillDef.noSprint = true;
            weighdownSkillDef.rechargeStock = 1;
            weighdownSkillDef.requiredStock = 1;
            weighdownSkillDef.shootDelay = 0.5f;
            weighdownSkillDef.stockToConsume = 1;
            weighdownSkillDef.icon = Resources.Load<Sprite>("textures/bufficons/texBuffSlow50Icon");
            weighdownSkillDef.skillDescriptionToken = "SAXTONHALE_UTILITY_WEIGHDOWN_DESCRIPTION";
            weighdownSkillDef.skillName = "SAXTONHALE_UTILITY_WEIGHDOWN_NAME";
            weighdownSkillDef.skillNameToken = "SAXTONHALE_UTILITY_WEIGHDOWN_NAME";


            LoadoutAPI.AddSkillDef(crouchSkillDef);
            LoadoutAPI.AddSkillDef(weighdownSkillDef);

            skillLocator.utility = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.utility._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.utility.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = crouchSkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(crouchSkillDef.skillNameToken, false, null)
            };
        }

        private static void SpecialSetup(GameObject bodyPrefab)
        {
            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ROR2_SaxtonHale.States.Rage));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 300f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.PrioritySkill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("textures/itemicons/texMaskIcon");
            mySkillDef.skillDescriptionToken = "SAXTONHALE_SPECIAL_CLASSIC_DESCRIPTION";
            mySkillDef.skillName = "SAXTONHALE_SPECIAL_CLASSIC_NAME";
            mySkillDef.skillNameToken = "SAXTONHALE_SPECIAL_CLASSIC_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.special = bodyPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.special._skillFamily = newFamily;
            SkillFamily skillFamily = skillLocator.special.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }

        private static void AdditionalSetup()
        {
            NoAttackSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            NoAttackSkillDef.activationState = new SerializableEntityStateType(typeof(ROR2_SaxtonHale.States.NoAttackState));
            NoAttackSkillDef.activationStateMachineName = "Weapon";
            NoAttackSkillDef.baseMaxStock = 0;
            NoAttackSkillDef.baseRechargeInterval = 0f;
            NoAttackSkillDef.beginSkillCooldownOnSkillEnd = true;
            NoAttackSkillDef.canceledFromSprinting = false;
            NoAttackSkillDef.fullRestockOnAssign = false;
            NoAttackSkillDef.interruptPriority = InterruptPriority.Frozen;
            NoAttackSkillDef.isBullets = false;
            NoAttackSkillDef.isCombatSkill = false;
            NoAttackSkillDef.mustKeyPress = false;
            NoAttackSkillDef.noSprint = true;
            NoAttackSkillDef.rechargeStock = 0;
            NoAttackSkillDef.requiredStock = 1;
            NoAttackSkillDef.shootDelay = 0.5f;
            NoAttackSkillDef.stockToConsume = 1;
            NoAttackSkillDef.icon = Resources.Load<SkillDef>("skilldefs/captainbody/CaptainSkillUsedUp").icon;
            NoAttackSkillDef.skillDescriptionToken = "SAXTONHALE_SCARED_DESCRIPTION";
            NoAttackSkillDef.skillName = "SAXTONHALE_SCARED_NAME";
            NoAttackSkillDef.skillNameToken = "SAXTONHALE_SCARED_NAME";
        }
    }
}
