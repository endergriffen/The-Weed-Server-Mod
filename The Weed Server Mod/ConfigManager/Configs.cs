using BepInEx.Configuration;

namespace The_Weed_Server_Mod.ConfigManager
{
    public class Configs
    {
        private static Configs instance;

        public static Configs Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Configs();
                }
                return instance;
            }
        }

        public ConfigEntry<float> ExampleFloatConfig { get; private set; }
        public ConfigEntry<int> ExampleIntConfig { get; private set; }
        public ConfigEntry<string> ExampleStringConfig { get; private set; }
        public ConfigEntry<bool> ExampleBoolConfig { get; private set; }

        public ConfigEntry<string> CoHostPermissionList { get; private set; }
        public ConfigEntry<string> BannedPlayerBlacklist { get; private set; }

        public ConfigEntry<bool> NotificationMessageConfig { get; private set; }
        public ConfigEntry<float> NotificationMessageTimer { get; private set; }

        public ConfigEntry<float> TruckScreenMessageConfig { get; private set; }
        public ConfigEntry<string> TruckScreenFullMessageConfig { get; private set; }

        public ConfigEntry<float> ButtonXPosition { get; private set; }
        public ConfigEntry<float> ButtonYPosition { get; private set; }
        public ConfigEntry<bool> UseAbsolutePosition { get; private set; }

        public ConfigEntry<float> SliderXPosition { get; private set; }
        public ConfigEntry<float> SliderYPosition { get; private set; }

        public ConfigEntry<bool> TogglePermission { get; private set; }
        public ConfigEntry<float> ToggleXPosition { get; private set; }
        public ConfigEntry<float> ToggleYPosition { get; private set; }

        public void Setup(ConfigFile config)
        {
            ExampleFloatConfig = config.Bind("Float", "Example Float Config", 0f, "This is simply a example of any configs that require a float.");
            ExampleIntConfig = config.Bind("Int", "Example Int Config", 0, "This is simply a example of any configs that require a int.");
            ExampleStringConfig = config.Bind("String", "Example String Config", "apple", "This is simply a example of any configs that require a string.");
            ExampleBoolConfig = config.Bind("Bool", "Example Bool Config", false, "This is simply a example of any configs that require a bool.");

            CoHostPermissionList = config.Bind("CoHost", "CoHost Permission List", "endergriffen, weed.", "A List which you input usernames into to give them CoHost permissions.");
            BannedPlayerBlacklist = config.Bind("Player", "Banned Player Blacklist", "", "A List which you input usernames into to auto kick them from your lobbies if they join.");

            NotificationMessageConfig = config.Bind("UI", "Notification Message", true, "The togglable feature for 'Notification Message' class.");
            NotificationMessageTimer = config.Bind("UI", "Notification Message Timer", 4f, "The timer for how long the messages are on screen.");

            TruckScreenMessageConfig = config.Bind("TruckScreen", "Truck Screen Message", 1f, ".");
            TruckScreenFullMessageConfig = config.Bind("TruckScreen", "Truck Screen String Message", "e", ".");

            ButtonXPosition = config.Bind("UI Button", "Button X Position", 455f, ".");
            ButtonYPosition = config.Bind("UI Button", "Button Y Position", 340f, ".");
            UseAbsolutePosition = config.Bind("UI Button", "Use Absolute Position", true, ".");

            SliderXPosition = config.Bind("UI Slider", "Slider X Position", 455f, "X position for mod slider");
            SliderYPosition = config.Bind("UI Slider", "Slider Y Position", 240f, "Y position for mod slider");

            TogglePermission = config.Bind("UI Toggle", "Example Feature", true, "Enable/disable example feature");
            ToggleXPosition = config.Bind("UI Toggle", "Toggle X Position", 455f, "X position for mod toggle");
            ToggleYPosition = config.Bind("UI Toggle", "Toggle Y Position", 320f, "Y position for mod toggle");
        }
    }
}