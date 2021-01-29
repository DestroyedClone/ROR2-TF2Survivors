using EntityStates;
using R2API;
using RoR2;

namespace ROR2_Scout.Modules
{
    public static class States
    {
        public static void RegisterStates()
        {
            LoadoutAPI.AddSkill(typeof(ROR2_Scout.States.FireScattergun));
            LoadoutAPI.AddSkill(typeof(ROR2_Scout.States.FireForceNature));

            LoadoutAPI.AddSkill(typeof(ROR2_Scout.States.FireScoutPistol));
            LoadoutAPI.AddSkill(typeof(ROR2_Scout.States.FireCleaver));
            LoadoutAPI.AddSkill(typeof(ROR2_Scout.States.FireMilk));

            LoadoutAPI.AddSkill(typeof(ROR2_Scout.States.FireMarkFan));
            LoadoutAPI.AddSkill(typeof(ROR2_Scout.States.FireBleedBat));
            LoadoutAPI.AddSkill(typeof(ROR2_Scout.States.FireSandman));

            LoadoutAPI.AddSkill(typeof(ROR2_Scout.States.ActivateRage));
        }
    }
}
