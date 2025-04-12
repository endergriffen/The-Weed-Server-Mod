using HarmonyLib;
using UnityEngine.InputSystem;

namespace The_Weed_Server_Mod.PlayerFolder
{
    [HarmonyPatch(typeof(PlayerController), "Update")]
    public static class Player_Update
    {
        [HarmonyPostfix]
        public static void UpdateCycle(PlayerController __instance)
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                Plugin.Instance.mls.LogInfo("'P' Keybind has been pressed!");
            }
        }
    }
}