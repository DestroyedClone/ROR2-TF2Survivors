﻿using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace ROR2_Scout.Modules
{
    public static class Prefabs
    {
        public static GameObject scoutPrefab;
        //public static GameObject paladinDisplayPrefab;

        private static PhysicMaterial ragdollMaterial;

        public static void CreatePrefabs()
        {
            CreatePaladin();

            BodyCatalog.getAdditionalEntries += delegate (List<GameObject> list)
            {
                list.Add(scoutPrefab);
            };
        }

        private static void CreatePaladin()
        {
            scoutPrefab = CreatePrefab("RobPaladinBody", "mdlPaladin", new BodyInfo
            {
                armor = 0f,
                armorGrowth = 0f,
                bodyName = "ScoutBody",
                bodyNameToken = "SCOUT_NAME",
                characterPortrait = Assets.charPortrait,
                crosshair = Resources.Load<GameObject>("Prefabs/Crosshair/SimpleDotCrosshair"),
                damage = 15f,
                damageGrowth = 2.4f,
                healthGrowth = 64,
                healthRegen = 1.5f,
                jumpCount = 2,
                maxHealth = 160f,
                maxInteractionDistance = 5f,
                subtitleNameToken = "SCOUT_SUBTITLE"
            });

            /*SetupCharacterModel(paladinPrefab, new CustomRendererInfo[]
            {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = Modules.Skins.CreateMaterial("matPaladin", StaticValues.maxSwordGlow, Color.white)
                },
                new CustomRendererInfo
                {
                    childName = "Model",
                    material = Modules.Skins.CreateMaterial("matPaladin", 10, Color.white)
                }
            }, 1);*/

            //paladinDisplayPrefab = CreateDisplayPrefab("PaladinDisplay", paladinPrefab);

            // create hitboxes

            //GameObject model = scoutPrefab.GetComponent<ModelLocator>().modelTransform.gameObject;
            //ChildLocator childLocator = model.GetComponent<ChildLocator>();

            //Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SwordHitbox"), "Sword");
            //Modules.Helpers.CreateHitbox(model, childLocator.FindChild("LeapHitbox"), "LeapStrike");
            //Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SpinSlashHitbox"), "SpinSlash");
            //Modules.Helpers.CreateHitbox(model, childLocator.FindChild("SpinSlashLargeHitbox"), "SpinSlashLarge");
        }


        /*public static GameObject CreateDisplayPrefab(string modelName, GameObject prefab)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody"), modelName + "Prefab");

            GameObject model = CreateModel(newPrefab, modelName);
            Transform modelBaseTransform = SetupModel(newPrefab, model.transform);

            model.AddComponent<CharacterModel>().baseRendererInfos = prefab.GetComponentInChildren<CharacterModel>().baseRendererInfos;

            return model.gameObject;
        }*/

        public static void SetupCharacterModel(GameObject prefab, CustomRendererInfo[] rendererInfo, int mainRendererIndex)
        {
            CharacterModel characterModel = prefab.GetComponent<ModelLocator>().modelTransform.gameObject.AddComponent<CharacterModel>();
            ChildLocator childLocator = characterModel.GetComponent<ChildLocator>();

            characterModel.body = prefab.GetComponent<CharacterBody>();

            List<CharacterModel.RendererInfo> rendererInfos = new List<CharacterModel.RendererInfo>();

            for (int i = 0; i < rendererInfo.Length; i++)
            {
                rendererInfos.Add(new CharacterModel.RendererInfo
                {
                    renderer = childLocator.FindChild(rendererInfo[i].childName).GetComponent<SkinnedMeshRenderer>(),
                    defaultMaterial = rendererInfo[i].material,
                    ignoreOverlays = rendererInfo[i].ignoreOverlays,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On
                });
            }

            characterModel.baseRendererInfos = rendererInfos.ToArray();

            characterModel.autoPopulateLightInfos = true;
            characterModel.invisibilityCount = 0;
            characterModel.temporaryOverlays = new List<TemporaryOverlay>();

            characterModel.mainSkinnedMeshRenderer = characterModel.baseRendererInfos[mainRendererIndex].renderer.GetComponent<SkinnedMeshRenderer>();
        }

        public static GameObject CreatePrefab(string bodyName, string modelName, BodyInfo bodyInfo)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/CharacterBodies/MercBody"), bodyName);

            //GameObject model = CreateModel(newPrefab, modelName);
            //Transform modelBaseTransform = SetupModel(newPrefab, model.transform);

            #region CharacterBody
            CharacterBody bodyComponent = newPrefab.GetComponent<CharacterBody>();

            bodyComponent.bodyIndex = -1;
            bodyComponent.name = bodyInfo.bodyName;
            bodyComponent.baseNameToken = bodyInfo.bodyNameToken;
            bodyComponent.subtitleNameToken = bodyInfo.subtitleNameToken;
            bodyComponent.portraitIcon = bodyInfo.characterPortrait;
            bodyComponent.crosshairPrefab = bodyInfo.crosshair;

            bodyComponent.bodyFlags = CharacterBody.BodyFlags.ImmuneToExecutes;
            bodyComponent.rootMotionInMainState = false;

            bodyComponent.baseMaxHealth = bodyInfo.maxHealth;
            bodyComponent.levelMaxHealth = bodyInfo.healthGrowth;

            bodyComponent.baseRegen = bodyInfo.healthRegen;
            bodyComponent.levelRegen = bodyComponent.baseRegen * 0.2f;

            bodyComponent.baseMaxShield = bodyInfo.shield;
            bodyComponent.levelMaxShield = bodyInfo.shieldGrowth;

            bodyComponent.baseMoveSpeed = bodyInfo.moveSpeed;
            bodyComponent.levelMoveSpeed = bodyInfo.moveSpeedGrowth;

            bodyComponent.baseAcceleration = bodyInfo.acceleration;

            bodyComponent.baseJumpPower = bodyInfo.jumpPower;
            bodyComponent.levelJumpPower = bodyInfo.jumpPowerGrowth;

            bodyComponent.baseDamage = bodyInfo.damage;
            bodyComponent.levelDamage = bodyInfo.damageGrowth;

            bodyComponent.baseAttackSpeed = bodyInfo.attackSpeed;
            bodyComponent.levelAttackSpeed = bodyInfo.attackSpeedGrowth;

            bodyComponent.baseArmor = bodyInfo.armor;
            bodyComponent.levelArmor = bodyInfo.armorGrowth;

            bodyComponent.baseCrit = bodyInfo.crit;
            bodyComponent.levelCrit = bodyInfo.critGrowth;

            bodyComponent.baseJumpCount = bodyInfo.jumpCount;

            bodyComponent.sprintingSpeedMultiplier = 1.45f;

            bodyComponent.hideCrosshair = false;
            //bodyComponent.aimOriginTransform = modelBaseTransform.Find("AimOrigin");
            bodyComponent.hullClassification = HullClassification.Human;

            bodyComponent.preferredPodPrefab = bodyInfo.podPrefab;

            bodyComponent.isChampion = false;
            #endregion

            //SetupCharacterDirection(newPrefab, modelBaseTransform, model.transform);
            SetupCameraTargetParams(newPrefab);
            //SetupModelLocator(newPrefab, modelBaseTransform, model.transform);
            SetupRigidbody(newPrefab);
            SetupCapsuleCollider(newPrefab);
            //SetupMainHurtbox(newPrefab, model);
            //SetupFootstepController(model);
            //SetupRagdoll(model);
            //SetupAimAnimator(newPrefab, model);
            SetupInteractor(newPrefab, bodyInfo);

            return newPrefab;
        }

        private static void SetupInteractor(GameObject prefab, BodyInfo bodyInfo)
        {
            var interactor = prefab.GetComponent<Interactor>();
            interactor.maxInteractionDistance = bodyInfo.maxInteractionDistance;
        }

        private static Transform SetupModel(GameObject prefab, Transform modelTransform)
        {
            GameObject modelBase = new GameObject("ModelBase");
            modelBase.transform.parent = prefab.transform;
            modelBase.transform.localPosition = new Vector3(0f, -0.92f, 0f);
            modelBase.transform.localRotation = Quaternion.identity;
            modelBase.transform.localScale = new Vector3(1f, 1f, 1f);

            GameObject cameraPivot = new GameObject("CameraPivot");
            cameraPivot.transform.parent = modelBase.transform;
            cameraPivot.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            cameraPivot.transform.localRotation = Quaternion.identity;
            cameraPivot.transform.localScale = Vector3.one;

            GameObject aimOrigin = new GameObject("AimOrigin");
            aimOrigin.transform.parent = modelBase.transform;
            aimOrigin.transform.localPosition = new Vector3(0f, 2.2f, 0f);
            aimOrigin.transform.localRotation = Quaternion.identity;
            aimOrigin.transform.localScale = Vector3.one;
            prefab.GetComponent<CharacterBody>().aimOriginTransform = aimOrigin.transform;

            modelTransform.parent = modelBase.transform;
            modelTransform.localPosition = Vector3.zero;
            modelTransform.localRotation = Quaternion.identity;

            return modelBase.transform;
        }

        /*private static GameObject CreateModel(GameObject main, string modelName)
        {
            ScoutPlugin.DestroyImmediate(main.transform.Find("ModelBase").gameObject);
            ScoutPlugin.DestroyImmediate(main.transform.Find("CameraPivot").gameObject);
            ScoutPlugin.DestroyImmediate(main.transform.Find("AimOrigin").gameObject);

            return Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(modelName);
        }*/

        private static void SetupCharacterDirection(GameObject prefab, Transform modelBaseTransform, Transform modelTransform)
        {
            CharacterDirection characterDirection = prefab.GetComponent<CharacterDirection>();
            characterDirection.targetTransform = modelBaseTransform;
            characterDirection.overrideAnimatorForwardTransform = null;
            characterDirection.rootMotionAccumulator = null;
            characterDirection.modelAnimator = modelTransform.GetComponent<Animator>();
            characterDirection.driveFromRootRotation = false;
            characterDirection.turnSpeed = 720f;
        }

        private static void SetupCameraTargetParams(GameObject prefab)
        {
            CameraTargetParams cameraTargetParams = prefab.GetComponent<CameraTargetParams>();
            cameraTargetParams.cameraParams = Resources.Load<GameObject>("Prefabs/CharacterBodies/MercBody").GetComponent<CameraTargetParams>().cameraParams;
            cameraTargetParams.cameraPivotTransform = prefab.transform.Find("ModelBase").Find("CameraPivot");
            cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;
            cameraTargetParams.recoil = Vector2.zero;
            cameraTargetParams.idealLocalCameraPos = Vector3.zero;
            cameraTargetParams.dontRaycastToPivot = false;
        }

        private static void SetupModelLocator(GameObject prefab, Transform modelBaseTransform, Transform modelTransform)
        {
            ModelLocator modelLocator = prefab.GetComponent<ModelLocator>();
            modelLocator.modelTransform = modelTransform;
            modelLocator.modelBaseTransform = modelBaseTransform;
        }

        private static void SetupRigidbody(GameObject prefab)
        {
            Rigidbody rigidbody = prefab.GetComponent<Rigidbody>();
            rigidbody.mass = 100f;
        }

        private static void SetupCapsuleCollider(GameObject prefab)
        {
            CapsuleCollider capsuleCollider = prefab.GetComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0f, 0f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 1.82f;
            capsuleCollider.direction = 1;
        }

        private static void SetupMainHurtbox(GameObject prefab, GameObject model)
        {
            HurtBoxGroup hurtBoxGroup = model.AddComponent<HurtBoxGroup>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            HurtBox mainHurtbox = childLocator.FindChild("MainHurtbox").gameObject.AddComponent<HurtBox>();
            mainHurtbox.gameObject.layer = LayerIndex.entityPrecise.intVal;
            mainHurtbox.healthComponent = prefab.GetComponent<HealthComponent>();
            mainHurtbox.isBullseye = true;
            mainHurtbox.damageModifier = HurtBox.DamageModifier.Normal;
            mainHurtbox.hurtBoxGroup = hurtBoxGroup;
            mainHurtbox.indexInGroup = 0;

            hurtBoxGroup.hurtBoxes = new HurtBox[]
            {
                mainHurtbox
            };

            hurtBoxGroup.mainHurtBox = mainHurtbox;
            hurtBoxGroup.bullseyeCount = 1;
        }

        private static void SetupFootstepController(GameObject model)
        {
            FootstepHandler footstepHandler = model.AddComponent<FootstepHandler>();
            footstepHandler.baseFootstepString = "Play_player_footstep";
            footstepHandler.sprintFootstepOverrideString = "";
            footstepHandler.enableFootstepDust = true;
            footstepHandler.footstepDustPrefab = Resources.Load<GameObject>("Prefabs/GenericFootstepDust");
        }

        private static void SetupRagdoll(GameObject model)
        {
            RagdollController ragdollController = model.GetComponent<RagdollController>();

            if (!ragdollController) return;

            if (ragdollMaterial == null) ragdollMaterial = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RagdollController>().bones[1].GetComponent<Collider>().material;

            foreach (Transform i in ragdollController.bones)
            {
                if (i)
                {
                    i.gameObject.layer = LayerIndex.ragdoll.intVal;
                    Collider j = i.GetComponent<Collider>();
                    if (j)
                    {
                        j.material = ragdollMaterial;
                        j.sharedMaterial = ragdollMaterial;
                    }
                }
            }
        }

        private static void SetupAimAnimator(GameObject prefab, GameObject model)
        {
            AimAnimator aimAnimator = model.AddComponent<AimAnimator>();
            aimAnimator.directionComponent = prefab.GetComponent<CharacterDirection>();
            aimAnimator.pitchRangeMax = 60f;
            aimAnimator.pitchRangeMin = -60f;
            aimAnimator.yawRangeMin = -80f;
            aimAnimator.yawRangeMax = 80f;
            aimAnimator.pitchGiveupRange = 30f;
            aimAnimator.yawGiveupRange = 10f;
            aimAnimator.giveupDuration = 3f;
            aimAnimator.inputBank = prefab.GetComponent<InputBankTest>();
        }
    }
}