using R2API;
using System;

namespace ROR2_SaxtonHale.Modules
{
    public static class Tokens
    {
        public static void AddTokens()
        {
            string desc = "Saxton Hale is a heavy hitting tank that can uses his superior biology to kill enemies (hippies).<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > (NOT IMPL) You can choose loadout depending on closely you want the game to feel like VSH: Standard and Classic." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Use superjump to quickly get to normally unreachable areas." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Perform a weighdown midair to quickly reach the ground after being airborne." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Use your rage in a tough spot to stun all enemies." + Environment.NewLine + Environment.NewLine;

            LanguageAPI.Add("SAXTONHALE_NAME", "Saxton Hale");
            LanguageAPI.Add("SAXTONHALE_DESCRIPTION", desc);
            LanguageAPI.Add("SAXTONHALE_SUBTITLE", "President & CEO of Mann. Co");
            LanguageAPI.Add("SAXTONHALE_LORE", "SAXTON HAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALE");
            LanguageAPI.Add("SAXTONHALE_OUTRO_FLAVOR", "..and so he left, ready to take back Mann. Co.");

            LanguageAPI.Add("SAXTONHALEBODY_DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add("SAXTONHALEBODY_NUDE_SKIN_NAME", "Ring of Fired");

            LanguageAPI.Add("SAXTONHALE_PASSIVE_CLASSIC_NAME", "Classic Passive");
            LanguageAPI.Add("SAXTONHALE_PASSIVE_CLASSIC_DESCRIPTION", "Saxton Hale receives 60% reduced knockback from all attacks. Hale instantly kill enemies he lands on if traveling fast enough. Hale has fall damage immunity.");

            LanguageAPI.Add("SAXTONHALE_PASSIVE_AUSTRALIUM_NAME", "Australium Empowered");
            LanguageAPI.Add("SAXTONHALE_PASSIVE_AUSTRALIUM_DESCRIPTION", "Saxton Hale receives 60% reduced knockback from all attacks. Hale deals 500% damage to enemies he lands on if traveling fast enough. Hale has fall damage immunity.");

            desc = "Hale swings his dangerous fists for <style=cIsDamage>200% damage</style>.";

            LanguageAPI.Add("SAXTONHALE_PRIMARY_FISTS_NAME", "Hale's Own Fists");
            LanguageAPI.Add("SAXTONHALE_PRIMARY_FISTS_DESCRIPTION", desc);

            desc = "Hold down to charge your jump, and let go to launch yourself. You are temporarily immune to knockback at the start.";

            LanguageAPI.Add("SAXTONHALE_SECONDARY_SUPERJUMP_NAME", "Brave Jump");
            LanguageAPI.Add("SAXTONHALE_SECONDARY_SUPERJUMP_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Crouch</style> on the ground for knockback immunity, or hold midair to quickly plummet towards the ground.";

            LanguageAPI.Add("SAXTONHALE_UTILITY_WEIGHDOWN_NAME", "Weighdown");
            LanguageAPI.Add("SAXTONHALE_UTILITY_WEIGHDOWN_DESCRIPTION", desc);

            desc = "Saxton lunges forward for 250% damage, stunning and knocking enemies back. Deals +50% increased damage to buildings.";

            LanguageAPI.Add("SAXTONHALE_UTILITY_LUNGE_NAME", "Brave Lunge");
            LanguageAPI.Add("SAXTONHALE_UTILITY_LUNGE_DESCRIPTION", desc);

            desc = "<style=cIsUtility>Stun</style> all enemies within 500m for 6 seconds.";

            LanguageAPI.Add("SAXTONHALE_SPECIAL_CLASSIC_NAME", "Classic Rage");
            LanguageAPI.Add("SAXTONHALE_SPECIAL_CLASSIC_DESCRIPTION", desc);

            desc = "+75% attack speed, -50% damage, and attacks have a 75% chance to ignite enemies for 8 seconds.";

            LanguageAPI.Add("SAXTONHALE_SPECIAL_BRAWL_NAME", "Brawl Rage");
            LanguageAPI.Add("SAXTONHALE_SPECIAL_BRAWL_DESCRIPTION", desc);

            LanguageAPI.Add("KEYWORD_SCARED", "Prevents movement and attacking for a duration.");

            LanguageAPI.Add("SAXTONHALE_SCARED_NAME", "SCARED!");
            LanguageAPI.Add("SAXTONHALE_SCARED_DESCRIPTION", "You're too scared to attack!");
        }
    }
}