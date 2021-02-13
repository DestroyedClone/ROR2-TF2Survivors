using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Medic.Modules
{
    public static class Helpers
    {
        public static void CreateHitbox(GameObject prefab, Transform hitboxTransform, string hitboxName)
        {
            HitBoxGroup hitBoxGroup = prefab.AddComponent<HitBoxGroup>();

            HitBox hitBox = hitboxTransform.gameObject.AddComponent<HitBox>();
            hitboxTransform.gameObject.layer = LayerIndex.projectile.intVal;

            hitBoxGroup.hitBoxes = new HitBox[]
            {
                hitBox
            };

            hitBoxGroup.groupName = hitboxName;
        }

        public static TeamIndex GetEnemyTeam(TeamIndex teamIndex)
        {
            if (teamIndex == TeamIndex.Monster) return TeamIndex.Player;
            else if (teamIndex == TeamIndex.Player) return TeamIndex.Monster;
            else return TeamIndex.Neutral;
        }

        public static Vector3 GetHeadPosition(CharacterBody characterBody)
        {
            var dist = Vector3.Distance(characterBody.corePosition, characterBody.footPosition);
            return characterBody.corePosition + Vector3.up * dist;
        }

        public static Animator GetModelAnimator(CharacterBody characterBody)
        {
            if (characterBody.modelLocator && characterBody.modelLocator.modelTransform)
            {
                return characterBody.modelLocator.modelTransform.GetComponent<Animator>();
            }
            return null;
        }
    }
}