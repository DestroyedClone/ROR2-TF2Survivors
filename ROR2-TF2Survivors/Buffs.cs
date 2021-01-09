using R2API;
using RoR2;
using UnityEngine;

namespace ROR2_Scout
{
    public static class Buffs
    {
        public static BuffIndex milkedDebuff;
        public static BuffIndex markedDebuff;
        public static BuffIndex bonkBuff;
        public static BuffIndex critColaBuff;
        public static BuffIndex bonkBoyBuff;

        public static void RegisterBuffs()
        {
            BuffDef milkedDef = new BuffDef
            {
                name = "Mad Milked",
                iconPath = "textures/bufficons/texBuffBleedingIcon",
                buffColor = Color.white,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None
            };
            CustomBuff milked = new CustomBuff(milkedDef);
            milkedDebuff = BuffAPI.Add(milked);

            BuffDef markedDef = new BuffDef
            {
                name = "Marked For Death",
                iconPath = "textures/bufficons/texBuffDeathMarkIcon",
                buffColor = Color.white,
                canStack = false,
                isDebuff = true,
                eliteIndex = EliteIndex.None
            };
            CustomBuff marked = new CustomBuff(markedDef);
            markedDebuff = BuffAPI.Add(marked);

            BuffDef bonkDef = new BuffDef
            {
                name = "Bonk! Atomic Punch",
                iconPath = "Textures/BuffIcons/texBuffGenericShield",
                buffColor = Color.white,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff bonk = new CustomBuff(bonkDef);
            bonkBuff = BuffAPI.Add(bonk);

            BuffDef critColaDef = new BuffDef
            {
                name = "Crit-a-Cola",
                iconPath = "textures/bodyicons/ClayBruiserBody",
                buffColor = Color.white,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff critCola = new CustomBuff(critColaDef);
            critColaBuff = BuffAPI.Add(critCola);

            BuffDef bonkBoyDef = new BuffDef
            {
                name = "Bonk Boy",
                iconPath = "textures/bodyicons/GreaterWispBody",
                buffColor = Color.yellow,
                canStack = false,
                isDebuff = false,
                eliteIndex = EliteIndex.None
            };
            CustomBuff bonkBoy = new CustomBuff(bonkBoyDef);
            bonkBoyBuff = BuffAPI.Add(bonkBoy);
        }
    }
}
