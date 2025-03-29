using HarmonyLib;
using UnityEngine;
using The_Weed_Server_Mod.ConfigManager;
using The_Weed_Server_Mod.PlayerFolder;

namespace The_Weed_Server_Mod.UIElement
{
    [HarmonyPatch(typeof(BigMessageUI), "Update")]
    public class Notification_Message
    {
        [HarmonyPostfix]
        public static void ShowMessage(BigMessageUI __instance)
        {
            if (!Configs.Instance.NotificationMessageConfig.Value)
            {
                Plugin.Instance.mls.LogInfo("'Notification_Message' Class Disabled!");
                return;
            }

            if (Player_Tracker.message != null && !string.IsNullOrEmpty(Player_Tracker.message))
            {
                Color textColor = new Color(0.5f, 1f, 0f);
                Color outlineColor = Color.black;

                __instance.BigMessage(Player_Tracker.message, "!", 30f, textColor, outlineColor);
                Traverse.Create(__instance).Field("bigMessageTimer").SetValue(Configs.Instance.NotificationMessageTimer.Value);

                Player_Tracker.message = string.Empty;
            }
        }
    }
}