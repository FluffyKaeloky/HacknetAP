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
    public class GameMenuPatch
    {
        public static DateTime exitMenuTime = DateTime.MinValue;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameScreen), nameof(GameScreen.ExitScreen))]
        static void ExitScreenPatch()
        {
            exitMenuTime = DateTime.Now;
        }
    }
}
