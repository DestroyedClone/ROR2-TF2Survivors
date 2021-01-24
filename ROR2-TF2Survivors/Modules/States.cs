using EntityStates;
using R2API;
using RoR2;

namespace ROR2_Scout.Modules
{
    public static class States
    {
        public static void RegisterStates()
        {
            LoadoutAPI.AddSkill(typeof(States.FireScattergun));
            LoadoutAPI.AddSkill(typeof(States.FireForceNature));

            LoadoutAPI.AddSkill(typeof(States.FireScoutPistol));
            LoadoutAPI.AddSkill(typeof(States.FireCleaver));
            LoadoutAPI.AddSkill(typeof(States.FireMilk));

            LoadoutAPI.AddSkill(typeof(States.FireMarkFan));
            LoadoutAPI.AddSkill(typeof(States.FireBleedBat));
            LoadoutAPI.AddSkill(typeof(States.FireSandman));

            LoadoutAPI.AddSkill(typeof(States.ActivateRage));
        }
    }
}
