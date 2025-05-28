using HarmonyLib;
using The_Weed_Server_Mod.UIElement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace The_Weed_Server_Mod.PlayerFolder
{
    [HarmonyPatch(typeof(PlayerController), "Update")]
    public static class Keybind_Detection
    {
        [HarmonyPostfix]
        public static void UpdateCycle(PlayerController __instance)
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                Plugin.Instance.mls.LogInfo("'P' Keybind has been pressed!");
                /*
                var uiManager = GameObject.Find("SimpleUIManager")?.GetComponent<SimpleUIWindow>();
                if (uiManager != null)
                {
                    bool isActive = uiManager.gameObject.activeSelf;
                    uiManager.ToggleUI(!isActive);
                }
                */
            }
        }
    }
}