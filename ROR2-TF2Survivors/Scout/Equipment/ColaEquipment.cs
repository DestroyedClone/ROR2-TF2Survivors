using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using ROR2_TF2Survivors.Base_Classes;

namespace ROR2_TF2Survivors.Scout.Equipments
{
    public class ColaEquipment : EquipmentBase
    {
        public static ConfigEntry<float> cooldown;

        public override string EquipmentName => "Bonk! Crit-a-Cola";
        public override string EquipmentLangTokenName => "COLA_EQUIPMENT";
        public override string EquipmentPickupDesc => "Temporarily deal and receive more damage.";
        public override string EquipmentFullDescription => "Use to deal <style=cIsDamage>+35% more damage</style>, but <style=cDeath>take +50% more damage</style> for 8 seconds.";
        public override string EquipmentLore => "";
        public override string EquipmentModelPath => "";
        public override string EquipmentIconPath => "";

        public override float Cooldown => cooldown.Value;

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
        }

        public override bool IsLunar => true;

        private void CreateConfig(ConfigFile config)
        {
            cooldown = config.Bind<float>("Equipment: " + EquipmentName, "Cooldown duration", 60f, "How long should the cooldown be? (seconds)");
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            throw new NotImplementedException();
        }

        public override void Hooks()
        {
            throw new NotImplementedException();
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            var characterBody = slot.characterBody;
            if (characterBody)
            {
                characterBody.AddTimedBuffAuthority(Buffs.critColaBuff, 8f);
                return true;
            }
            return false;
        }
    }
}
