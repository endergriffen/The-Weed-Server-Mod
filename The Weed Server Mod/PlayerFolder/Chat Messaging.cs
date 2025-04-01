using HarmonyLib;
using System.Collections.Generic;
using System;
using The_Weed_Server_Mod.ConfigManager;
using UnityEngine;
using The_Weed_Server_Mod.LobbyScreen;

namespace The_Weed_Server_Mod.PlayerFolder
{
    public class Chat_Messaging
    {
        public static Dictionary<string, List<string>> PlayerChatHistory = new Dictionary<string, List<string>>();

        public static string NotificationMessage { get; set; }

        // List of target words
        public static List<string> BadWordList = new List<string>
        {
            "skinwalker"
        };

        public static List<string> CommandWordList = new List<string>
        {
            "/display"
        };

        [HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.ChatMessageSendRPC))]
        [HarmonyPostfix]
        public static void ChatMessageSendRPC_Postfix(PlayerAvatar __instance, string _message, bool crouching)
        {
            var playerNameTraverse = Traverse.Create(__instance).Field("playerName");
            if (!playerNameTraverse.FieldExists()) return;
            string playerName = playerNameTraverse.GetValue() as string ?? "Unknown";

            if (string.IsNullOrEmpty(_message)) return;

            // Check for bad words
            foreach (string targetWord in BadWordList)
            {
                if (_message.IndexOf(targetWord, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Plugin.Instance.mls.LogInfo($"[BADWORD DETECTED] {playerName} said '{targetWord}'!");
                    NotificationMessage = $"{playerName} SAID '{targetWord}'";
                }
            }

            // Check if the message is a command
            foreach (string command in CommandWordList)
            {
                if (_message.StartsWith(command, StringComparison.OrdinalIgnoreCase))
                {
                    ChatManager chatManagerInstance = UnityEngine.Object.FindObjectOfType<ChatManager>();
                    if (chatManagerInstance != null)
                    {
                        // Extract the message after "/display"
                        string messageContent = _message.Substring(command.Length).Trim();
                        if (string.IsNullOrEmpty(messageContent)) messageContent = "No message provided.";

                        Custom_Screen_API.AddChatMessage(chatManagerInstance, messageContent);
                        Plugin.Instance.mls.LogInfo($"[COMMAND DETECTED] {playerName} used command '{command}' with message: '{messageContent}'!");
                    }
                }
            }

            // Store chat history
            if (!PlayerChatHistory.ContainsKey(playerName))
            {
                PlayerChatHistory[playerName] = new List<string>();
            }
            PlayerChatHistory[playerName].Add(_message);

            Plugin.Instance.mls.LogInfo($"[CHAT] {playerName}: {_message}");
        }
    }
}