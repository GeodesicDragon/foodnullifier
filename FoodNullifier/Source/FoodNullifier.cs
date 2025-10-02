using RimWorld;
using System.Reflection;
using Verse;

namespace FoodNullifier
{

    // GAME STARTUP

    [StaticConstructorOnStartup]
    public static class FoodNullifier_Startup
    {
        static FoodNullifier_Startup()
        {
            var FN_Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            Log.Message("FoodNullifier_WelcomeMessage".Translate(FN_Version));
        }
    }

    // HEDIFF
    public class HediffCompProperties_NullifyHunger : HediffCompProperties
    {
        public int tickInterval = 150;

        public HediffCompProperties_NullifyHunger()
        {
            this.compClass = typeof(HediffComp_NullifyHunger);
        }
    }

    public class HediffComp_NullifyHunger : HediffComp
    {
        private int tickCounter = 0;
        public HediffCompProperties_NullifyHunger Props => (HediffCompProperties_NullifyHunger)this.props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);

            tickCounter++;
            if (tickCounter >= Props.tickInterval)
            {
                tickCounter = 0;

                if (Pawn?.needs?.food != null && Pawn.needs.food.CurLevel < Pawn.needs.food.MaxLevel)
                {
                    Pawn.needs.food.CurLevel = Pawn.needs.food.MaxLevel;
                }
            }
        }
    }

}
