using Verse;
using RimWorld;

namespace FoodNullifier
{
    public class Hediff_NullifyHunger : Hediff
    {
        public override void PostTick()
        {
            base.PostTick();

            // Ensure pawn and food need exist
            if (pawn?.needs?.food != null)
            {
                // Always keep hunger full
                pawn.needs.food.CurLevel = pawn.needs.food.MaxLevel;
            }
        }
    }
}