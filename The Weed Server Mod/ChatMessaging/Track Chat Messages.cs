using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;

namespace The_Weed_Server_Mod.ChatMessaging
{
    public class Track_Chat_Messages
    {
        public static Dictionary<string, List<string>> PlayerChatHistory = new Dictionary<string, List<string>>();

        public static List<string> HostCommandList = new List<string>
        {
            "/display", "/poll", "/level", "/vote", "/endpoll", "/endlevel", "/results"
        };

        [HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.ChatMessageSendRPC))]
        [HarmonyPostfix]
        public static void ChatMessageSendRPC_Postfix(PlayerAvatar __instance, string _message, bool crouching)
        {
            var playerNameTraverse = Traverse.Create(__instance).Field("playerName");
            if (!playerNameTraverse.FieldExists()) return;
            string playerName = playerNameTraverse.GetValue() as string ?? "Unknown";

            if (string.IsNullOrEmpty(_message)) return;

            if (!PlayerChatHistory.ContainsKey(playerName))
            {
                PlayerChatHistory[playerName] = new List<string>();
            }
            PlayerChatHistory[playerName].Add(_message);

            Plugin.Instance.mls.LogInfo($"[CHAT] {playerName}: {_message}");

            if (PhotonNetwork.IsMasterClient && __instance.photonView.IsMine)
            {
                foreach (string command in HostCommandList)
                {
                    if (_message.StartsWith(command, StringComparison.OrdinalIgnoreCase))
                    {
                        string[] parts = _message.Split(' ');
                        Host_Commands.HandleCommand(command, parts, __instance);
                        return;
                    }
                }
            }
        }
    }
}