using System;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace Medic
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [R2APISubmoduleDependency(nameof(PrefabAPI), nameof(SurvivorAPI), nameof(LoadoutAPI), nameof(ItemAPI), nameof(DifficultyAPI), nameof(BuffAPI))]
    public class MedicPlugin : BaseUnityPlugin
    {
        public const string ModVer = "1.0.0";
        public const string ModName = "TF2 Saxton Hale";
        public const string ModGuid = "com.DestroyedClone.ROR2TF2SAXTONHALE";

        public static MedicPlugin instance;

        public static GameObject characterPrefab;

        public GameObject doppelganger;

        // clone this material to make our own with proper shader/properties
        public static Material commandoMat;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public static event Action awake;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public static event Action start;

        public static readonly Color characterColor = new Color(0.443f, 0.219f, 0.098f);

        public MedicPlugin()
        {
            awake += MedicPlugin_Load;
            start += MedicPlugin_LoadStart;
        }

        private void MedicPlugin_Load()
        {
            instance = this;

            // load assets and read config
            //Modules.Assets.PopulateAssets();
            //Modules.Config.ReadConfig();

            Modules.Prefabs.CreatePrefabs(); // create body and display prefabs
            characterPrefab = Modules.Prefabs.medicPrefab; // cache this for other mods to use it
            //Modules.ItemDisplays.RegisterDisplays(); // add item displays(pain)
            Modules.States.RegisterStates(); // register states
            Modules.Skills.SetupSkills(Modules.Prefabs.medicPrefab);
            Modules.Survivors.RegisterSurvivors(); // register them into the body catalog
            Modules.Skins.RegisterSkins(); // add skins
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs
            //Modules.Effects.RegisterEffects(); // add and register custom effects
            //Modules.Unlockables.RegisterUnlockables(); // add unlockables
            Modules.Tokens.AddTokens(); // register name tokens

            CreateDoppelganger(); // artifact of vengeance

            Hook();
        }

        private void MedicPlugin_LoadStart()
        {
        }

        public void Awake()
        {
            Action awake = MedicPlugin.awake;
            if (awake == null)
            {
                return;
            }
            awake();
        }

        public void Start()
        {
            Action start = MedicPlugin.start;
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
            doppelganger = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/LoaderMonsterMaster"), "HaleMonsterMaster");
            doppelganger.GetComponent<CharacterMaster>().bodyPrefab = Modules.Prefabs.halePrefab;

            MasterCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(doppelganger);
            };
        }
    }
}
