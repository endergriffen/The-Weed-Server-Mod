using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using The_Weed_Server_Mod.ConfigManager;
using The_Weed_Server_Mod.UIElement;
using The_Weed_Server_Mod.PlayerFolder;
using System;
using UnityEngine;
using The_Weed_Server_Mod.TruckScreen;
using The_Weed_Server_Mod.ChatMessaging;
using System.Collections.Generic;

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

        private static readonly List<Type> patchClasses = new List<Type>
        {
            typeof(Plugin),
            typeof(Configs),

            typeof(PlayerData),
            typeof(Player_Statistics),
            typeof(Connected_Players),
            typeof(Keybind_Detection),
            typeof(Death_and_Revive_Detection),
            typeof(Inventory_Detection),
            typeof(Ping_Detection),
            typeof(Tumble_Detection),
            typeof(Upgrade_Detection),

            typeof(Host_Commands),
            typeof(Track_Chat_Messages),

            typeof(Poll_State_Manager),
            typeof(Display_Display_Command),
            typeof(Display_Level_Command),
            typeof(Display_Poll_Command),

            typeof(Notification_Message),
            typeof(UI_Setup),
            typeof(UI_Base)
        };

        public readonly Harmony harmony = new Harmony(modGUID);
        public static Plugin Instance;
        internal ManualLogSource mls;

        internal UI_Base Menu;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            UI_Base.menuRect = new Rect(UI_Base.MENUX, UI_Base.MENUY, UI_Base.MENUWIDTH, UI_Base.MENUHEIGHT);

            mls = BepInEx.Logging.Logger.CreateLogSource("TWSM");
            mls.LogInfo("The Weed Server Mod for REPO is alive!");

            Configs.Instance.Setup(Config);

            foreach (var patchClass in patchClasses)
            {
                try
                {
                    harmony.PatchAll(patchClass);
                    mls.LogInfo($"Successfully patched: {patchClass.Name}");
                }
                catch (Exception ex)
                {
                    mls.LogError($"Failed to patch: {patchClass.Name} - {ex.Message}");
                }
            }

            CreateMenu();

            CreateInputListener();
        }

        public void CreateMenu()
        {
            var gameObject = new UnityEngine.GameObject("UI_Base");
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            gameObject.AddComponent<UI_Base>();
            Menu = gameObject.AddComponent<UI_Base>();
        }

        private void CreateInputListener()
        {
            var inputListenerObject = new GameObject("UI_InputListener");
            UnityEngine.Object.DontDestroyOnLoad(inputListenerObject);
            inputListenerObject.hideFlags = HideFlags.HideAndDontSave;
            inputListenerObject.AddComponent<UI_Setup>();
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