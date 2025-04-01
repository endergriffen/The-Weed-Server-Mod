using HarmonyLib;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static ChatManager;

namespace The_Weed_Server_Mod.LobbyScreen
{
    [HarmonyPatch(typeof(ChatManager))]
    public static class Custom_Screen_API
    {
        public static void AddChatMessage(ChatManager __instance, string playerMessage)
        {
            __instance.StartCoroutine(DelayedChatMessage(__instance, 0.5f, playerMessage));
        }

        private static IEnumerator DelayedChatMessage(ChatManager __instance, float delay, string playerMessage)
        {
            yield return new WaitForSeconds(delay); // Wait before displaying the message

            __instance.PossessChatScheduleStart(-1);
            __instance.PossessChat(PossessChatID.SelfDestruct, playerMessage, 2f, Color.red, 0f, sendInTaxmanChat: true, 2);
            __instance.PossessChatScheduleEnd();
        }
    }
}