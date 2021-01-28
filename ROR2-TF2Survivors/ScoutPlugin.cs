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

    public class ScoutPlugin : BaseUnityPlugin
    {
        public const string ModVer = "1.0.0";
        public const string ModName = "Team Fortress 2 Scout";
        public const string ModGuid = "com.DestroyedClone.ROR2TF2SCOUT";

        public static ScoutPlugin instance;

        public static event Action AwakeEvent;
        public static event Action StartEvent;

        public ScoutPlugin()
        {
            AwakeEvent += ScoutPlugin_Load;
            StartEvent += ScoutPlugin_LoadStart;
        }

        private void ScoutPlugin_Load()
        {
            instance = this;
        }

        private void ScoutPlugin_LoadStart()
        {
        }

        public void Awake()
        {
            Action awake = ScoutPlugin.AwakeEvent;
            if (awake == null)
            {
                return;
            }
            awake();
        }
        public void Start()
        {
            Action start = ScoutPlugin.StartEvent;
            if (start == null)
            {
                return;
            }
            start();
        }

        public static GameObject characterPrefab;

        public GameObject doppelganger;

        public static readonly Color characterColor = Color.yellow;

        public SkillLocator skillLocator;
        public void Init()
        {
            CreatePrefab();
            //CreateDisplayPrefab();
            RegisterCharacter();
            Modules.Buffs.RegisterBuffs();
            CreateDoppelganger();
            //CreateBossPrefab();

            Hooks();
        }

        private void Hooks()
        {
            //On.RoR2.Run.Start += GiveEquipment;
            RoR2.CharacterBody.onBodyStartGlobal += GiveEquipmentOnBodyStart;
            On.RoR2.HealthComponent.TakeDamage += HC_TakeDamage;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void HC_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            var victim = self.body;
            var teamComponent = victim.teamComponent;
            var attacker = damageInfo.attacker.gameObject?.GetComponent<CharacterBody>();
            var attackerTeamComponent = attacker.teamComponent;
            if (victim)
            {
                if (victim.HasBuff(Modules.Buffs.bonkBuff))
                {
                    // We dont reject it so that the force component still applies
                    damageInfo.damageType |= (DamageType.NonLethal | DamageType.Silent);
                    damageInfo.damage = 0f;
                    damageInfo.procCoefficient = 0f;
                    damageInfo.damageColorIndex = DamageColorIndex.Item;
                }
                if (victim.HasBuff(Modules.Buffs.critColaBuff))
                {
                    damageInfo.damage *= 1.5f;
                }
            }
            if (attacker)
            {
                if (attacker.HasBuff(Modules.Buffs.critColaBuff))
                {
                    damageInfo.damage *= 1.35f;
                }
            }

            orig(self, damageInfo);

            if (attacker)
            {
                if (victim && teamComponent && victim.HasBuff(Modules.Buffs.milkedDebuff) && attackerTeamComponent && attackerTeamComponent.teamIndex != teamComponent.teamIndex)
                {
                    attacker.healthComponent?.Heal(damageInfo.damage*0.05f, default);
                }
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self)
            {
                if (self.HasBuff(Modules.Buffs.bonkBuff))
                {
                    self.attackSpeed = 0;

                    SkillLocator skillLocator = self.skillLocator;
                    if (skillLocator)
                    {
                        skillLocator.DeductCooldownFromAllSkillsServer(-1);
                    }
                }
            }
        }


        private void GiveEquipmentOnBodyStart(CharacterBody obj)
        {
            if (obj.bodyIndex == SurvivorCatalog.GetBodyIndexFromSurvivorIndex(SurvivorIndex.Commando))
            {
                switch (obj.GetComponentsInChildren<GenericSkill>().FirstOrDefault(x => x.skillFamily.variants[0].skillDef.skillName == "SCOUTSURVIVOR_EQUIPMENT_NONE_NAME").skillDef.skillName)
                {
                    case "SCOUTSURVIVOR_EQUIPMENT_BONK_NAME":
                        SafeGiveEquipment(obj, Equipments.BonkEquipment.Index);
                        break;
                    case "SCOUTSURVIVOR_EQUIPMENT_COLA_NAME":
                        SafeGiveEquipment(obj, Equipments.ColaEquipment.Index);
                        break;
                }
            }
        }

        private void SafeGiveEquipment(CharacterBody characterBody, EquipmentIndex equipmentIndex)
        {
            if (NetworkServer.active)
            {
                var inventory = characterBody.inventory;
                if (inventory && inventory.GetEquipmentIndex() == EquipmentIndex.None)
                {
                    inventory.SetEquipmentIndex(equipmentIndex);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        private static void CreatePrefab()
        {
            characterPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), "TF2Scout");

            characterPrefab.GetComponent<NetworkIdentity>().localPlayerAuthority = true;

            //GameObject model = CreateModel(characterPrefab, 0);

            CharacterDirection characterDirection = characterPrefab.GetComponent<CharacterDirection>();
            characterDirection.moveVector = Vector3.zero;
            //characterDirection.targetTransform = gameObject.transform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            //characterDirection.modelAnimator = model.GetComponentInChildren<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;

            CharacterBody bodyComponent = characterPrefab.GetComponent<CharacterBody>();
            bodyComponent.bodyIndex = -1;
            bodyComponent.name = "Scout";
            bodyComponent.baseNameToken = "SCOUT_NAME";
            bodyComponent.subtitleNameToken = "SCOUT_SUBTITLE";
            bodyComponent.bodyFlags = CharacterBody.BodyFlags.IgnoreFallDamage | CharacterBody.BodyFlags.ImmuneToExecutes;
            bodyComponent.rootMotionInMainState = false;
            bodyComponent.mainRootSpeed = 0;
            bodyComponent.baseMaxHealth = 90;
            bodyComponent.levelMaxHealth = 15;
            bodyComponent.baseRegen = 0.5f;
            bodyComponent.levelRegen = 0.25f;
            bodyComponent.baseMaxShield = 0;
            bodyComponent.levelMaxShield = 0;
            bodyComponent.baseMoveSpeed = 12;
            bodyComponent.levelMoveSpeed = 0;
            bodyComponent.baseAcceleration = 80;
            bodyComponent.baseJumpPower = 15;
            bodyComponent.levelJumpPower = 0;
            bodyComponent.baseDamage = 15;
            bodyComponent.levelDamage = 1.5f;
            bodyComponent.baseAttackSpeed = 1;
            bodyComponent.levelAttackSpeed = 0;
            bodyComponent.baseCrit = 1;
            bodyComponent.levelCrit = 0;
            bodyComponent.baseArmor = 15;
            bodyComponent.levelArmor = 0f;
            bodyComponent.baseJumpCount = 2; //change
            bodyComponent.sprintingSpeedMultiplier = 1f; //1.45
            bodyComponent.wasLucky = false;
            bodyComponent.hideCrosshair = false;
            //bodyComponent.crosshairPrefab = Resources.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair");
            //bodyComponent.aimOriginTransform = gameObject3.transform;
            bodyComponent.hullClassification = HullClassification.Human;
            //bodyComponent.portraitIcon = Modules.Assets.charPortrait;
            bodyComponent.isChampion = false;
            bodyComponent.currentVehicle = null;
            bodyComponent.skinIndex = 0U;
            bodyComponent.preferredPodPrefab = null;
            /*
            LoadoutAPI.AddSkill(typeof(States.PaladinMain));
            LoadoutAPI.AddSkill(typeof(States.SpawnState));
            LoadoutAPI.AddSkill(typeof(States.Emotes.BaseEmote));
            LoadoutAPI.AddSkill(typeof(States.Emotes.PraiseTheSun));
            LoadoutAPI.AddSkill(typeof(States.Emotes.PointDown));
            LoadoutAPI.AddSkill(typeof(States.Emotes.Rest));
            */

            //var stateMachine = bodyComponent.GetComponent<EntityStateMachine>();
            //stateMachine.mainStateType = new SerializableEntityStateType(typeof(States.PaladinMain));
            //stateMachine.initialStateType = new SerializableEntityStateType(typeof(States.SpawnState));

            CharacterMotor characterMotor = characterPrefab.GetComponent<CharacterMotor>();
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            characterMotor.characterDirection = characterDirection;
            characterMotor.muteWalkMotion = false;
            characterMotor.mass = 100f;
            characterMotor.airControl = 0.25f;
            characterMotor.disableAirControlUntilCollision = false;
            characterMotor.generateParametersOnAwake = true;

            CameraTargetParams cameraTargetParams = characterPrefab.GetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<CameraTargetParams>().cameraParams;
            cameraTargetParams.cameraPivotTransform = null;
            cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            cameraTargetParams.recoil = Vector2.zero;
            cameraTargetParams.idealLocalCameraPos = Vector3.zero;
            cameraTargetParams.dontRaycastToPivot = false;

            //ChildLocator childLocator = model.GetComponent<ChildLocator>();

            TeamComponent teamComponent = null;
            if (characterPrefab.GetComponent<TeamComponent>() != null) teamComponent = characterPrefab.GetComponent<TeamComponent>();
            else teamComponent = characterPrefab.GetComponent<TeamComponent>();
            teamComponent.hideAllyCardDisplay = false;
            teamComponent.teamIndex = TeamIndex.None;

            HealthComponent healthComponent = characterPrefab.GetComponent<HealthComponent>();
            healthComponent.shield = 0f;
            healthComponent.barrier = 0f;
            healthComponent.magnetiCharge = 0f;
            healthComponent.body = null;
            healthComponent.dontShowHealthbar = false;
            healthComponent.globalDeathEventChanceCoefficient = 1f;

            characterPrefab.GetComponent<Interactor>().maxInteractionDistance = 5f; //3 is default
            characterPrefab.GetComponent<InteractionDriver>().highlightInteractor = true;

            CharacterDeathBehavior characterDeathBehavior = characterPrefab.GetComponent<CharacterDeathBehavior>();
            characterDeathBehavior.deathStateMachine = characterPrefab.GetComponent<EntityStateMachine>();
            //characterDeathBehavior.deathState = new SerializableEntityStateType(typeof(GenericCharacterDeath));

            SfxLocator sfxLocator = characterPrefab.GetComponent<SfxLocator>();
            //sfxLocator.deathSound = Sounds.DeathSound;
            sfxLocator.barkSound = "";
            sfxLocator.openSound = "";
            sfxLocator.landingSound = "Play_char_land";
            sfxLocator.fallDamageSound = "Play_char_land_fall_damage";
            sfxLocator.aliveLoopStart = "";
            sfxLocator.aliveLoopStop = "";

            Rigidbody rigidbody = characterPrefab.GetComponent<Rigidbody>();
            rigidbody.mass = 100f;
            rigidbody.drag = 0f;
            rigidbody.angularDrag = 0f;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.interpolation = RigidbodyInterpolation.None;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rigidbody.constraints = RigidbodyConstraints.None;

            CapsuleCollider capsuleCollider = characterPrefab.GetComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = false;
            capsuleCollider.material = null;
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            //capsuleCollider.radius = 0.5f;
            //capsuleCollider.height = 1.82f;
            //capsuleCollider.direction = 1;

            KinematicCharacterMotor kinematicCharacterMotor = characterPrefab.GetComponent<KinematicCharacterMotor>();
            kinematicCharacterMotor.CharacterController = characterMotor;
            kinematicCharacterMotor.Capsule = capsuleCollider;
            kinematicCharacterMotor.Rigidbody = rigidbody;
            kinematicCharacterMotor.DetectDiscreteCollisions = false;
            kinematicCharacterMotor.GroundDetectionExtraDistance = 0f;
            kinematicCharacterMotor.MaxStepHeight = 0.2f;
            kinematicCharacterMotor.MinRequiredStepDepth = 0.1f;
            kinematicCharacterMotor.MaxStableSlopeAngle = 55f;
            kinematicCharacterMotor.MaxStableDistanceFromLedge = 0.5f;
            kinematicCharacterMotor.PreventSnappingOnLedges = false;
            kinematicCharacterMotor.MaxStableDenivelationAngle = 55f;
            kinematicCharacterMotor.RigidbodyInteractionType = RigidbodyInteractionType.None;
            kinematicCharacterMotor.PreserveAttachedRigidbodyMomentum = true;
            kinematicCharacterMotor.HasPlanarConstraint = false;
            kinematicCharacterMotor.PlanarConstraintAxis = Vector3.up;
            kinematicCharacterMotor.StepHandling = StepHandlingMethod.None;
            kinematicCharacterMotor.LedgeHandling = true;
            kinematicCharacterMotor.InteractiveRigidbodyHandling = true;
            kinematicCharacterMotor.SafeMovement = false;

            /*
            FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");
            */

            //PhysicMaterial physicMat = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RagdollController>().bones[1].GetComponent<Collider>().material;

            characterPrefab.AddComponent<ScoutController>();
        }

        private void RegisterCharacter()
        {
            //characterDisplay.AddComponent<NetworkIdentity>();

            //string unlockString = "PALADIN_UNLOCKABLE_REWARD_ID";
            //if (Modules.Config.forceUnlock.Value) unlockString = "";

            SurvivorDef survivorDef = new SurvivorDef
            {
                name = "SCOUTSURVIVOR_NAME",
                //unlockableName = unlockString,
                descriptionToken = "SCOUTSURVIVOR_DESCRIPTION",
                primaryColor = characterColor,
                bodyPrefab = characterPrefab,
                //displayPrefab = characterDisplay,
                outroFlavorToken = "SCOUTSURVIVOR_OUTRO_FLAVOR"
            };

            SurvivorAPI.AddSurvivor(survivorDef);

            BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(characterPrefab);
            };

            characterPrefab.tag = "Player";
        }
        private void CreateDoppelganger()
        {
            doppelganger = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/CommandoMonsterMaster"), "ScoutMonsterMaster");

            MasterCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(doppelganger);
            };

            CharacterMaster component = doppelganger.GetComponent<CharacterMaster>();
            component.bodyPrefab = characterPrefab;
        }


    }
}
