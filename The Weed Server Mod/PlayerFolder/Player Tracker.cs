using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

namespace The_Weed_Server_Mod.PlayerFolder
{
    public class Player_Tracker
    {
        private static List<string> onlinePlayers = new List<string>();
        public static string message { get; set; }

        [HarmonyPatch(typeof(PlayerController), "Update")]
        [HarmonyPostfix]
        public static void UpdateCycle(PlayerController __instance)
        {
            if (!PhotonNetwork.IsMasterClient) { return; }

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!onlinePlayers.Contains(player.NickName))
                {
                    onlinePlayers.Add(player.NickName);
                    Plugin.Instance.mls.LogInfo($"Player: {player.NickName} was logged in and was added to the list.");
                    message = $"{player.NickName} HAS CONNECTED";
                }
            }
        }

        public static List<string> GetOnlinePlayers()
        {
            return new List<string>(onlinePlayers);
        }

        [HarmonyPatch(typeof(NetworkManager), "OnPlayerLeftRoom")]
        [HarmonyPostfix]
        public static void PlayerDisconnected(NetworkManager __instance, Player otherPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) { return; }

            if (onlinePlayers.Contains(otherPlayer.NickName))
            {
                onlinePlayers.Remove(otherPlayer.NickName);
                Plugin.Instance.mls.LogInfo($"Player: {otherPlayer.NickName} has left the room and was removed from the list.");
                message = $"{otherPlayer.NickName} HAS DISCONNECTED";
            }
        }
    }
}