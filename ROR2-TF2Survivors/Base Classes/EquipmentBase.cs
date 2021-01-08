using BepInEx.Configuration;
using RoR2;
using R2API;
using System;
using System.Collections.Generic;
using System.Text;

namespace ROR2_TF2Survivors.Base_Classes
{
    public abstract class EquipmentBase
    {
        public abstract string EquipmentName { get; }
        public abstract string EquipmentLangTokenName { get; }
        public abstract string EquipmentPickupDesc { get; }
        public abstract string EquipmentFullDescription { get; }
        public abstract string EquipmentLore { get; }

        public abstract string EquipmentModelPath { get; }
        public abstract string EquipmentIconPath { get; }

        public virtual bool AppearsInSinglePlayer { get; } = true;
        public virtual bool AppearsInMultiPlayer { get; } = true;
        public virtual bool CanDrop { get; } = true;
        public virtual float Cooldown { get; } = 60f;
        public virtual bool EnigmaCompatible { get; } = true;
        public virtual bool IsBoss { get; } = false;
        public virtual bool IsLunar { get; } = false;

        public EquipmentIndex Index;

        public abstract void Init(ConfigFile config);

        protected void CreateLang()
        {
            LanguageAPI.Add("EQUIPMENT_" + EquipmentLangTokenName + "_NAME", EquipmentName);
            LanguageAPI.Add("EQUIPMENT_" + EquipmentLangTokenName + "_PICKUP", EquipmentPickupDesc);
            LanguageAPI.Add("EQUIPMENT_" + EquipmentLangTokenName + "_DESCRIPTION", EquipmentFullDescription);
            LanguageAPI.Add("EQUIPMENT_" + EquipmentLangTokenName + "_LORE", EquipmentLore);
        }

        public abstract ItemDisplayRuleDict CreateItemDisplayRules();

        protected void CreateEquipment()
        {
            EquipmentDef equipmentDef = new RoR2.EquipmentDef()
            {
                name = "EQUIPMENT_" + EquipmentLangTokenName,
                nameToken = "EQUIPMENT_" + EquipmentLangTokenName + "_NAME",
                pickupToken = "EQUIPMENT_" + EquipmentLangTokenName + "_PICKUP",
                descriptionToken = "EQUIPMENT_" + EquipmentLangTokenName + "_DESCRIPTION",
                loreToken = "EQUIPMENT_" + EquipmentLangTokenName + "_LORE",
                pickupModelPath = EquipmentModelPath,
                pickupIconPath = EquipmentIconPath,
                appearsInSinglePlayer = AppearsInSinglePlayer,
                appearsInMultiPlayer = AppearsInMultiPlayer,
                canDrop = CanDrop,
                cooldown = Cooldown,
                enigmaCompatible = EnigmaCompatible,
                isBoss = IsBoss,
                isLunar = IsLunar
            };
            var itemDisplayRules = CreateItemDisplayRules();
            ItemAPI.Add(new CustomEquipment(equipmentDef, itemDisplayRules));
            On.RoR2.EquipmentSlot.PerformEquipmentAction += PerformEquipmentAction;
        }
        private bool PerformEquipmentAction(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, RoR2.EquipmentSlot self, EquipmentIndex equipmentIndex)
        {
            if (equipmentIndex == Index)
            {
                return ActivateEquipment(self);
            } else
            {
                return orig(self, equipmentIndex);
            }
        }
        protected abstract bool ActivateEquipment(EquipmentSlot slot);

        public abstract void Hooks();
    }
}
