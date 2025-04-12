/*
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine.Windows;
using UnityEngine;
using UnityEngine.InputSystem;

namespace The_Weed_Server_Mod.SpectateFolder
{
    [HarmonyPatch(typeof(PlayerDeathHead))]
    public class DuckSpectateController
    {
        public static bool isSpectating = false;
        private EnemyDuck currentDuck;

        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        void Update()
        {
            // Toggle spectate mode when F is pressed.
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                Plugin.Instance.mls.LogInfo("'F' Keybind has been pressed!");

                isSpectating = !isSpectating;
                if (isSpectating)
                {
                    // Find any active duck enemy in the scene.
                    currentDuck = FindObjectOfType<EnemyDuck>();
                    if (currentDuck != null)
                    {
                        // Set the spectate camera's target to the duck enemy's transform.
                        SetSpectateTarget(currentDuck.transform);
                        Plugin.Instance.mls.LogInfo($"Now spectating duck enemy: {currentDuck.name}");
                    }
                    else
                    {
                        Plugin.Instance.mls.LogWarning("No duck enemy found to spectate.");
                        isSpectating = false;
                    }
                }
                else
                {
                    currentDuck = null;
                }
            }

            // If spectating, continuously update the target (in case the duck enemy moves).
            if (isSpectating && currentDuck != null)
            {
                SetSpectateTarget(currentDuck.transform);
            }
        }

        // Helper method to update the spectate camera target.
        private void SetSpectateTarget(Component target)
        {
            // Assuming the SpectateCamera singleton has a field "player" which we can set.
            // We use reflection via Harmony's AccessTools.
            Type spectateCameraType = typeof(SpectateCamera);
            FieldInfo playerField = AccessTools.Field(spectateCameraType, "player");
            if (playerField != null && SpectateCamera.instance != null)
            {
                playerField.SetValue(SpectateCamera.instance, target);
            }
        }
    }
}
*/