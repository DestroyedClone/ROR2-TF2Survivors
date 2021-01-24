using BepInEx.Configuration;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Text;

namespace ROR2_Scout.Equipments
{
    public class BonkEquipment : EquipmentBase
    {
        public static ConfigEntry<float> cooldown;

        public override string EquipmentName => "Bonk! Atomic Punch";
        public override string EquipmentLangTokenName => "BONK_EQUIPMENT";
        public override string EquipmentPickupDesc => "Become immune to damage, but unable to attack.";
        public override string EquipmentFullDescription => "Use to gain <style=cIsUtility>immunity to damage</style> but not knockback for 8 seconds." +
                    "\n<style=cDeath>Unable to attack while active.</style>";
        public override string EquipmentLore => "'BONK' ENERGY DRINK" +
            "\n\"I AM A FRICKIN' BLUR HERE\" - Satisfied Customer" +
            "\n\nRunnin’ rings around all them molasses-slow dummies out there on the battle- field is thirsty frickin’ work. But when it comes to quenchin’ that thirst, only one thermonuclear thirst detonator packs all the “Atomic Punch” you’ll ever need. Bonk! is fulla radiation, which as we all know is pretty great for givin’ people superpowers. Just one can’ll blast ya into a few-second rush of radioactive energy so powerful you’ll be dodgin’ bullets like they ain’t even there!" +
            "\n\n<style=cSub><size=80%>Note: Bonk! contains several hundred times the daily recommended allowance of sugar. After the beneficial effects of the radiation wear off, drinkers have reported experiencing feelings of lethargy that can last several seconds. Reading this sentence absolves Bonk! of any liability for killing sprees inspired by or deaths resulting from the ingestion of Bonk! Enjoy Bonk! responsibly… Or by the case!</size></style>";
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
                characterBody.AddTimedBuffAuthority(Modules.Buffs.bonkBuff, 8f);
                return true;
            }
            return false;
        }
    }
}
