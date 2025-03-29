﻿using BepInEx.Logging;
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
            "endergriffen" 
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