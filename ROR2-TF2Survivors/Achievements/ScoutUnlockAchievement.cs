using R2API;
using R2API.Utils;
using RoR2;
using System;

namespace ROR2_Scout.Achievements
{
    public class ScoutUnlockAchievement : ModdedUnlockableAndAchievement<CustomSpriteProvider> //TODO: Taken from HuntressCollectCrowbarsAchievement
    {
        public override String AchievementIdentifier { get; } = "SCOUTSURVIVOR_UNLOCKABLE_ACHIEVEMENT_ID";
        public override String UnlockableIdentifier { get; } = "SCOUTSURVIVOR_UNLOCKABLE_REWARD_ID";
        public override String PrerequisiteUnlockableIdentifier { get; } = "SCOUTSURVIVOR_UNLOCKABLE_PREREQ_ID";
        public override String AchievementNameToken { get; } = "SCOUTSURVIVOR_UNLOCKABLE_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "SCOUTSURVIVOR_UNLOCKABLE_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "SCOUTSURVIVOR_UNLOCKABLE_UNLOCKABLE_NAME";
        protected override CustomSpriteProvider SpriteProvider { get; } = new CustomSpriteProvider("@Paladin:Assets/Paladin/Icons/texPaladinAchievement.png");

        public override int LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("TF2Scout");
        }

		// Token: 0x060032E9 RID: 13033 RVA: 0x000D06F5 File Offset: 0x000CE8F5
		public override void OnInstall()
		{
			base.OnInstall();
		}

		public override void OnUninstall()
		{
			this.SetCurrentInventory(null);
			base.OnUninstall();
		}

		private void UpdateInventory()
		{
			Inventory inventory = null;
			if (base.localUser.cachedMasterController)
			{
				inventory = base.localUser.cachedMasterController.master.inventory;
			}
			this.SetCurrentInventory(inventory);
		}

		private void SetCurrentInventory(Inventory newInventory)
		{
			if (this.currentInventory == newInventory)
			{
				return;
			}
			if (this.currentInventory != null)
			{
				this.currentInventory.onInventoryChanged -= this.OnInventoryChanged;
			}
			this.currentInventory = newInventory;
			if (this.currentInventory != null)
			{
				this.currentInventory.onInventoryChanged += this.OnInventoryChanged;
				this.OnInventoryChanged();
			}
		}

		public override void OnBodyRequirementMet()
		{
			base.OnBodyRequirementMet();
			base.localUser.onMasterChanged += this.UpdateInventory;
			this.UpdateInventory();
		}

		public override void OnBodyRequirementBroken()
		{
			base.localUser.onMasterChanged -= this.UpdateInventory;
			this.SetCurrentInventory(null);
			base.OnBodyRequirementBroken();
		}

		private void OnInventoryChanged()
		{
			if (ScoutUnlockAchievement.requirement <= this.currentInventory.GetItemCount(ItemIndex.SprintBonus))
			{
				base.Grant();
			}
		}

		private Inventory currentInventory;

		private static readonly int requirement = 30;
	}
}
