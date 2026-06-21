using Hacknet;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HacknetArchipelago.Patches.Computers
{
    // Could be a basis to make computers inviolable if you don't have the check for Archipelago's Admin accesses checks. Would be fun, but would also require a logic map of the IPs you need to access in order of priority.
    [HarmonyPatch]
    public static class ComputerPatches
    {
        public static Dictionary<Computer, (int secLevel, int portsNeededToCrack)> overridenSecInstances = new Dictionary<Computer, (int secLevel, int portsNeededToCrack)>();

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Computer), nameof(Computer.connect))]
        public static void ConnectPatch(Computer __instance)
        {
            /*if (__instance.idName == OS.currentInstance.thisComputer.idName)
                return;

            overridenSecInstances.Add(__instance, (__instance.securityLevel, __instance.portsNeededForCrack));*/

            /*HacknetAPCore.Logger.LogDebug("Set sec level and ports to crack");
            __instance.securityLevel = 5;
            __instance.portsNeededForCrack = 99998;*/
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Computer), nameof(Computer.disconnecting))]
        public static void DisconnectingPatch(Computer __instance)
        {
            /*if (!overridenSecInstances.ContainsKey(__instance))
                return;

            (int secLevel, int portsNeededToCrack) restoreValues = overridenSecInstances[__instance];

            __instance.securityLevel = restoreValues.secLevel;
            __instance.portsNeededForCrack = restoreValues.portsNeededToCrack;

            overridenSecInstances.Remove(__instance);

            HacknetAPCore.Logger.LogDebug("Restored computer sec level");*/
        }
    }
}
