using R2API;
using RoR2;
using UnityEngine;

namespace ROR2_SaxtonHale.Modules
{
    public static class Buffs
    {
        public static BuffIndex scaredDebuff;

        public static BuffIndex weighdownBuff;
        public static BuffIndex rageBuff;

        public static void RegisterBuffs()
        {
            BuffDef scaredDef = new BuffDef
            {
                name = "Scared!\nUnable to attack!",
                //iconPath = "Textures/BuffIcons/texBuffGenericShield",
                //buffColor = SaxtonHale.characterColor,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None
            };
            CustomBuff scared = new CustomBuff(scaredDef);
            scaredDebuff = BuffAPI.Add(scared);

            BuffDef weighdownDef = new BuffDef
            {
                name = "Weighdown!\nImmunity to knockback.",
                iconPath = "Textures/BuffIcons/texBuffGenericShield",
                //buffColor = SaxtonHale.characterColor,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff weighdown = new CustomBuff(weighdownDef);
            weighdownBuff = BuffAPI.Add(weighdown);
        }
    }
}
