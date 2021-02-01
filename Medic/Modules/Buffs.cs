using R2API;
using RoR2;
using UnityEngine;

namespace Medic.Modules
{
    public static class Buffs
    {

        // UBERCHARGE
        public static BuffIndex godUberBuff;
        public static BuffIndex critUberBuff;
        public static BuffIndex resUberBuff;
        public static BuffIndex healUberBuff;

        public static void RegisterBuffs()
        {
            BuffDef godUberDef = new BuffDef
            {
                name = "ÜberCharge!\nInvulnerable!",
                iconPath = "textures/itemicons/texMaskIcon",
                buffColor = Color.blue,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff godUber = new CustomBuff(godUberDef);
            godUberBuff = BuffAPI.Add(godUber);

            BuffDef critUberDef = new BuffDef
            {
                name = "ÜberCharge!\nDamage Boost!",
                iconPath = "textures/itemicons/texMaskIcon",
                buffColor = Color.yellow,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff critUber = new CustomBuff(critUberDef);
            critUberBuff = BuffAPI.Add(critUber);

            BuffDef resUberDef = new BuffDef
            {
                name = "ÜberCharge!\nResistance!",
                iconPath = "textures/itemicons/texMaskIcon",
                buffColor = Color.magenta,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff resUber = new CustomBuff(resUberDef);
            resUberBuff = BuffAPI.Add(resUber);

            BuffDef healUberDef = new BuffDef
            {
                name = "ÜberCharge!\nSuperhealing!",
                iconPath = "textures/itemicons/texMaskIcon",
                buffColor = Color.green,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff healUber = new CustomBuff(healUberDef);
            healUberBuff = BuffAPI.Add(healUber);
        }
    }
}
