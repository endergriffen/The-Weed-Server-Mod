using System;
using System.Collections.Generic;
using System.Linq;

namespace The_Weed_Server_Mod.TruckScreen
{
    public static class Poll_State_Manager
    {
        public static bool IsPollActive { get; private set; } = false;
        public static bool IsLevelDecisionActive { get; private set; } = false;

        public static DateTime? PollStartTime { get; private set; } = null;
        public static DateTime? LevelDecisionStartTime { get; private set; } = null;

        public static string CurrentPollQuestion { get; private set; }
        public static List<string> PollOptions { get; private set; } = new List<string>();
        public static Dictionary<string, int> PollVotes { get; private set; } = new Dictionary<string, int>();
        private static Dictionary<string, string> PollPlayerVotes { get; set; } = new Dictionary<string, string>();

        // Track available levels for voting
        public static List<LevelInfo> AvailableLevels { get; private set; } = new List<LevelInfo>();

        // Track votes for each level
        private static Dictionary<string, int> LevelVotes = new Dictionary<string, int>();

        // Track which players have voted
        private static Dictionary<string, string> PlayerVotes = new Dictionary<string, string>();

        public static void StartPoll(string question, List<string> options)
        {
            // End any active level decision
            if (IsLevelDecisionActive)
            {
                EndLevelDecision();
                Plugin.Instance.mls.LogInfo("Ended level decision due to new poll starting");
            }

            IsPollActive = true;
            PollStartTime = DateTime.Now;
            CurrentPollQuestion = question;
            PollOptions = options;
            PollVotes.Clear();
            PollPlayerVotes.Clear();
            Plugin.Instance.mls.LogInfo("Poll started");

            foreach (var option in options)
            {
                string key = option.ToUpper();
                PollVotes[key] = 0;
            }
        }

        public static void EndPoll()
        {
            IsPollActive = false;
            PollStartTime = null;
            CurrentPollQuestion = null;
            PollOptions.Clear();
            PollVotes.Clear();
            PollPlayerVotes.Clear();
            Plugin.Instance.mls.LogInfo("Poll ended");
        }

        public static bool RegisterPollVote(string playerName, string option)
        {
            if (!IsPollActive) return false;

            option = option.ToUpper();
            if (!PollVotes.ContainsKey(option)) return false;

            // Remove previous vote
            if (PollPlayerVotes.TryGetValue(playerName, out string prevVote))
            {
                PollVotes[prevVote]--;
            }

            PollVotes[option]++;
            PollPlayerVotes[playerName] = option;
            return true;
        }

        public static void StartLevelDecision(List<LevelInfo> levels)
        {
            // End any active poll
            if (IsPollActive)
            {
                EndPoll();
                Plugin.Instance.mls.LogInfo("Ended poll due to new level decision starting");
            }

            IsLevelDecisionActive = true;
            LevelDecisionStartTime = DateTime.Now;

            // Store available levels and reset votes
            AvailableLevels = levels;
            LevelVotes.Clear();
            PlayerVotes.Clear();

            // Initialize vote counts for each level
            foreach (var level in levels)
            {
                LevelVotes[level.Name.ToUpper()] = 0;
            }

            Plugin.Instance.mls.LogInfo("Level decision started");
        }

        public static void EndLevelDecision()
        {
            IsLevelDecisionActive = false;
            LevelDecisionStartTime = null;
            AvailableLevels.Clear();
            LevelVotes.Clear();
            PlayerVotes.Clear();
            Plugin.Instance.mls.LogInfo("Level decision ended");
        }

        public static bool RegisterVote(string playerName, string levelName)
        {
            if (!IsLevelDecisionActive)
            {
                return false;
            }

            // Convert to uppercase for case-insensitive comparison
            levelName = levelName.ToUpper();

            // Check if the level is valid
            if (!LevelVotes.ContainsKey(levelName))
            {
                return false;
            }

            // If player already voted, remove their previous vote
            if (PlayerVotes.TryGetValue(playerName, out string previousVote))
            {
                if (LevelVotes.ContainsKey(previousVote))
                {
                    LevelVotes[previousVote]--;
                }
            }

            // Register the new vote
            LevelVotes[levelName]++;
            PlayerVotes[playerName] = levelName;

            return true;
        }

        public static Dictionary<string, int> GetVoteResults()
        {
            return new Dictionary<string, int>(LevelVotes);
        }

        public static string GetWinningLevel()
        {
            if (LevelVotes.Count == 0)
            {
                return null;
            }

            return LevelVotes.OrderByDescending(kv => kv.Value).First().Key;
        }

        public static int GetTotalVotes()
        {
            return LevelVotes.Values.Sum();
        }
    }
}