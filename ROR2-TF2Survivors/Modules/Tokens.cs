using R2API;
using System;

namespace ROR2_Scout.Modules
{
    public static class Tokens
    {
        public static void AddTokens()
        {

            string desc = "We lost 5, kick the scout.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > As a Scout, your Pistol is great for picking off enemies at a distance." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > As a Scout, use Mad Milk to douse flames on yourself and on your teammates." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > As a Scout, be careful when using Crit-a-Cola. Saving it for surprise attacks and taking advantage of your speed can help you avoid taking extra damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > As a Scout, you and your teammates regain lost health when hitting enemies drenched in Mad Milk. Initiate fights with it to improve your team's survivability.</color>" + Environment.NewLine + Environment.NewLine;

            LanguageAPI.Add("SCOUTSURVIVOR_NAME", "Teufort Scout");
            LanguageAPI.Add("SCOUTSURVIVOR_DESCRIPTION", desc);
            LanguageAPI.Add("SCOUTSURVIVOR_SUBTITLE", "Force of Nature");
            LanguageAPI.Add("SCOUTSURVIVOR_LORE", "Born and raised in Boston, Massachusetts, USA, the Scout is a fast-running scrapper with a baseball bat and a snarky \"in-your-face\" attitude.");
            LanguageAPI.Add("SCOUTSURVIVOR_OUTRO_FLAVOR", "..and so he left, really freakin' cool.");

            LanguageAPI.Add("SCOUTSURVIVORBODY_RED_SKIN_NAME", "Reliable Excavation Demolition");
            LanguageAPI.Add("SCOUTSURVIVORBODY_BLU_SKIN_NAME", "Builder's League United");
            LanguageAPI.Add("SCOUTSURVIVORBODY_LIME_SKIN_NAME", "The Bitter Taste of Defeat and Lime");

            LanguageAPI.Add("SCOUTSURVIVORBODY_MALE_BONK_SKIN_NAME", "Bonk Boy");
            LanguageAPI.Add("SCOUTSURVIVORBODY_FEMALE_BONK_SKIN_NAME", "Bonk Girl");

            LanguageAPI.Add("SCOUTSURVIVOR_PASSIVE_NAME", "Irradiated Biology");
            LanguageAPI.Add("SCOUTSURVIVOR_PASSIVE_DESCRIPTION", "Scout can <style=cIsUtility>double jump</style>." +
                "\n[MVM+] Scout has an <style=cIsUtility>increased interaction</style> range."); //from 3 to 5

            LanguageAPI.Add("SCOUTSURVIVOR_PRIMARY_SCATTERGUN_NAME", "Scattergun");
            LanguageAPI.Add("SCOUTSURVIVOR_PRIMARY_SCATTERGUN_DESCRIPTION", "Fire a spread of bullets that deal <style=cIsDamage>6x80% damage</style>.");

            LanguageAPI.Add("SCOUTSURVIVOR_PRIMARY_FORCENATURE_NAME", "Force 'a Nature");
            LanguageAPI.Add("SCOUTSURVIVOR_PRIMARY_FORCENATURE_DESCRIPTION", "Fire a wide spread of bullets that deal <style=cIsDamage>8x60% damage</style>.\nDeals knockback to <style=cIsUtility>target and shooter<style>.");

            desc = "Fire a stream of bullets that deal <style=cIsDamage>70% damage</style>.\n<style=cIsUtility>Holds 12.</style>";
            LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_PISTOL_NAME", "Pistol");
            LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_PISTOL_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Bleeding.</style> Throw a cleaver for <style=cIsDamage>100% damage that bleeds the enemy</style> on hit. Deals a <style=cIsDamage>critical hit</style> at maximum range</style>.";
            LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_CLEAVER_NAME", "The Flying Guillotine");
            LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_CLEAVER_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Milking.</style> Throw a jar to douse enemies, inflicting <style=cIsUtility>Milked</style>. " +
                "Allies that hit <style=cIsUtility>milked</style> enemies <style=cIsHealing> heal 5% of damage dealt.</style>" +
                "Can <style=cIsUtility>extinguish</style> allies.";
            LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_MILK_NAME", "Mad Milk");
            LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_MILK_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Death-Marking.</style> Swing your weapon for <style=cIsDamage>30% damage</style> and <style=cIsDamage>mark the enemy for death</style>." +
                " <style=cDeath>Only one enemy may be marked at a time</style>.";
            LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_MARKDEATH_NAME", "Fan O' War");
            LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_MARKDEATH_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Stunning.</style> Swing for <style=cIsDamage>70% damage</style> and launch a ball for <style=cIsDamage>40% damage</style>. If you hit an enemy at the maximum range, it stuns the enemy.";
            LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_SANDMAN_NAME", "Sandman");
            LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_SANDMAN_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Bleeding.</style> Swing your club for <style=cIsDamage>70% damage</style> and <style=cIsUtility>bleed the enemy</style>.";
            LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_BLEEDBAT_NAME", "Boston Basher");
            LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_BLEEDBAT_DESCRIPTION", desc);

            desc = "Gain +50% attack speed, +50% movement speed, and max utility charges for 8 seconds.";
            LanguageAPI.Add("SCOUTSURVIVOR_SPECIAL_FF2_NAME", "Empowered");
            LanguageAPI.Add("SCOUTSURVIVOR_SPECIAL_FF2_DESCRIPTION", desc);

            desc = "Start with no equipment.";
            LanguageAPI.Add("SCOUTSURVIVOR_EQUIPMENT_NONE_NAME", "None");
            LanguageAPI.Add("SCOUTSURVIVOR_EQUIPMENT_NONE_DESCRIPTION", desc);

            desc = "Use to gain <style=cIsUtility>immunity to damage</style> but not knockback for 8 seconds." +
                "\n<style=cDeath>Unable to attack while active.</style>";
            LanguageAPI.Add("SCOUTSURVIVOR_EQUIPMENT_BONK_NAME", "None");
            LanguageAPI.Add("SCOUTSURVIVOR_EQUIPMENT_BONK_DESCRIPTION", desc);

            desc = "Use to deal <style=cIsDamage>+35% more damage</style>, but <style=cDeath>take +50% more damage</style> for 8 seconds.";
            LanguageAPI.Add("SCOUTSURVIVOR_EQUIPMENT_COLA_NAME", "None");
            LanguageAPI.Add("SCOUTSURVIVOR_EQUIPMENT_COLA_DESCRIPTION", desc);

            LanguageAPI.Add("KEYWORD_MILKING", "When dealing damage to a milked enemy, recover 5% of health in damage.");
            LanguageAPI.Add("KEYWORD_BLEEDING", "Deals damage over time.");
            LanguageAPI.Add("KEYWORD_DEATHMARKING", "The afflicted enemy takes +50% more damage.");
        }
    }
}
