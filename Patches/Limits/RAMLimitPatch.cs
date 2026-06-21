using Hacknet;
using HacknetArchipelago.Managers;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Pathfinder.Event.Gameplay;
using System;

namespace HacknetArchipelago.Patches
{
    [HarmonyPatch]
    public class RAMLimitPatch
    {
        public static bool ramWasSet = false;
        internal static int _lastRamLimit = -1;

        public const int MINIMUM_RAM = 350;
        public const int RAM_UPGRADE_STEP = 50;
        public const int MAXIMUM_RAM = 800;

        public static void LimitRAM(OSUpdateEvent oSUpdateEvent)
        {
            if(HacknetAPCore.SlotData.LimitsShuffle != HacknetAPSlotData.LimitsMode.OnlyRAM &&
                HacknetAPCore.SlotData.LimitsShuffle != HacknetAPSlotData.LimitsMode.EnableAllLimits)
            {
                return;
            }
            if(InventoryManager._ramLimit == 0 || OS.currentInstance.initShowsTutorial) { return; }

            OS os = oSUpdateEvent.OS;

            int totalRam = GetRAMLimit();

            if(_lastRamLimit != totalRam)
            {
                if(OS.DEBUG_COMMANDS)
                {
                    HacknetAPCore.Logger.LogDebug($"Updating RAM to new value: {InventoryManager._ramLimit}");
                }

                os.ramAvaliable = totalRam;
                os.totalRam = totalRam - (OS.TOP_BAR_HEIGHT + 2);
                _lastRamLimit = totalRam;

                UpdateRamModule();
            }
        }

        public static int GetRAMLimit()
        {
            var ramUpgradesCollected = InventoryManager.ProgressiveRAMsCollected;
            int totalRam = MINIMUM_RAM + (ramUpgradesCollected * RAM_UPGRADE_STEP);

            totalRam = (int)MathHelper.Clamp(totalRam, MINIMUM_RAM, MAXIMUM_RAM);

            return totalRam;
        }

        public static void UpdateRamModule()
        {
            OS os = OS.currentInstance;

            UpdateRamModule(os);
        }

        public static void UpdateRamModule(OS os)
        {
            int originalLocation = 2;
            
            // Fix for having the RAM window in possibly different locations based on active x-server.
            if (os.ram != null)
                originalLocation = os.ram.Bounds.X;

            os.modules.Remove(os.ram);
            os.ram = new RamModule(new Rectangle(originalLocation, OS.TOP_BAR_HEIGHT,
                RamModule.MODULE_WIDTH, os.ramAvaliable + RamModule.contentStartOffset), os);
            os.ram.name = "RAM";
            os.modules.Add(os.ram);
        }

        // Fix for RAM amount when reloading a session.
        [HarmonyPostfix]
        [HarmonyPatch(typeof(OS), nameof (OS.LoadContent))]
        public static void LoadContentPostfix(OS __instance)
        {
            if (HacknetAPCore.SlotData.LimitsShuffle != HacknetAPSlotData.LimitsMode.OnlyRAM &&
                HacknetAPCore.SlotData.LimitsShuffle != HacknetAPSlotData.LimitsMode.EnableAllLimits)
            {
                return;
            }

            __instance.ramAvaliable = GetRAMLimit();
            __instance.totalRam = __instance.ramAvaliable - (OS.TOP_BAR_HEIGHT + 2);

            UpdateRamModule(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TutorialExe), MethodType.Constructor, new Type[] { typeof(Rectangle), typeof(OS) })]
        public static void TutorialNeededRAMFix(TutorialExe __instance, Rectangle location, OS operatingSystem)
        {
            __instance.baseRamCost = 1;
            __instance.ramCost = 1;
        }
    }
}
