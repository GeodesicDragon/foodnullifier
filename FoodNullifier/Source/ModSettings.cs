using FoodNullifier;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace FoodNullifier_ModSettings
{
    // Mod Settings Storage
    public class FoodNullifer_Settings : ModSettings
    {
        public bool AffectConsciousness = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref AffectConsciousness, "AffectConsciousness", true);
            base.ExposeData();
        }
    }

    // Settings Window
    public class FoodNullifier_SettingsWindow : Mod
    {
        public static FoodNullifer_Settings settings;

        public FoodNullifier_SettingsWindow(ModContentPack content) : base(content)
        {
            settings = GetSettings<FoodNullifer_Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard FN_Options = new Listing_Standard();
            FN_Options.Begin(inRect);

            // Only one option just now — affect consciousness
            FN_Options.CheckboxLabeled("FN_Consciousness".Translate(), ref settings.AffectConsciousness);
            FN_Options.Label("FN_ChangeNeedsRestart".Translate());

            FN_Options.End();
        }

        public override string SettingsCategory()
        {
            return "FN_ModName".Translate();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }
    }
}
