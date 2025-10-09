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
}