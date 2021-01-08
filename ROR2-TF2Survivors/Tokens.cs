using R2API;
using System;

namespace ROR2_TF2Survivors
{
    public static class Tokens
    {
        public static void AddTokens()
        {
            // Mercs
            // Scout
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
                LanguageAPI.Add("SCOUTSURVIVOR_PRIMARY_FORCENATURE_DESCRIPTION", "Fire a wide spread of bullets that deal <style=cIsDamage>8x60% damage</style>.\nDeals <style=cIsUtility>increased knockback<style>.");

                desc = "Fire a stream of bullets that deal <style=cIsDamage>70% damage</style>.\n<style=cIsUtility>Holds 12. Adds 12 per mag.</style>";
                LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_PISTOL_NAME", "Pistol");
                LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_PISTOL_DESCRIPTION", desc);

                desc = "<style=cIsUtility>Bleeding.</style> Throw a cleaver for <style=cIsDamage>100% damage that bleeds the enemy</style> on hit. Deals a <style=cIsDamage>critical hit</style> at maximum range and will <style=cIsUtility>recharge 25% quicker</style>.";
                LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_CLEAVER_NAME", "The Flying Guiollotine");
                LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_CLEAVER_DESCRIPTION", desc);

                desc = "<style=cIsUtility>Milking.</style> Throw a jar to douse enemies, inflicting <style=cIsUtility>Milked</style>. " +
                    "Allies that hit <style=cIsUtility>milked</style> enemies <style=cIsHealing> heal 5% of damage dealt.</style>" +
                    "Can extinguish 25% of burn stacks off an ally.";
                LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_MILK_NAME", "Mad Milk");
                LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_MILKL_DESCRIPTION", desc);

                desc = "<style=cIsUtility>Death-Marking.</style> Swing your weapon for <style=cIsDamage>30% damage</style> and <style=cIsDamage>mark the enemy for death</style>." +
                    " <style=cDeath>Only one enemy may be marked at a time</style>.";
                LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_MARKDEATH_NAME", "Fan O' War");
                LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_MARKDEATH_DESCRIPTION", desc);

                desc = "<style=cIsUtility>Stunning.</style> Swing and launch a ball for <style=cIsDamage>70% damage + 40% damage</style>. If you hit an enemy at the maximum range, it stuns the enemy.";
                LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_SANDMAN_NAME", "Sandman");
                LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_SANDMAN_DESCRIPTION", desc);

                desc = "<style=cIsUtility>Bleeding.</style> Swing your club for <style=cIsDamage>70% damage</style> and <style=cIsUtility>bleed the enemy</style>.";
                LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_BLEEDBAT_NAME", "Boston Basher");
                LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_BLEEDBAT_DESCRIPTION", desc);

                desc = "Gain +50% attack speed, +50% movement speed, and max utility charges for 8 seconds.";
                LanguageAPI.Add("SCOUTSURVIVOR_SPECIAL_FF2_NAME", "Classic Rage");
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


            // Saxton Hale
            {
                string desc = "Saxton Hale is a heavy hitting tank that can uses his superior biology to kill enemies (hippies).<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
                desc = desc + "< ! > You can choose loadout depending on closely you want the game to feel like VSH: Standard and Classic." + Environment.NewLine + Environment.NewLine;
                desc = desc + "< ! > Use superjump to quickly get to normally unreachable areas.." + Environment.NewLine + Environment.NewLine;
                desc = desc + "< ! > Perform a weighdown midair to quickly reach the ground after being airborne.." + Environment.NewLine + Environment.NewLine;
                desc = desc + "< ! > Use your rage in a tough spot to stun all nearby enemies." + Environment.NewLine + Environment.NewLine;

                LanguageAPI.Add("SCOUTSURVIVOR_NAME", "Saxton Hale");
                LanguageAPI.Add("SCOUTSURVIVOR_DESCRIPTION", desc);
                LanguageAPI.Add("SCOUTSURVIVOR_SUBTITLE", "President & CEO of Mann. Co");
                LanguageAPI.Add("SCOUTSURVIVOR_LORE", "SAXTON HAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALE");
                LanguageAPI.Add("SCOUTSURVIVOR_OUTRO_FLAVOR", "..and so he left, ready to take back Mann. Co.");

                LanguageAPI.Add("SCOUTSURVIVORBODY_DEFAULT_SKIN_NAME", "Default");
                LanguageAPI.Add("SCOUTSURVIVORBODY_LUNAR_SKIN_NAME", "Lunar");

                LanguageAPI.Add("SCOUTSURVIVOR_PASSIVE_CLASSIC_NAME", "Classic Passive");
                LanguageAPI.Add("SCOUTSURVIVOR_PASSIVE_CLASSIC_DESCRIPTION", "Saxton Hale receives 60% reduced knockback from all attacks. Hale instantly kill enemies he lands on if traveling fast enough. Hale has fall damage immunity.");

                LanguageAPI.Add("SCOUTSURVIVOR_PASSIVE_AUSTRALIUM_NAME", "Australium Empowered");
                LanguageAPI.Add("SCOUTSURVIVOR_PASSIVE_AUSTRALIUM_DESCRIPTION", "Saxton Hale receives 60% reduced knockback from all attacks. Hale deals 500% damage to enemies he lands on if traveling fast enough. Hale has fall damage immunity.");

                desc = "Hale swings his dangerous fists for <style=cIsDamage>200% damage</style>.";

                LanguageAPI.Add("SCOUTSURVIVOR_PRIMARY_FISTS_NAME", "Hale's Own Fists");
                LanguageAPI.Add("SCOUTSURVIVOR_PRIMARY_FISTS_DESCRIPTION", desc);

                desc = "Hold down to charge your jump, and let go to launch yourself. You are temporarily immune to knockback at the start.";

                LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_SUPERJUMP_NAME", "Brave Jump");
                LanguageAPI.Add("SCOUTSURVIVOR_SECONDARY_SUPERJUMP_DESCRIPTION", desc);

                desc = "<style=cIsUtility>Crouch</style> on the ground for knockback immunity, or hold midair to quickly plummet towards the ground.";

                LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_WEIGHDOWN_NAME", "Weighdown");
                LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_WEIGHDOWN_DESCRIPTION", desc);

                desc = "Saxton lunges forward for 250% damage, stunning and knocking enemies back. Deals +50% increased damage to buildings.";

                LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_LUNGE_NAME", "Brave Lunge");
                LanguageAPI.Add("SCOUTSURVIVOR_UTILITY_LUNGE_DESCRIPTION", desc);

                desc = "<style=cIsUtility>Stun</style> all enemies within 500m for 6 seconds.";

                LanguageAPI.Add("SCOUTSURVIVOR_SPECIAL_CLASSIC_NAME", "Classic Rage");
                LanguageAPI.Add("SCOUTSURVIVOR_SPECIAL_CLASSIC_DESCRIPTION", desc);

                desc = "+75% attack speed, -50% damage, and attacks have a 75% chance to ignite enemies for 8 seconds.";

                LanguageAPI.Add("SCOUTSURVIVOR_SPECIAL_BRAWL_NAME", "Brawl Rage");
                LanguageAPI.Add("SCOUTSURVIVOR_SPECIAL_BRAWL_DESCRIPTION", desc);

                LanguageAPI.Add("KEYWORD_SCARED", "Prevents movement and attacking for a duration.");

            }
        }
    }
}
