using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using The_Weed_Server_Mod.TruckScreen;

namespace The_Weed_Server_Mod.ChatMessaging
{
    public class Host_Commands
    {
        public static readonly string[] HostCommandList = { "/display", "/poll", "/level", "/vote", "/endpoll", "/endlevel", "/results" };

        public static void HandleCommand(string command, string[] args, PlayerAvatar sender)
        {
            switch (command.ToLower())
            {
                case "/display":
                    HandleDisplayCommand(args, sender);
                    break;
                case "/poll":
                    HandlePollCommand(args, sender);
                    break;
                case "/level":
                    HandleLevelCommand(args, sender);
                    break;
                case "/vote":
                    HandleVoteCommand(args, sender);
                    break;
                case "/endpoll":
                    HandleEndPollCommand(args, sender);
                    break;
                case "/endlevel":
                    HandleEndLevelCommand(args, sender);
                    break;
                case "/results":
                    HandleResultsCommand(args, sender);
                    break;
                default:
                    break;
            }
        }

        private static void HandleDisplayCommand(string[] args, PlayerAvatar sender)
        {
            if (args.Length > 1)
            {
                string message = string.Join(" ", args.Skip(1));
                Display_Display_Command.ShowDisplayMessage(message);
                Plugin.Instance.mls.LogInfo($"[DISPLAY] Showing message: {message}");
            }
        }

        private static void HandlePollCommand(string[] args, PlayerAvatar sender)
        {
            if (args.Length > 1)
            {
                string input = string.Join(" ", args.Skip(1));
                int bracketIndex = input.IndexOf('[');
                int closingBracketIndex = input.LastIndexOf(']');

                if (bracketIndex != -1 && closingBracketIndex != -1 && bracketIndex < closingBracketIndex)
                {
                    string question = input.Substring(0, bracketIndex).Trim();
                    string optionsPart = input.Substring(bracketIndex + 1, closingBracketIndex - bracketIndex - 1).Trim();
                    List<string> options = optionsPart.Split('/').Select(o => o.Trim()).Where(o => !string.IsNullOrEmpty(o)).ToList();

                    if (options.Count >= 2)
                    {
                        Display_Poll_Command.ShowPollMessage(question, options);
                        Plugin.Instance.mls.LogInfo($"[POLL] Started: '{question}' with options: {string.Join(", ", options)}");
                    }
                    else
                    {
                        Plugin.Instance.mls.LogInfo("[POLL] Rejected: At least two options required.");
                    }
                }
                else
                {
                    Plugin.Instance.mls.LogInfo("[POLL] Rejected: Format must be '/poll Question [Option1/Option2]'.");
                }
            }
        }

        private static void HandleLevelCommand(string[] args, PlayerAvatar sender)
        {
            Display_Level_Command.ShowLevelMessage();
            Plugin.Instance.mls.LogInfo($"[LEVEL] Starting level voting");
        }

        private static void HandleVoteCommand(string[] args, PlayerAvatar sender)
        {
            string playerName = GetPlayerName(sender);

            // First check for poll votes
            if (Poll_State_Manager.IsPollActive)
            {
                if (args.Length < 2)
                {
                    Plugin.Instance.mls.LogInfo($"[VOTE] Rejected: No option specified by {playerName}");
                    return;
                }

                string option = args[1].ToUpper();
                bool success = Poll_State_Manager.RegisterPollVote(playerName, option);

                if (success)
                {
                    Plugin.Instance.mls.LogInfo($"[VOTE] {playerName} voted for {option}");
                    Display_Poll_Command.UpdateVoteDisplay();
                }
                else
                {
                    Plugin.Instance.mls.LogInfo($"[VOTE] Rejected: Invalid option '{option}' from {playerName}");
                }
                return;
            }

            // Then check for level votes
            if (Poll_State_Manager.IsLevelDecisionActive)
            {
                if (args.Length < 2)
                {
                    Plugin.Instance.mls.LogInfo($"[VOTE] Rejected: No level specified by {playerName}");
                    return;
                }

                string levelVote = args[1].ToUpper();
                bool success = Poll_State_Manager.RegisterVote(playerName, levelVote);

                if (success)
                {
                    Plugin.Instance.mls.LogInfo($"[VOTE] {playerName} voted for {levelVote}");
                    Display_Level_Command.UpdateVoteDisplay();
                }
                else
                {
                    Plugin.Instance.mls.LogInfo($"[VOTE] Rejected: Invalid level '{levelVote}' from {playerName}");
                }
                return;
            }

            // If neither is active
            Plugin.Instance.mls.LogInfo($"[VOTE] Rejected: No active voting session for {playerName}");
        }

        private static void HandleEndPollCommand(string[] args, PlayerAvatar sender)
        {
            if (Poll_State_Manager.IsPollActive)
            {
                Display_Poll_Command.EndPoll();
                Plugin.Instance.mls.LogInfo($"[POLL] Manually ended poll");
            }
            else
            {
                Plugin.Instance.mls.LogInfo($"[POLL] No active poll to end");
            }
        }

        private static void HandleEndLevelCommand(string[] args, PlayerAvatar sender)
        {
            if (Poll_State_Manager.IsLevelDecisionActive)
            {
                // Show results before ending
                Display_Level_Command.ShowVoteResults();

                // Wait a moment before ending the level decision
                // Since we can't easily add a delay here, we'll just end it immediately
                Display_Level_Command.EndLevelDecision();
                Plugin.Instance.mls.LogInfo($"[LEVEL] Manually ended level voting");
            }
            else
            {
                Plugin.Instance.mls.LogInfo($"[LEVEL] No active level voting to end");
            }
        }

        private static void HandleResultsCommand(string[] args, PlayerAvatar sender)
        {
            if (Poll_State_Manager.IsLevelDecisionActive)
            {
                Display_Level_Command.ShowVoteResults();
                Plugin.Instance.mls.LogInfo($"[LEVEL] Showing vote results");
            }
            else
            {
                Plugin.Instance.mls.LogInfo($"[LEVEL] No active level voting to show results for");
            }
        }

        private static string GetPlayerName(PlayerAvatar player)
        {
            var playerNameTraverse = Traverse.Create(player).Field("playerName");
            if (!playerNameTraverse.FieldExists()) return "Unknown";
            return playerNameTraverse.GetValue() as string ?? "Unknown";
        }
    }
}