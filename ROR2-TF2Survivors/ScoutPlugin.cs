using System;
using System.Linq;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Networking;
using KinematicCharacterController;

using static RoR2.SkillLocator;

namespace ROR2_Scout
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [R2APISubmoduleDependency(nameof(PrefabAPI), nameof(SurvivorAPI), nameof(LoadoutAPI), nameof(ItemAPI), nameof(DifficultyAPI), nameof(BuffAPI))]

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
    public class ScoutPlugin : BaseUnityPlugin
    {
        public const string ModVer = "1.0.0";
        public const string ModName = "Team Fortress 2 Scout";
        public const string ModGuid = "com.DestroyedClone.ROR2TF2SCOUT";

        public static ScoutPlugin instance;

        public static GameObject characterPrefab;

        public GameObject doppelganger;

        // clone this material to make our own with proper shader/properties
        public static Material commandoMat;

        public static event Action awake;
        public static event Action start;

        public static readonly Color characterColor = new Color(0.7176f, 0.098039f, 0.098039f);

        public ScoutPlugin()
        {
            awake += ScoutPlugin_Load;
            start += ScoutPlugin_LoadStart;
        }

        private void ScoutPlugin_Load()
        {
            instance = this;

            Modules.Prefabs.CreatePrefabs(); // create body and display prefabs
            characterPrefab = Modules.Prefabs.scoutPrefab; // cache this for other mods to use it
            //Modules.ItemDisplays.RegisterDisplays(); // add item displays(pain)
            Modules.States.RegisterStates(); // register states
            Modules.Skills.SetupSkills(Modules.Prefabs.scoutPrefab);
            Modules.Survivors.RegisterSurvivors(); // register them into the body catalog
            Modules.Skins.RegisterSkins(); // add skins
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            //Modules.Effects.RegisterEffects(); // add and register custom effects
            //Modules.Unlockables.RegisterUnlockables(); // add unlockables
            Modules.Tokens.AddTokens(); // register name tokens

            CreateDoppelganger(); // artifact of vengeance

            Hook();
        }

        private void ScoutPlugin_LoadStart()
        {
            //Modules.Projectiles.LateSetup();
        }

        public void Awake()
        {
            Action awake = ScoutPlugin.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void Start()
        {
            Action start = ScoutPlugin.start;
            if (start == null)
            {
                return;
            }
            start();
        }

        private void Hook()
        {

        }

        private void CreateDoppelganger()
        {
            doppelganger = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster"), "ScoutMonsterMaster");
            doppelganger.GetComponent<CharacterMaster>().bodyPrefab = Modules.Prefabs.scoutPrefab;

            MasterCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(doppelganger);
            };
        }
    }
}
