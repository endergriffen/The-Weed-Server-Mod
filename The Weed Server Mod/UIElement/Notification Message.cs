using HarmonyLib;
using UnityEngine;
using The_Weed_Server_Mod.ConfigManager;
using The_Weed_Server_Mod.PlayerFolder;

namespace The_Weed_Server_Mod.UIElement
{
    [HarmonyPatch(typeof(BigMessageUI), "Update")]
    public class Notification_Message
    {
        public enum MessageType
        {
            Notification,
            Warning,
            Alert
        }

        [HarmonyPostfix]
        public static void ShowMessage(BigMessageUI __instance)
        {
            if (!Configs.Instance.NotificationMessageConfig.Value)
            {
                Plugin.Instance.mls.LogInfo("'Notification_Message' Class Disabled!");
                return;
            }

            var msg = Connected_Players.NotificationMessage;
            if (msg != null && !string.IsNullOrEmpty(msg.Message))
            {
                Color textColor;
                Color outlineColor = Color.black;

                switch (msg.Type)
                {
                    case MessageType.Notification:
                        textColor = new Color(0.5f, 1f, 0f);
                        break;
                    case MessageType.Warning:
                        textColor = new Color(1f, 0.65f, 0f);
                        break;
                    case MessageType.Alert:
                        textColor = Color.red;
                        break;
                    default:
                        textColor = Color.blue;
                        break;
                }

                __instance.BigMessage(msg.Message, "!", 30f, textColor, outlineColor);
                Traverse.Create(__instance).Field("bigMessageTimer").SetValue(Configs.Instance.NotificationMessageTimer.Value);

                Connected_Players.NotificationMessage = null;
            }
        }
    }
}