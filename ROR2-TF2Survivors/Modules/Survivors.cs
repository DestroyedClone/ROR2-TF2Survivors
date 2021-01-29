using R2API;
using RoR2;
using UnityEngine.Networking;

namespace ROR2_Scout.Modules
{
    public class Survivors
    {
        public static void RegisterSurvivors()
        {
            //Prefabs.paladinDisplayPrefab.AddComponent<NetworkIdentity>(); TODO

            string unlockString = "SCOUT_UNLOCKABLE_REWARD_ID";
            //if (Config.forceUnlock.Value) unlockString = "";

            SurvivorDef survivorDef = new SurvivorDef //TODO CrossRef with document
            {
                name = "SCOUT_NAME",
                unlockableName = unlockString,
                descriptionToken = "SCOUT_DESCRIPTION",
                primaryColor = ScoutPlugin.characterColor,
                bodyPrefab = ScoutPlugin.characterPrefab,
                //displayPrefab = characterDisplay,
                outroFlavorToken = "SCOUT_OUTRO_FLAVOR"
            };

            SurvivorAPI.AddSurvivor(survivorDef);
        }
    }
}
