using System.Collections.Generic;

namespace The_Weed_Server_Mod.PlayerFolder
{
    public class PlayerData
    {
        public string Name { get; set; }
        public bool IsCoHost { get; set; }
        public int DeathCount { get; set; }
        public int ReviveCount { get; set; }
        public bool IsConnected { get; set; }
        public int Ping { get; set; }
        public bool Stunned { get; set; }
        public List<string> Inventory { get; set; }
        public Dictionary<string, int> Upgrades { get; set; }

        public PlayerData(string name)
        {
            Name = name;
            IsCoHost = false;
            DeathCount = 0;
            ReviveCount = 0;
            IsConnected = true;
            Ping = 0;
            Stunned = false;
            Inventory = new List<string>();
            Upgrades = new Dictionary<string, int>();
        }
    }

    public class Player_Statistics
    {
        private static List<PlayerData> players = new List<PlayerData>();

        public static List<string> playerList
        {
            get { return players.ConvertAll(player => player.Name); }
        }

        public static List<string> GetPlayerList()
        {
            return new List<string>(playerList);
        }

        public static void AddPlayer(string playerName)
        {
            if (!playerList.Contains(playerName))
            {
                players.Add(new PlayerData(playerName));
            }
        }

        public static PlayerData GetPlayer(string playerName)
        {
            return players.Find(player => player.Name == playerName);
        }

        public static void IncrementDeathCount(string playerName)
        {
            var player = GetPlayer(playerName);
            if (player != null)
            {
                player.DeathCount++;
            }
        }

        public static void IncrementReviveCount(string playerName)
        {
            var player = GetPlayer(playerName);
            if (player != null)
            {
                player.ReviveCount++;
            }
        }

        public static void SetCoHost(string playerName, bool isCoHost)
        {
            var player = GetPlayer(playerName);
            if (player != null)
            {
                player.IsCoHost = isCoHost;
            }
        }

        public static void SetConnectionStatus(string playerName, bool isConnected)
        {
            var player = GetPlayer(playerName);
            if (player != null)
            {
                player.IsConnected = isConnected;
            }
        }

        public static void UpdatePing(string playerName, int ping)
        {
            var player = GetPlayer(playerName);
            if (player != null)
            {
                player.Ping = ping;
            }
        }

        public static void SetStunned(string playerName, bool stunned)
        {
            var player = GetPlayer(playerName);
            if (player != null)
            {
                player.Stunned = stunned;
            }
        }

        public static void AddToInventory(string playerName, string item)
        {
            var player = GetPlayer(playerName);
            if (player != null)
            {
                player.Inventory.Add(item);
            }
        }

        public static void RemoveFromInventory(string playerName, string item)
        {
            var player = GetPlayer(playerName);
            if (player != null)
            {
                player.Inventory.Remove(item);
            }
        }

        public static void AddOrUpdateUpgrade(string playerName, string upgradeName, int level)
        {
            var player = GetPlayer(playerName);
            if (player != null)
            {
                player.Upgrades[upgradeName] = level;
            }
        }
    }
}