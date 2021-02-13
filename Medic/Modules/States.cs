using EntityStates;
using R2API;
using RoR2;
using Medic.States;

namespace Medic.Modules
{
    public static class States
    {
        public static void RegisterStates()
        {

            LoadoutAPI.AddSkill(typeof(StockPassive));
            LoadoutAPI.AddSkill(typeof(CritPassive));
            LoadoutAPI.AddSkill(typeof(ResPassive));
            LoadoutAPI.AddSkill(typeof(HealPassive));

            LoadoutAPI.AddSkill(typeof(FireSyringeGun));
            LoadoutAPI.AddSkill(typeof(FireHealSyringeGun));
            LoadoutAPI.AddSkill(typeof(FireCrossbow));

            LoadoutAPI.AddSkill(typeof(ChooseHealTarget));

            LoadoutAPI.AddSkill(typeof(ActivateUber));
            LoadoutAPI.AddSkill(typeof(Weighdown));

            LoadoutAPI.AddSkill(typeof(Rage));
            LoadoutAPI.AddSkill(typeof(NoAttackState));
        }
    }
}
