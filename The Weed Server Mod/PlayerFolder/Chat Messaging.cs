using HarmonyLib;
using System.Collections.Generic;
using System;
using The_Weed_Server_Mod.ConfigManager;

namespace The_Weed_Server_Mod.PlayerFolder
{
    public class Chat_Messaging
    {
        public static Dictionary<string, List<string>> PlayerChatHistory = new Dictionary<string, List<string>>();

        public static string NotificationMessage { get; set; }

        [HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.ChatMessageSendRPC))]
        [HarmonyPostfix]
        public static void ChatMessageSendRPC_Postfix(PlayerAvatar __instance, string _message, bool crouching)
        {
            string targetWord = Configs.Instance.ExampleStringConfig.Value;

            var playerNameTraverse = Traverse.Create(__instance).Field("playerName");
            if (!playerNameTraverse.FieldExists()) return;
            string playerName = playerNameTraverse.GetValue() as string ?? "Unknown";

            if (string.IsNullOrEmpty(_message)) return;

            if (!string.IsNullOrEmpty(targetWord) && _message.IndexOf(targetWord, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Plugin.Instance.mls.LogInfo($"[BADWORD DETECTED] {playerName} said '{targetWord}'!");
                NotificationMessage = $"{playerName} SAID '{targetWord}'";
            }

            if (!PlayerChatHistory.ContainsKey(playerName))
            {
                PlayerChatHistory[playerName] = new List<string>();
            }
            PlayerChatHistory[playerName].Add(_message);

            Plugin.Instance.mls.LogInfo($"[CHAT] {playerName}: {_message}");
        }
    }
}