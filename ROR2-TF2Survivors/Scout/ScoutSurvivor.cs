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
using KinematicCharacterController;
using ROR2_TF2Survivors.Base_Classes;
using static ROR2_TF2Survivors.Base_Classes.EquipmentBase;

namespace ROR2_TF2Survivors.Scout
{
    public class ScoutSurvivor
    {
        public static TF2Survivors instance;

        public static GameObject characterPrefab;

        public GameObject doppelganger;

        public static event Action Awake;
        public static event Action Start;

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
            if (victim)
            {
                if (victim.HasBuff(Buffs.bonkBuff))
                {
                    // We dont reject it so that the force component still applies
                    damageInfo.damage = 0f;
                    damageInfo.procCoefficient = 0f;
                    damageInfo.damageColorIndex = DamageColorIndex.Item;
                }
            }
            orig(self, damageInfo);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self)
            {
                if (self.HasBuff(Buffs.bonkBuff))
                {
                    self.attackSpeed = 0;
                }
            }
        }
        EquipmentIndex BonkIndex = EquipmentCatalog.FindEquipmentIndex("Bonk");
        EquipmentIndex ColaIndex = EquipmentCatalog.FindEquipmentIndex("Cola");

        private void GiveEquipmentOnBodyStart(CharacterBody obj)
        {
            if (obj.bodyIndex == SurvivorCatalog.GetBodyIndexFromSurvivorIndex(SurvivorIndex.Commando))
            {
                switch (obj.GetComponentsInChildren<GenericSkill>().FirstOrDefault(x => x.skillFamily.variants[0].skillDef.skillName == "SCOUTSURVIVOR_EQUIPMENT_NONE_NAME").skillDef.skillName)
                {
                    case "SCOUTSURVIVOR_EQUIPMENT_BONK_NAME":
                        SafeGiveEquipment(obj, Equipments.BonkEquipment.);
                        break;
                    case "SCOUTSURVIVOR_EQUIPMENT_COLA_NAME":
                        SafeGiveEquipment(obj, ColaIndex);
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


        private void GiveEquipment2(On.RoR2.Run.orig_Start orig, Run self)
        {
            orig(self);
            if (self.isServer)
            {
                var playerList = PlayerCharacterMasterController.instances;
                foreach (var player in playerList)
                {
                    var body = player.master.GetBody();
                    if (body)
                    {
                        var inventory = body.inventory;
                        if (inventory)
                        {
                            inventory.SetEquipmentIndex(EquipmentIndex.AffixBlue);
                        }
                    }
                }
            }
        }

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
            doppelganger = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterMasters/LoaderMonsterMaster"), "SaxtonHaleMonsterMaster");

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
            LoadoutAPI.AddSkill(typeof(FistsAttack));

            SkillDef mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(FistsAttack));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = false;
            mySkillDef.canceledFromSprinting = false;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            //mySkillDef.icon = Modules.Assets.icon1;
            mySkillDef.skillDescriptionToken = "SAXTONHALE_PRIMARY_FISTS_DESCRIPTION";
            mySkillDef.skillName = "SAXTONHALE_PRIMARY_FISTS_NAME";
            mySkillDef.skillNameToken = "SAXTONHALE_PRIMARY_FISTS_NAME";

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
        }


        public class ScoutController : MonoBehaviour
        {
            float timeAirborne = 0f;
            uint killsInDuration = 0;

        }
    }
}
