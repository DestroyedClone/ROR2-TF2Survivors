using System;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;
using KinematicCharacterController;

namespace ROR2_TF2Survivors
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [R2APISubmoduleDependency(nameof(PrefabAPI), nameof(SurvivorAPI), nameof(LoadoutAPI), nameof(ItemAPI), nameof(DifficultyAPI), nameof(BuffAPI))]

    public class TF2Survivors : BaseUnityPlugin
    {
        public const string ModVer = "1.0.0";
        public const string ModName = "Team Fortress 2 Survivors";
        public const string ModGuid = "com.DestroyedClone.RORTF2Survivors";

        public static TF2Survivors instance;

        public static event Action awake;
        public static event Action start;

        public TF2Survivors()
        {
            awake += TF2Survivors_Load;
            start += TF2Survivors_LoadStart;
        }

        private void TF2Survivors_Load()
        {
            instance = this;

            Modules.Assets.PopulateAssets();
            Modules.Config.ReadConfig();

            CreatePrefab();
            CreateDisplayPrefab();
            RegisterCharacter();
            Modules.Skins.RegisterSkins();
            Modules.Buffs.RegisterBuffs();
            Modules.Projectiles.RegisterProjectiles();
            Modules.ItemDisplays.RegisterDisplays();
            Modules.Effects.RegisterEffects();
            Modules.Unlockables.RegisterUnlockables();
            Modules.Tokens.AddTokens();

            CreateDoppelganger();

            Hook();
        }

        private void TF2Survivors_LoadStart()
        {
        }

        public void Awake()
        {
            Action awake = TF2Survivors.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }
        public void Start()
        {
            Action start = TF2Survivors.start;
            if (start == null)
            {
                return;
            }
            start();
        }

    }
}
