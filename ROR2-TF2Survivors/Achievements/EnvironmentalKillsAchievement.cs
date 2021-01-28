using UnityEngine;
using UnityEngine.Networking;
using R2API;
using R2API.Utils;
using RoR2;
using System;

namespace ROR2_Scout.Achievements
{
    public class EnvironmnetalKills : ModdedUnlockableAndAchievement<CustomSpriteProvider> //TODO: Taken from SuicideHermitCrabsAchievement
    {
        public override String AchievementIdentifier { get; } = "SCOUTSURVIVOR_FORCENATUREUNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "SCOUTSURVIVOR_FORCENATUREUNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "SCOUTSURVIVOR_FORCENATUREUNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "SCOUTSURVIVOR_FORCENATUREUNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "SCOUTSURVIVOR_FORCENATUREUNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "SCOUTSURVIVOR_FORCENATUREUNLOCKABLE_UNLOCKABLE_NAME";
        protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Paladin:Assets/Paladin/Icons/texPaladinAchievement.png");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("TF2Scout");
        }

        public override void OnInstall()
        {
            base.OnInstall();
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            GlobalEventManager.onCharacterDeathGlobal -= GlobalEventManager_onCharacterDeathGlobal;
//
            RoR2.Stats.StatDef statDef = new RoR2.Stats.StatDef("General Suicides", RoR2.Stats.StatRecordType.Max, RoR2.Stats.StatDataType.ULong, RoR2.Stats.StatDef.DisplayValueFormatterDelegate );

        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport)
        {
            if (!damageReport.victimBody)
            {
                return;
            }
            GameObject inflictor = damageReport.damageInfo.inflictor;
            if (!inflictor || !inflictor.GetComponent<MapZone>())
            {
                return;
            }
            if (damageReport.victimBody.teamComponent.teamIndex != TeamIndex.Player)
            {
                PlayerStatsComponent masterPlayerStatsComponent = base.networkUser.masterPlayerStatsComponent;
                if (masterPlayerStatsComponent)
                {
                    masterPlayerStatsComponent.currentStats.PushStatValue(StatDef.suicideHermitCrabsAchievementProgress, 1UL);
                }
            }
        }
    }
}
