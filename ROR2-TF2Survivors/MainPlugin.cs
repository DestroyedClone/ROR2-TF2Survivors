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

        public static event Action AwakeEvent;
        public static event Action StartEvent;

        public TF2Survivors()
        {
            AwakeEvent += TF2Survivors_Load;
            StartEvent += TF2Survivors_LoadStart;
        }

        private void TF2Survivors_Load()
        {
            instance = this;
        }

        private void TF2Survivors_LoadStart()
        {
        }

        public void Awake()
        {
            Action awake = TF2Survivors.AwakeEvent;
            if (awake == null)
            {
                return;
            }
            awake();
        }
        public void Start()
        {
            Action start = TF2Survivors.StartEvent;
            if (start == null)
            {
                return;
            }
            start();
        }

    }
}
