using EntityStates;
using R2API;
using RoR2;

namespace ROR2_SaxtonHale.Modules
{
    public static class States
    {
        public static void RegisterStates()
        {
            LoadoutAPI.AddSkill(typeof(Punch));

            LoadoutAPI.AddSkill(typeof(Superjump));

            LoadoutAPI.AddSkill(typeof(Weighdown));

            LoadoutAPI.AddSkill(typeof(Rage));
        }
    }
}
