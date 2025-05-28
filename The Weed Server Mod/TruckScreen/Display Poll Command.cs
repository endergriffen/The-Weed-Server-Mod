using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace The_Weed_Server_Mod.TruckScreen
{
    public class Display_Poll_Command
    {
        public static void ShowPollMessage(string question, List<string> options)
        {
            Poll_State_Manager.StartPoll(question, options);
            TruckScreenText truckScreen = GameObject.FindObjectOfType<TruckScreenText>();
            if (truckScreen != null && truckScreen.TryGetComponent<PhotonView>(out var pv))
            {
                string formattedMsg = $"<color=#FFFF00><size=0.25>POLL: {question}</color></size>\n\n";
                foreach (var option in options)
                {
                    formattedMsg += $"<color=#FFFFFF><size=0.25>{option}</color></size>\n";
                    formattedMsg += $"<size=0.25><mark=#808080>{new string('l', Display_Level_Command.MIN_BAR_LENGTH)}</mark></size>\n\n";
                }
                pv.RPC("MessageSendCustomRPC", RpcTarget.All, "", formattedMsg);
            }
        }

        public static void UpdateVoteDisplay()
        {
            if (!Poll_State_Manager.IsPollActive) return;

            TruckScreenText truckScreen = GameObject.FindObjectOfType<TruckScreenText>();
            if (truckScreen != null && truckScreen.TryGetComponent<PhotonView>(out var pv))
            {
                string formattedMsg = $"<color=#FFFF00><size=0.25>POLL: {Poll_State_Manager.CurrentPollQuestion}</color></size>\n\n";
                int totalVotes = Poll_State_Manager.PollVotes.Values.Sum();

                foreach (var option in Poll_State_Manager.PollOptions)
                {
                    string key = option.ToUpper();
                    Poll_State_Manager.PollVotes.TryGetValue(key, out int votes);
                    float percentage = totalVotes > 0 ? (float)votes / totalVotes : 0;
                    int barLength = Display_Level_Command.MIN_BAR_LENGTH + (int)(percentage * (Display_Level_Command.MAX_BAR_LENGTH - Display_Level_Command.MIN_BAR_LENGTH));

                    formattedMsg += $"<color=#FFFFFF><size=0.25>{option} ({votes})</color></size>\n";
                    formattedMsg += $"<size=0.25><mark=#808080>{new string('l', barLength)}</mark></size>\n\n";
                }
                pv.RPC("MessageSendCustomRPC", RpcTarget.All, "", formattedMsg);
            }
        }

        public static void EndPoll()
        {
            TruckScreenText truckScreen = GameObject.FindObjectOfType<TruckScreenText>();
            if (truckScreen != null && truckScreen.TryGetComponent<PhotonView>(out var pv))
            {
                // Show final results
                string results = $"<color=#FFFF00>POLL RESULTS: {Poll_State_Manager.CurrentPollQuestion}</color>\n\n";
                var winningOption = Poll_State_Manager.PollVotes.OrderByDescending(kv => kv.Value).FirstOrDefault();

                foreach (var option in Poll_State_Manager.PollOptions)
                {
                    string key = option.ToUpper();
                    Poll_State_Manager.PollVotes.TryGetValue(key, out int votes);
                    results += option == winningOption.Key
                        ? $"<color=#FFFF00><size=0.25>{option}: {votes}</color></size>\n"
                        : $"<color=#FFFFFF><size=0.25>{option}: {votes}</color></size>\n";
                }
                results += $"\n<color=#00FF00>Winner: {winningOption.Key}</color>";
                pv.RPC("MessageSendCustomRPC", RpcTarget.All, "", results);
            }

            Poll_State_Manager.EndPoll();
        }
    }
}