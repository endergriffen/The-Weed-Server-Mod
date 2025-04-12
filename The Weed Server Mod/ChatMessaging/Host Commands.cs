using System.Linq;
using The_Weed_Server_Mod.TruckScreen;

namespace The_Weed_Server_Mod.ChatMessaging
{
    public class Host_Commands
    {
        public static readonly string[] HostCommandList = { "/display", "/poll", "/level" };

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
                string question = string.Join(" ", args.Skip(1));
                Display_Poll_Command.ShowPollMessage(question);
                Plugin.Instance.mls.LogInfo($"[POLL] Starting poll: {question}");
            }
        }

        private static void HandleLevelCommand(string[] args, PlayerAvatar sender)
        {
            string decideLevel = string.Join(" ", args.Skip(1));
            Display_Level_Command.ShowLevelMessage();
            Plugin.Instance.mls.LogInfo($"[LEVEL] Starting voting:");
        }
    }
}