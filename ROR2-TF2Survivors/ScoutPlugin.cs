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
            Buffs.RegisterBuffs();
            //NemItemDisplays.RegisterDisplays();
            //NemforcerSkins.RegisterSkins();
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
                if (victim.HasBuff(Buffs.bonkBuff))
                {
                    // We dont reject it so that the force component still applies
                    damageInfo.damageType |= (DamageType.NonLethal | DamageType.Silent);
                    damageInfo.damage = 0f;
                    damageInfo.procCoefficient = 0f;
                    damageInfo.damageColorIndex = DamageColorIndex.Item;
                }
                if (victim.HasBuff(Buffs.critColaBuff))
                {
                    damageInfo.damage *= 1.5f;
                }
            }
            if (attacker)
            {
                if (attacker.HasBuff(Buffs.critColaBuff))
                {
                    damageInfo.damage *= 1.35f;
                }
            }

            orig(self, damageInfo);

            if (attacker)
            {
                if (victim && teamComponent && victim.HasBuff(Buffs.milkedDebuff) && attackerTeamComponent && attackerTeamComponent.teamIndex != teamComponent.teamIndex)
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
                if (self.HasBuff(Buffs.bonkBuff))
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
            bodyComponent.name = "ScoutSurvivor";
            bodyComponent.baseNameToken = "SAXTONHALE_NAME";
            bodyComponent.subtitleNameToken = "SAXTONHALE_SUBTITLE";
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

            SkillSetup();

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
        private void SkillSetup()
        {
            foreach (GenericSkill obj in characterPrefab.GetComponentsInChildren<GenericSkill>())
            {
                BaseUnityPlugin.DestroyImmediate(obj);
            }

            skillLocator = characterPrefab.GetComponent<SkillLocator>();

            PassiveSetup();
            PrimarySetup();
            SecondarySetup();
            UtilitySetup();
            SpecialSetup();
            EquipmentSetup();
        }
        private void PassiveSetup()
        {
            skillLocator.passiveSkill.enabled = true;
            skillLocator.passiveSkill.skillNameToken = "SCOUTSURVIVOR_PASSIVE_NAME";
            skillLocator.passiveSkill.skillDescriptionToken = "SCOUTSURVIVOR_PASSIVE_DESC";
            skillLocator.passiveSkill.icon = Resources.Load<Sprite>("textures/itemicons/texAffixGreenIcon.png");
        }

        private void PrimarySetup()
        {
            LoadoutAPI.AddSkill(typeof(States.FireScattergun));

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.FireScattergun));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = true;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            //mySkillDef.icon = Modules.Assets.icon2;
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_PRIMARY_SCATTERGUN_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_PRIMARY_SCATTERGUN_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_PRIMARY_SCATTERGUN_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.primary = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.primary.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.primary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            LoadoutAPI.AddSkill(typeof(States.FireForceNature));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.FireForceNature));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            //mySkillDef.icon = Modules.Assets.icon2b;
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_PRIMARY_FORCENATURE_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_PRIMARY_FORCENATURE_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_PRIMARY_FORCENATURE_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",//PALADIN_LIGHTNINGSPEARUNLOCKABLE_REWARD_ID
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }
        private void SecondarySetup()
        {
            LoadoutAPI.AddSkill(typeof(States.FireScoutPistol));

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.FireScoutPistol));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 12;
            mySkillDef.baseRechargeInterval = 3f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = true;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 12;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            //mySkillDef.icon = Modules.Assets.icon2;
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_SECONDARY_PISTOL_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_SECONDARY_PISTOL_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_SECONDARY_PISTOL_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.secondary = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.secondary.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.secondary.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            LoadoutAPI.AddSkill(typeof(States.FireCleaver));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.FireCleaver));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("textures/itemicons/texSawmerangIcon");
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_SECONDARY_CLEAVER_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_SECONDARY_CLEAVER_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_SECONDARY_CLEAVER_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_BLEEDING"
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            LoadoutAPI.AddSkill(typeof(States.FireMilk));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.FireMilk));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 12f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("textures/itemicons/texLifestealOnHitIcon");
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_SECONDARY_MILK_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_SECONDARY_MILK_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_SECONDARY_MILK_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_MILKING"
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }
        private void UtilitySetup()
        {
            LoadoutAPI.AddSkill(typeof(States.FireMarkFan));

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.FireMarkFan));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("textures/itemicons/texDeathMarkIcon");
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_UTILITY_MARKDEATH_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_UTILITY_MARKDEATH_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_UTILITY_MARKDEATH_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_DEATHMARKING",
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.utility = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.utility.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.utility.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            LoadoutAPI.AddSkill(typeof(States.FireBleedBat));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.FireBleedBat));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("textures/bufficons/texBuffBleedingIcon");
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_UTILITY_BLEEDBAT_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_UTILITY_BLEEDBAT_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_UTILITY_BLEEDBAT_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_BLEEDING"
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            LoadoutAPI.AddSkill(typeof(States.FireSandman));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.FireSandman));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 2f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("textures/itemicons/texStunGrenadeIcon");
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_UTILITY_SANDMAN_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_UTILITY_SANDMAN_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_UTILITY_SANDMAN_NAME";
            mySkillDef.keywordTokens = new string[] {
                "KEYWORD_STUNNING"
            };

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }
        private void SpecialSetup()
        {
            LoadoutAPI.AddSkill(typeof(States.ActivateRage));

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.ActivateRage));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 60f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            //mySkillDef.icon = Modules.Assets.icon1;
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_SPECIAL_FF2_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_SPECIAL_FF2_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_SPECIAL_FF2_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            skillLocator.special = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            newFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamily);
            skillLocator.special.SetFieldValue("_skillFamily", newFamily);
            SkillFamily skillFamily = skillLocator.special.skillFamily;

            skillFamily.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }
        private void EquipmentSetup()
        {
            LoadoutAPI.AddSkill(typeof(States.EquipmentNone));

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.EquipmentNone));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 0;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 0;
            mySkillDef.requiredStock = 0;
            mySkillDef.shootDelay = 0f;
            mySkillDef.stockToConsume = 0;
            mySkillDef.icon = Resources.Load<Sprite>("textures/bodyicons/texUnidentifiedKillerIcon");
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_EQUIPMENT_NONE_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_EQUIPMENT_NONE_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_EQUIPMENT_NONE_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            var passive = characterPrefab.AddComponent<GenericSkill>();
            SkillFamily skillFamily = ScriptableObject.CreateInstance<SkillFamily>();
            skillFamily.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(skillFamily);
            SkillFamily skillFamilyPassive = passive.skillFamily;
            skillFamilyPassive.variants[0] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            LoadoutAPI.AddSkill(typeof(States.EquipmentBonk));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.EquipmentBonk));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("textures/bufficons/texBuffAffixWhite");
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_EQUIPMENT_BONK_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_EQUIPMENT_BONK_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_EQUIPMENT_BONK_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };

            LoadoutAPI.AddSkill(typeof(States.EquipmentCola));

            mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(States.EquipmentCola));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 6f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Skill;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = false;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("textures/itemicons/texSawmerangIcon");
            mySkillDef.skillDescriptionToken = "SCOUTSURVIVOR_EQUIPMENT_COLA_DESCRIPTION";
            mySkillDef.skillName = "SCOUTSURVIVOR_EQUIPMENT_COLA_NAME";
            mySkillDef.skillNameToken = "SCOUTSURVIVOR_EQUIPMENT_COLA_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);

            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            };
        }


        public class ScoutController : MonoBehaviour
        {
            CharacterBody scoutBody;
            SkillLocator scoutSkillLocator;
            bool currentlyBuffed = false;
            readonly float[] rechargeStopwatches = //rd
            {
                0f,0f,0f,0f
            };
            readonly int[] stocks = //rd
{
                0,0,0,0
            };

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "UnityEngine")]
            void Awake()
            {
                scoutBody = gameObject.GetComponent<CharacterBody>();
                scoutSkillLocator = scoutBody?.masterObject.GetComponent<SkillLocator>();
                if (!scoutBody || !scoutSkillLocator)
                {
                    Chat.AddMessage("ScoutController failed: missing objects");
                    enabled = false;
                }
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "UnityEngine")]
            void FixedUpdate()
            {
                if (scoutBody)
                {
                    if (scoutBody.HasBuff(Buffs.bonkBuff))
                    {
                        if (!currentlyBuffed) // so it happens once
                        {
                            // TODO: Network this
                            // Referenced from SkillLocator's DeductCooldownFromAllSkillsAuthority
                            for (int i = 0; i < scoutSkillLocator.allSkills.Length; i++)
                            {
                                GenericSkill genericSkill = scoutSkillLocator.allSkills[i];

                                rechargeStopwatches[i] = genericSkill.rechargeStopwatch;
                                stocks[i] = genericSkill.stock;

                                genericSkill.rechargeStopwatch = -1000f;
                                genericSkill.stock = -1000;
                                currentlyBuffed = true;
                            }
                        }
                    } else
                    {
                        if (currentlyBuffed)
                        { //reset the stocks back to normal
                            for (int i = 0; i < scoutSkillLocator.allSkills.Length; i++)
                            {
                                GenericSkill genericSkill = scoutSkillLocator.allSkills[i];

                                genericSkill.rechargeStopwatch = rechargeStopwatches[i];
                                genericSkill.stock = stocks[i];
                                currentlyBuffed = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
