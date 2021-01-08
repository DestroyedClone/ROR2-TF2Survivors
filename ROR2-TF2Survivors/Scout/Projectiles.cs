using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace ROR2_TF2Survivors.Scout
{
    public static class Projectiles
    {
        public static GameObject cleaverProjectile;
        public static GameObject milkjarProjectile;
        public static GameObject baseballProjectile;
        public static GameObject ornamentProjectile;

        public static void RegisterProjectiles()
        {
            cleaverProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/EngiGrenadeProjectile"), "Prefabs/Projectiles/CleaverProjectile");
            milkjarProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/EngiGrenadeProjectile"), "Prefabs/Projectiles/MilkjarProjectile");
            baseballProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/EngiGrenadeProjectile"), "Prefabs/Projectiles/BaseballProjectile");
            ornamentProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/EngiGrenadeProjectile"), "Prefabs/Projectiles/OrnamentProjectile");

            cleaverProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            cleaverProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            cleaverProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.BleedOnHit;
            cleaverProjectile.GetComponent<ProjectileImpactExplosion>().destroyOnWorld = true;
            cleaverProjectile.GetComponent<ProjectileImpactExplosion>().blastRadius = 0.1f;
            cleaverProjectile.GetComponent<ProjectileImpactExplosion>().timerAfterImpact = false;
            //cleaverProjectile.GetComponent<ProjectileSimple>().velocity = 10; //doesnt work?

            ProjectileCatalog.getAdditionalEntries += list =>
            {
                list.Add(cleaverProjectile);
                list.Add(milkjarProjectile);
                list.Add(baseballProjectile);
                list.Add(ornamentProjectile);
            };

            RegProj(cleaverProjectile);
            RegProj(milkjarProjectile);
            RegProj(baseballProjectile);
            RegProj(ornamentProjectile);
        }
        private static void RegProj(GameObject g)
        { if (g) PrefabAPI.RegisterNetworkPrefab(g); }

        public class CleaverController : MonoBehaviour
        {
            float stopWatch = 0f;


        }

    }
}