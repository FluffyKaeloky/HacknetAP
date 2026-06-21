using Hacknet;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HacknetArchipelago.Patches
{
    [HarmonyPatch]
    internal class TutorialPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(OS), nameof(OS.addExe))]
        public static void AddExePrefix(OS __instance, ExeModule exe)
        {
            HacknetAPCore.Logger.LogDebug("Exe prefix");

            if (exe.IdentifierName == "Tutorial v16.2")
            {
                HacknetAPCore.Logger.LogDebug("Cast override");
                exe.ramCost = 1;
                exe.baseRamCost = 1;
            }
        }
    }
}
