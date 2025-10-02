using FoodNullifier;
using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace FoodNullifier_ModSettings
{
    // Mod Settings Storage
    public class FoodNullifer_Settings : ModSettings
    {
        public bool AffectConsciousness = true;
        public int tickCount = 2;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref AffectConsciousness, "AffectConsciousness", true);
            Scribe_Values.Look(ref tickCount, "tickCount", 150);
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

            FN_Options.CheckboxLabeled("FN_Consciousness".Translate(), ref settings.AffectConsciousness);

            FN_Options.Label("FN_TickRate".Translate(settings.tickCount));
            settings.tickCount = (int)FN_Options.Slider(settings.tickCount, 1, 10);
            FN_Options.Label("FN_TickRateExp".Translate());

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
            int tickCount = settings.tickCount;
            int tickRate = tickCount * 60;

            if (settings.AffectConsciousness)
            {
                var def = DefDatabase<HediffDef>.GetNamed("FoodNullifier");
                var capMod = def.stages[0].capMods
                    .FirstOrDefault(r => r.capacity == PawnCapacityDefOf.Consciousness);

                if (capMod == null)
                {
                    def.stages[0].capMods.Add(new PawnCapacityModifier
                    {
                        capacity = PawnCapacityDefOf.Consciousness,
                        offset = -0.1f
                    });
                }
                else
                {
                    capMod.offset = -0.1f;
                }
            }
            else
            {
                // Remove the whole stages block
                DefDatabase<HediffDef>.GetNamed("FoodNullifier").stages = null;
            }

            var tickComp = DefDatabase<HediffDef>.GetNamed("FoodNullifier").comps.FirstOrDefault(c => c is HediffCompProperties_NullifyHunger) as HediffCompProperties_NullifyHunger;
            tickComp.tickInterval = tickRate;
        }
    }
}