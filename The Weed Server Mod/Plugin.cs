using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using Photon.Pun;
using The_Weed_Server_Mod.ConfigManager;
using The_Weed_Server_Mod.UIElement;
using The_Weed_Server_Mod.PlayerFolder;
using System;
using The_Weed_Server_Mod.LobbyScreen;

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

            harmony.PatchAll(typeof(Notification_Message));

            harmony.PatchAll(typeof(Player_Tracker));
            harmony.PatchAll(typeof(Player_Update));
            harmony.PatchAll(typeof(Chat_Messaging));
            harmony.PatchAll(typeof(Cohost));

            harmony.PatchAll(typeof(Custom_Screen_API));
        }
    }
}

/*
Rules:
    No Mod Based Dependencies - (It can not rely on other mods to work)
    All Features MUST be Togglable - (If anyone adds a feature to the mod i.e. an item, the host has to be allowed to turn the feature ON or OFF)
    All Features must be Stable - (If a feature causes instability issues with the entire mod or sections of it, it either needs to be reworked or deleted until the instability issues are fixed)
    No Duplicate Features - (If a feature is too similar to another feature the new feature must either be reworked or deleted until the new feature is different enough to the previous feature)
    No Modifying Other Creators Features Without Permission - (A creator MUST get permission if they want to edit or delete another creators work)
    No Exploits - (If a feature is used to make a specific player godlike, it must be reworked or deleted)

Goals of the Mod:
    The goal of this mod is to cut down on creating individual modpacks/mods by making one centralized mod that all players in the server can download/update with their own work.

Current Features:
    Makes an in game UI messenger that displays a any message
    Makes a list to track connected players, then once players leave they will be removed from the list
    Makes a chat messaging system that read and logs all chat messages, then if a player says the funny word in chat it will notify the host via 'Notification_Message'
    A custom screen API that allows for the creation of custom screens, like the display command that will display a message on the screen
*/