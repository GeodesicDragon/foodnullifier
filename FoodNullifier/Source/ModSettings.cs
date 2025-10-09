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

            // Only one option now — affect consciousness
            FN_Options.CheckboxLabeled("FN_Consciousness".Translate(), ref settings.AffectConsciousness);

            FN_Options.End();
        }

        public override string SettingsCategory()
        {
            return "FN_ModName".Translate();
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
            FoodNullifier_ApplySettings();
        }

        public static void FoodNullifier_ApplySettings()
        {
            var def = DefDatabase<HediffDef>.GetNamedSilentFail("FoodNullifier");
            if (def == null)
            {
                Log.Error("[Food Nullifier] Could not find HediffDef 'FoodNullifier'");
                return;
            }

            if (settings.AffectConsciousness)
            {
                if (def.stages == null || def.stages.Count == 0)
                {
                    def.stages = new List<HediffStage> { new HediffStage() };
                }

                var stage = def.stages[0];
                if (stage.capMods == null)
                    stage.capMods = new List<PawnCapacityModifier>();

                var capMod = stage.capMods.FirstOrDefault(r => r.capacity == PawnCapacityDefOf.Consciousness);
                if (capMod == null)
                {
                    stage.capMods.Add(new PawnCapacityModifier
                    {
                        capacity = PawnCapacityDefOf.Consciousness,
                        offset = -0.1f
                    });
                }
                else
                {
                    capMod.offset = -0.1f;
                }

                Log.Message("[Food Nullifier] Consciousness penalty applied (-0.1)");
            }
            else
            {
                if (def.stages != null)
                {
                    foreach (var stage in def.stages)
                    {
                        stage.capMods?.RemoveAll(cm => cm.capacity == PawnCapacityDefOf.Consciousness);
                    }
                }

                Log.Message("[Food Nullifier] Consciousness penalty removed");
            }

            // 🔁 Force all pawns to recalculate their health and capacities
            foreach (var pawn in PawnsFinder.AllMapsAndWorld_Alive)
            {
                if (pawn.health?.hediffSet?.hediffs?.Any(h => h.def == def) ?? false)
                {
                    pawn.health.capacities.Notify_CapacityLevelsDirty();
                    pawn.health.Notify_HediffChanged(null);
                }
            }
        }
    }
}
