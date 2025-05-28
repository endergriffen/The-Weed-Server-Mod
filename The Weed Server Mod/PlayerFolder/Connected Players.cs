using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using The_Weed_Server_Mod.UIElement;

namespace The_Weed_Server_Mod.PlayerFolder
{
    public class Connected_Players
    {
        public class PlayerMessage
        {
            public string Message { get; set; }
            public Notification_Message.MessageType Type { get; set; }

            public PlayerMessage(string message, Notification_Message.MessageType type)
            {
                Message = message;
                Type = type;
            }
        }

        public static PlayerMessage NotificationMessage { get; set; }

        [HarmonyPatch(typeof(PlayerController), "Update")]
        [HarmonyPostfix]
        public static void UpdateCycle(PlayerController __instance)
        {
            if (!PhotonNetwork.IsMasterClient) { return; }

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!Player_Statistics.playerList.Contains(player.NickName))
                {
                    Player_Statistics.AddPlayer(player.NickName);
                    Player_Statistics.SetConnectionStatus(player.NickName, true);

                    Plugin.Instance.mls.LogInfo($"Player: {player.NickName} has joined and was added to the list.");
                    NotificationMessage = new PlayerMessage($"{player.NickName} HAS CONNECTED", Notification_Message.MessageType.Notification);
                }
                else
                {
                    Player_Statistics.SetConnectionStatus(player.NickName, true);
                }
            }
        }

        [HarmonyPatch(typeof(NetworkManager), "OnPlayerLeftRoom")]
        [HarmonyPostfix]
        public static void PlayerDisconnected(NetworkManager __instance, Player otherPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) { return; }

            if (Player_Statistics.playerList.Contains(otherPlayer.NickName))
            {
                Player_Statistics.SetConnectionStatus(otherPlayer.NickName, false);

                Plugin.Instance.mls.LogInfo($"Player: {otherPlayer.NickName} has left the room and was marked as disconnected.");
                NotificationMessage = new PlayerMessage($"{otherPlayer.NickName} HAS DISCONNECTED", Notification_Message.MessageType.Notification);
            }
        }
    }
}