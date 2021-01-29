using System;
using RoR2;
using R2API;
using UnityEngine;
using System.Linq;


namespace ROR2_SaxtonHale.Modules
{
    public static class Artifacts
    {
        public static ArtifactDef GoombaArtifactDef = ScriptableObject.CreateInstance<ArtifactDef>();
        private static readonly float maxDistance = 2f;
        private static readonly float minFallSpeed = 10f;
        private static readonly float bounceForce = 2000f;
        public static GameObject goombaGameObject = new GameObject();
        private static readonly DamageInfo goombaDamageInfo = new DamageInfo()
        {
            damage = 500f,
            inflictor = goombaGameObject
        };
        private static readonly string goombaDeathToken = "You have been Goomba Stomped!";
        private static readonly string goombaDeathMultiplayerToken = "{0} has been Goomba Stomped!";

        public static void RegisterArtifacts()
        {
            GoombaArtifactDef.nameToken = "Artifact of Headstomping";
            GoombaArtifactDef.descriptionToken = "Deal substantial damage upon landing on an enemy's head.";
            GoombaArtifactDef.smallIconDeselectedSprite = LoadoutAPI.CreateSkinIcon(Color.white, Color.white, Color.white, Color.white);
            GoombaArtifactDef.smallIconSelectedSprite = LoadoutAPI.CreateSkinIcon(Color.gray, Color.white, Color.white, Color.white);

            goombaGameObject.name = "GoombaStomp";

            LanguageAPI.Add("PLAYER_DEATH_QUOTE_GOOMBADEATH", goombaDeathToken);
            LanguageAPI.Add("PLAYER_DEATH_QUOTE_GOOMBADEATH_2P", goombaDeathMultiplayerToken);

            ArtifactCatalog.getAdditionalEntries += (list) =>
            {
                list.Add(GoombaArtifactDef);
            };

            On.RoR2.CharacterMotor.OnHitGround += CharacterMotor_OnHitGround;
            On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += GlobalEventManager_OnPlayerCharacterDeath; //full override until i can get IL
        }

        private static void GlobalEventManager_OnPlayerCharacterDeath(On.RoR2.GlobalEventManager.orig_OnPlayerCharacterDeath orig, GlobalEventManager self, DamageReport damageReport, NetworkUser victimNetworkUser)
        {
            if (!victimNetworkUser)
            {
                return;
            }
            CharacterBody victimBody = damageReport.victimBody;
            string text;

            if (damageReport.damageInfo.inflictor == goombaGameObject && RunArtifactManager.instance.IsArtifactEnabled(GoombaArtifactDef.artifactIndex))
            {
                text = "PLAYER_DEATH_QUOTE_GOOMBADEATH";
            }
            else if ((damageReport.damageInfo.damageType & DamageType.VoidDeath) != DamageType.Generic)
            {
                text = "PLAYER_DEATH_QUOTE_VOIDDEATH";
            }
            else if (damageReport.isFallDamage)
            {
                text = GlobalEventManager.fallDamageDeathQuoteTokens[UnityEngine.Random.Range(0, GlobalEventManager.fallDamageDeathQuoteTokens.Length)];
            }
            else if (victimBody && victimBody.inventory && victimBody.inventory.GetItemCount(ItemIndex.LunarDagger) > 0)
            {
                text = "PLAYER_DEATH_QUOTE_BRITTLEDEATH";
            }
            else
            {
                text = GlobalEventManager.standardDeathQuoteTokens[UnityEngine.Random.Range(0, GlobalEventManager.standardDeathQuoteTokens.Length)];
            }
            if (victimNetworkUser.masterController)
            {
                victimNetworkUser.masterController.finalMessageTokenServer = text;
            }
            Chat.SendBroadcastChat(new Chat.PlayerDeathChatMessage
            {
                subjectAsNetworkUser = victimNetworkUser,
                baseToken = text
            });
        }

        private static void CharacterMotor_OnHitGround(On.RoR2.CharacterMotor.orig_OnHitGround orig, CharacterMotor self, CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            bool hasGoombad = false;
            bool restoreFallDamage = false;
            if (RunArtifactManager.instance.IsArtifactEnabled(GoombaArtifactDef.artifactIndex))
            {
                if (self.body)
                {
                    Chat.AddMessage("Speed: " + Math.Abs(hitGroundInfo.velocity.y) + "/"+ minFallSpeed);
                    if (Math.Abs(hitGroundInfo.velocity.y) >= minFallSpeed)
                    {
                        var bodySearch = new BullseyeSearch() //let's just get the nearest player
                        {
                            viewer = self.body,
                            sortMode = BullseyeSearch.SortMode.Distance,
                            teamMaskFilter = TeamMask.GetEnemyTeams(self.body.teamComponent.teamIndex)
                        };
                        Debug.Log(bodySearch.GetResults());

                        var nearestBody = bodySearch.GetResults().ToArray();

                        // We very likely landed on an enemy.
                        if (nearestBody.Length > 0)
                        {
                            var firstBody = nearestBody.FirstOrDefault();
                            var distance = Vector3.Distance(hitGroundInfo.position, Helpers.GetHeadPosition(firstBody.healthComponent.body));
                            if (distance <= maxDistance)
                            {
                                goombaDamageInfo.attacker = self.body.gameObject;
                                firstBody.healthComponent.TakeDamage(goombaDamageInfo);
                                if ((self.body.bodyFlags & CharacterBody.BodyFlags.IgnoreFallDamage) == CharacterBody.BodyFlags.None)
                                {
                                    self.body.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                                    restoreFallDamage = true;
                                }
                                Chat.AddMessage("Goomba!");
                                hasGoombad = true;
                            }
                        }
                    }
                }
            }
            orig(self, hitGroundInfo);
            if (hasGoombad)
            {
                self.Motor.ForceUnground();
                self.ApplyForce(Vector3.up * bounceForce);
            }
            if (restoreFallDamage)
            {
                self.body.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            }
        }
    }
}
