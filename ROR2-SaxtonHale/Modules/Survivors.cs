using R2API;
using RoR2;
using UnityEngine.Networking;

namespace ROR2_SaxtonHale.Modules
{
    public static class Survivors
    {
        public static void RegisterSurvivors()
        {
            Prefabs.haleDisplayPrefab.AddComponent<NetworkIdentity>();

            SurvivorDef survivorDef = new SurvivorDef
            {
                name = "SAXTONHALE_NAME",
                descriptionToken = "SAXTONHALE_DESCRIPTION",
                primaryColor = SaxtonHalePlugin.characterColor,
                bodyPrefab = Prefabs.halePrefab,
                displayPrefab = Prefabs.haleDisplayPrefab,
                outroFlavorToken = "SAXTONHALE_OUTRO_FLAVOR"
            };

            SurvivorAPI.AddSurvivor(survivorDef);
        }
    }
}
