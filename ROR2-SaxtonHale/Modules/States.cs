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

            LoadoutAPI.AddSkill(typeof(ChargeSuperJump));
            LoadoutAPI.AddSkill(typeof(LaunchSuperJump));

            LoadoutAPI.AddSkill(typeof(Crouch));
            LoadoutAPI.AddSkill(typeof(Weighdown));

            LoadoutAPI.AddSkill(typeof(Rage));
            LoadoutAPI.AddSkill(typeof(NoAttackState));
        }
    }
}
