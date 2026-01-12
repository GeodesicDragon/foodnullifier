using FoodNullifier_ModSettings;
using RimWorld;
using System.Reflection;
using System.Xml;
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

    // PATCH OPERATION TO ALTER CONSCIOUSNESS
    public class PatchOperationFNConsciousnessCheck : PatchOperation
    {
        public string defName;

        protected override bool ApplyWorker(XmlDocument xml)
        {
            // Pull the current setting

            string reduceConsYes = "-0.1";
            string reduceConsNo = "0";

            // Build the xpath dynamically
            string xpath = $"Defs/HediffDef[defName=\"{defName}\"]/stages/li/capMods/li/offset";

            XmlNode node = xml.SelectSingleNode(xpath);
            if (node != null)
            {
                if (FoodNullifier_SettingsWindow.settings.AffectConsciousness == true)
                {
                    node.InnerText = reduceConsYes;
                }
                else if (FoodNullifier_SettingsWindow.settings.AffectConsciousness == false)
                {
                    node.InnerText = reduceConsNo;
                }
                return true;
            }

            return false;
        }
    }
}