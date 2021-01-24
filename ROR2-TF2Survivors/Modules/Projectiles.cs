using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace ROR2_Scout.Modules
{
    public static class Projectiles
    {
        public static GameObject cleaverProjectile;
        public static GameObject milkjarProjectile;
        public static GameObject baseballProjectile;
        public static GameObject ornamentProjectile;

        public static GameObject milkSplashWard;

        public static void RegisterProjectiles()
        {
            cleaverProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/EngiGrenadeProjectile"), "Prefabs/Projectiles/CleaverProjectile");
            milkjarProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/EngiGrenadeProjectile"), "Prefabs/Projectiles/MilkjarProjectile");
            baseballProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/EngiGrenadeProjectile"), "Prefabs/Projectiles/BaseballProjectile");
            ornamentProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/EngiGrenadeProjectile"), "Prefabs/Projectiles/OrnamentProjectile");

            milkSplashWard = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/SporeGrenadeProjectileDotZone"), "MilkSplashZone", true);

            cleaverProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            cleaverProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            cleaverProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.BleedOnHit;
            cleaverProjectile.GetComponent<ProjectileImpactExplosion>().destroyOnWorld = true;
            cleaverProjectile.GetComponent<ProjectileImpactExplosion>().blastRadius = 0.1f;
            cleaverProjectile.GetComponent<ProjectileImpactExplosion>().timerAfterImpact = false;
            //cleaverProjectile.GetComponent<ProjectileSimple>().velocity = 10; //doesnt work?

            var projectileImpactExplosion = milkjarProjectile.GetComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.fireChildren = true;
            projectileImpactExplosion.childrenProjectilePrefab = milkSplashWard;
            projectileImpactExplosion.childrenCount = 1;

            BuffWard buffWard = milkSplashWard.GetComponent<BuffWard>();
            buffWard.animateRadius = false;
            buffWard.buffDuration = 0.5f;
            buffWard.buffType = Buffs.milkedDebuff;
            buffWard.expireDuration = 0.6f;
            buffWard.invertTeamFilter = true;
            buffWard.radius = 5f;

            var newWard = milkSplashWard.AddComponent<ExtinguishZonePrefabAlt>();
            newWard.animateRadius = false;
            newWard.radius = 5f;

            baseballProjectile.AddComponent<BaseballController>();

            ProjectileCatalog.getAdditionalEntries += list =>
            {
                list.Add(cleaverProjectile);
                list.Add(milkjarProjectile);
                list.Add(baseballProjectile);
                list.Add(ornamentProjectile);
                list.Add(milkSplashWard);
            };

            RegProj(cleaverProjectile);
            RegProj(milkjarProjectile);
            RegProj(baseballProjectile);
            RegProj(ornamentProjectile);
            RegProj(milkSplashWard);
        }
        private static void RegProj(GameObject g)
        { if (g) PrefabAPI.RegisterNetworkPrefab(g); }

        public class CleaverController : MonoBehaviour, IProjectileImpactBehavior
        {
            float stopWatch = 0f;
            readonly float maxTime = 10f;
            bool hasHit = false;
            ProjectileController projectileController = null;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "UnityEngine")]
            void Awake()
            {
                projectileController = gameObject.GetComponent<ProjectileController>();
                if (!projectileController) enabled = false;
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "UnityEngine")]
            void FixedUpdate()
            {
                stopWatch += Time.fixedDeltaTime;
            }

            void IProjectileImpactBehavior.OnProjectileImpact(ProjectileImpactInfo impactInfo)
            {
                var hitEnemy = impactInfo.collider.gameObject.GetComponent<CharacterBody>();
                if (hitEnemy)
                {
                    if (!hasHit)
                    {
                        Chat.AddMessage("Cleaver hit!");
                        hasHit = true;
                        if (stopWatch >= maxTime)
                        {
                            var skillLocator = projectileController.owner.GetComponent<CharacterBody>()?.masterObject.GetComponent<SkillLocator>();
                            if (skillLocator)
                            {
                                skillLocator.utility.rechargeStopwatch += 1f;
                            }
                        }
                    }
                }
            }
        }

        public class BaseballController : MonoBehaviour
        {
            float stopWatch = 0f;
            readonly float maxDuration = 10f;
            ProjectileDamage projectileDamage;
            ProjectileSingleTargetImpact projectileSingleTargetImpact;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "UnityEngine")]
            void Awake()
            {
                projectileDamage = gameObject.GetComponent<ProjectileDamage>();
                projectileSingleTargetImpact = gameObject.GetComponent<ProjectileSingleTargetImpact>();
                if (!projectileDamage || !projectileSingleTargetImpact)
                {
                    enabled = false;
                    return;
                }
            } //nullcheck

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "UnityEngine")]
            void FixedUpdate()
            {
                stopWatch += Time.fixedDeltaTime;
                if (stopWatch >= maxDuration)
                {
                    projectileDamage.damageType |= DamageType.Stun1s;
                    // TODO: change later for mapwide sound
                    projectileSingleTargetImpact.enemyHitSoundString = "";
                    enabled = false;
                }
            }
        }
    }
}