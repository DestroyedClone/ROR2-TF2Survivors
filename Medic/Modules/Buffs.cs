using R2API;
using RoR2;
using UnityEngine;

namespace Medic.Modules
{
    public static class Buffs
    {

        // UBERCHARGE - ALLY
        public static BuffIndex godUberBuff;
        public static BuffIndex critUberBuff;
        public static BuffIndex resUberBuff;
        public static BuffIndex healUberBuff;

        // UBERCHARGE - ENEMY
        public static BuffIndex godUberEnemyBuff;
        public static BuffIndex critUberEnemyBuff;
        public static BuffIndex resUberEnemyBuff;
        public static BuffIndex healUberEnemyBuff;

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

            BuffDef godUberEnemyDef = new BuffDef
            {
                name = "ÜberCharge!\nVulnerable!!",
                iconPath = "textures/itemicons/texMaskIcon",
                buffColor = Color.white,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None
            };
            CustomBuff godUberEnemy = new CustomBuff(godUberEnemyDef);
            godUberEnemyBuff = BuffAPI.Add(godUberEnemy);

            BuffDef critUberEnemyDef = new BuffDef
            {
                name = "ÜberCharge!\nDamage Weakened!",
                iconPath = "textures/itemicons/texMaskIcon",
                buffColor = Color.white,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None
            };
            CustomBuff critUberEnemy = new CustomBuff(critUberEnemyDef);
            critUberEnemyBuff = BuffAPI.Add(critUberEnemy);

            BuffDef resUberEnemyDef = new BuffDef
            {
                name = "ÜberCharge!\nProlonged Debuffs!",
                iconPath = "textures/itemicons/texMaskIcon",
                buffColor = Color.white,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None
            };
            CustomBuff resUberEnemy = new CustomBuff(resUberEnemyDef);
            resUberEnemyBuff = BuffAPI.Add(resUberEnemy);

            BuffDef healUberEnemyDef = new BuffDef
            {
                name = "ÜberCharge!\nHealing Hurts!!",
                iconPath = "textures/itemicons/texMaskIcon",
                buffColor = Color.white,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None
            };
            CustomBuff healUberEnemy = new CustomBuff(healUberEnemyDef);
            healUberEnemyBuff = BuffAPI.Add(healUberEnemy);
        }
    }
}
