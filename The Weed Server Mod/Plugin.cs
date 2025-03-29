using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using The_Weed_Server_Mod.ConfigManager;
using The_Weed_Server_Mod.BigMessage;

namespace The_Weed_Server_Mod
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string modGUID = "Weed_And_Hope.The_Weed_Server_Mod";
        private const string modName = "The_Weed_Server_Mod";
        private const string modVersion = "1.0.0";

        private readonly string[] contributors = 
        { 
            "endergriffen",
            ""
        };

        public readonly Harmony harmony = new Harmony(modGUID);
        public static Plugin Instance;
        internal ManualLogSource mls;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource("TWSM");
            mls.LogInfo("The Weed Server Mod for REPO is alive!");

            Configs.Instance.Setup(Config);

            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(Configs));

            harmony.PatchAll(typeof(Big_Message_Handler));
        }
    }
}

/*
Rules:
    No Mod Based Dependencies - (It can not rely on other mods to work)
	All Features MUST be Togglable - (If anyone adds a feature to the mod i.e. an item, the host has to be allowed to turn the feature on/off)
	All Features must be Stable - (If a feature causes instability issues with the entire mod or sections of it, it either needs to be reworked or deleted until the instability issues are fixed)
	No Duplicate Features - (If a feature is too similar to another feature the new feature must either be reworked or deleted until the new feature is different enough to the previous feature)
	No Modifying Other Creators Features Without Permission - (A creator MUST get permission if they want to edit/delete another creators work)

Goals of the Mod:
	The goal of this mod is to cut down on creating individual modpacks/mods by making one centralized mod that all players in the server can download/update with their own work.

Current Features:
	Makes an in game UI messenger that displays a message based on the string config found in the 'Configs' file.

Update Changelog:
	- 
*/