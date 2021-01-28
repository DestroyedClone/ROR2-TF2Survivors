using EntityStates;
using R2API;
using RoR2;
using ROR2_SaxtonHale.States;

namespace ROR2_SaxtonHale.Modules
{
    public static class States
    {
        public static void RegisterStates()
        {
            LoadoutAPI.AddSkill(typeof(Punch));

            LoadoutAPI.AddSkill(typeof(ChargeSuperjump));
            LoadoutAPI.AddSkill(typeof(LaunchSuperjump));

            LoadoutAPI.AddSkill(typeof(Crouch));
            LoadoutAPI.AddSkill(typeof(Weighdown));

            LoadoutAPI.AddSkill(typeof(Rage));
        }
    }
}
