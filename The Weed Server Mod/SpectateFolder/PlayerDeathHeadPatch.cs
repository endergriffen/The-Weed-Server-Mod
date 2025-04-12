using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace The_Weed_Server_Mod.SpectateFolder
{
    [HarmonyPatch(typeof(PlayerDeathHead))]
    public class PlayerDeathHeadPatch
    {
        public static bool isSpectating = false;

        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        private static void PreFixUpdate(PlayerDeathHead __instance, PhysGrabObject ___physGrabObject)
        {
            if (__instance == null || ___physGrabObject == null ||
                __instance.playerAvatar == null || __instance.playerAvatar.photonView == null ||
                !__instance.playerAvatar.photonView.IsMine ||
                !(bool)AccessTools.Field(typeof(PlayerDeathHead), "triggered").GetValue(__instance))
            {
                return;
            }

            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                isSpectating = !isSpectating;
                if (isSpectating)
                {
                    AccessTools.Field(typeof(SpectateCamera), "player").SetValue(SpectateCamera.instance, __instance.playerAvatar);
                }
            }

            if (isSpectating)
            {
                ((Component)__instance.playerAvatar).transform.position =
                    ((Component)___physGrabObject).transform.position;
            }
        }

        public static bool CanSpectate()
        {
            return true;
        }
    }
}