using Photon.Pun;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace The_Weed_Server_Mod.TruckScreen
{
    public class Display_Level_Command
    {
        // Define all available levels with their colors and mark colors
        public static readonly List<LevelInfo> AllLevels = new List<LevelInfo>
        {
            new LevelInfo("MANOR", "#D4A276", "#6f4518"),
            new LevelInfo("WIZARD", "#7241b4", "#502d7e"),
            new LevelInfo("ARTIC", "#7CA1C2", "#3C6080"),
            new LevelInfo("MUSEUM", "#ffffff", "#808080")
        };

        // Constants for the voting bar
        public const int MIN_BAR_LENGTH = 3;  // Minimum bar length (0%)
        public const int MAX_BAR_LENGTH = 70; // Maximum bar length (100%)

        public static void ShowLevelMessage()
        {
            // Randomly select 3 levels
            List<LevelInfo> selectedLevels = GetRandomLevels(3);

            // Update state - this will end any active poll
            Poll_State_Manager.StartLevelDecision(selectedLevels);

            TruckScreenText truckScreen = GameObject.FindObjectOfType<TruckScreenText>();
            if (truckScreen != null)
            {
                PhotonView pv = truckScreen.GetComponent<PhotonView>();
                if (pv != null)
                {
                    // Build the formatted message
                    string formattedLevel = "<color=#00FF00><size=0.25>VOTE ON THE NEXT LEVEL WITH /vote {level}:</color></size>\n\n";

                    foreach (var level in selectedLevels)
                    {
                        // Start with minimum bar length (0%)
                        string bar = new string('l', MIN_BAR_LENGTH);
                        formattedLevel += $"<color={level.TextColor}><size=0.25>{level.Name}:</color></size>\n <size=0.25><mark={level.MarkColor}>{bar}</mark></size>\n\n";
                    }

                    pv.RPC("MessageSendCustomRPC", RpcTarget.All, "", formattedLevel);
                }
                else
                {
                    Plugin.Instance.mls.LogWarning("TruckScreenText does not have a PhotonView component.");
                }
            }
            else
            {
                Plugin.Instance.mls.LogWarning("TruckScreenText instance not found.");
            }
        }

        public static void UpdateVoteDisplay()
        {
            if (!Poll_State_Manager.IsLevelDecisionActive)
            {
                return;
            }

            var voteResults = Poll_State_Manager.GetVoteResults();
            int totalVotes = Poll_State_Manager.GetTotalVotes();

            TruckScreenText truckScreen = GameObject.FindObjectOfType<TruckScreenText>();
            if (truckScreen != null && totalVotes > 0)
            {
                PhotonView pv = truckScreen.GetComponent<PhotonView>();
                if (pv != null)
                {
                    string formattedLevel = "<color=#00FF00><size=0.25>VOTE ON THE NEXT LEVEL WITH /vote {level}:</color></size>\n\n";

                    foreach (var level in Poll_State_Manager.AvailableLevels)
                    {
                        int votes = voteResults.ContainsKey(level.Name.ToUpper()) ? voteResults[level.Name.ToUpper()] : 0;

                        // Calculate bar length based on vote percentage
                        float percentage = totalVotes > 0 ? (float)votes / totalVotes : 0;
                        int barLength = MIN_BAR_LENGTH + (int)Math.Round(percentage * (MAX_BAR_LENGTH - MIN_BAR_LENGTH));
                        string bar = new string('l', barLength);

                        formattedLevel += $"<color={level.TextColor}><size=0.25>{level.Name} ({votes}):</color></size>\n <size=0.25><mark={level.MarkColor}>{bar}</mark></size>\n\n";
                    }

                    pv.RPC("MessageSendCustomRPC", RpcTarget.All, "", formattedLevel);
                }
            }
        }

        public static void EndLevelDecision()
        {
            Poll_State_Manager.EndLevelDecision();

            // Optionally clear the truck screen or display a message that the level decision has ended
            TruckScreenText truckScreen = GameObject.FindObjectOfType<TruckScreenText>();
            if (truckScreen != null)
            {
                PhotonView pv = truckScreen.GetComponent<PhotonView>();
                if (pv != null)
                {
                    pv.RPC("MessageSendCustomRPC", RpcTarget.All, "", "<color=#FF0000>Level voting has ended</color>");
                }
            }
        }

        public static void ShowVoteResults()
        {
            if (!Poll_State_Manager.IsLevelDecisionActive)
            {
                return;
            }

            var voteResults = Poll_State_Manager.GetVoteResults();
            int totalVotes = Poll_State_Manager.GetTotalVotes();
            string winningLevel = Poll_State_Manager.GetWinningLevel();

            TruckScreenText truckScreen = GameObject.FindObjectOfType<TruckScreenText>();
            if (truckScreen != null)
            {
                PhotonView pv = truckScreen.GetComponent<PhotonView>();
                if (pv != null)
                {
                    string resultsMessage = "<color=#00FF00>LEVEL VOTE RESULTS:</color>\n\n";

                    foreach (var level in Poll_State_Manager.AvailableLevels)
                    {
                        int votes = voteResults.ContainsKey(level.Name.ToUpper()) ? voteResults[level.Name.ToUpper()] : 0;
                        string votePercentage = totalVotes > 0 ? $"{(votes * 100) / totalVotes}%" : "0%";

                        // Calculate bar length based on vote percentage
                        float percentage = totalVotes > 0 ? (float)votes / totalVotes : 0;
                        int barLength = MIN_BAR_LENGTH + (int)Math.Round(percentage * (MAX_BAR_LENGTH - MIN_BAR_LENGTH));
                        string bar = new string('l', barLength);

                        // Highlight the winning level
                        if (level.Name.ToUpper() == winningLevel)
                        {
                            resultsMessage += $"<color=#FFFF00><size=0.25>{level.Name}: {votes} votes ({votePercentage})</color></size>\n";
                            resultsMessage += $"<size=0.25><mark=#FFFF00>{bar}</mark></size>\n\n";
                        }
                        else
                        {
                            resultsMessage += $"<color={level.TextColor}><size=0.25>{level.Name}: {votes} votes ({votePercentage})</color></size>\n";
                            resultsMessage += $"<size=0.25><mark={level.MarkColor}>{bar}</mark></size>\n\n";
                        }
                    }

                    if (totalVotes > 0)
                    {
                        resultsMessage += $"<color=#FFFF00><size=0.25>WINNER: {winningLevel}</color></size>";
                    }
                    else
                    {
                        resultsMessage += "<color=#FF0000><size=0.25>No votes received</color></size>";
                    }

                    pv.RPC("MessageSendCustomRPC", RpcTarget.All, "", resultsMessage);
                }
            }
        }

        private static List<LevelInfo> GetRandomLevels(int count)
        {
            // Make sure we don't try to select more levels than are available
            count = Math.Min(count, AllLevels.Count);

            // Create a copy of the list to avoid modifying the original
            List<LevelInfo> availableLevels = new List<LevelInfo>(AllLevels);
            List<LevelInfo> selectedLevels = new List<LevelInfo>();

            // Select random levels
            System.Random random = new System.Random();
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(availableLevels.Count);
                selectedLevels.Add(availableLevels[index]);
                availableLevels.RemoveAt(index);
            }

            return selectedLevels;
        }
    }

    // Helper class to store level information
    public class LevelInfo
    {
        public string Name { get; }
        public string TextColor { get; }
        public string MarkColor { get; }

        public LevelInfo(string name, string textColor, string markColor)
        {
            Name = name;
            TextColor = textColor;
            MarkColor = markColor;
        }
    }
}

/*
$"<color=#D4A276>MANOR:</color>\n <mark=#6f4518>lll \n" +
$"<color=#8C60C6>WIZARD:</color>\n <mark=#5e2f98>lll \n" +
$"<color=#7CA1C2>WIZARD:</color>\n <mark=#3C6080>lll \n";
*/