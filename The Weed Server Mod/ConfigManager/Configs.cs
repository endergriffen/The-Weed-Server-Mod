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

        public ConfigEntry<bool> NotificationMessageConfig { get; private set; }
        public ConfigEntry<float> NotificationMessageTimer { get; private set; }

        public ConfigEntry<float> TruckScreenMessageConfig { get; private set; }
        public ConfigEntry<string> TruckScreenFullMessageConfig { get; private set; }

        public ConfigEntry<float> ButtonXPosition { get; private set; }
        public ConfigEntry<float> ButtonYPosition { get; private set; }
        public ConfigEntry<bool> UseAbsolutePosition { get; private set; }

        public void Setup(ConfigFile config)
        {
            ExampleFloatConfig = config.Bind("Float", "Example Float Config", 0f, "This is simply a example of any configs that require a float.");
            ExampleIntConfig = config.Bind("Int", "Example Int Config", 0, "This is simply a example of any configs that require a int.");
            ExampleStringConfig = config.Bind("String", "Example String Config", "apple", "This is simply a example of any configs that require a string.");
            ExampleBoolConfig = config.Bind("Bool", "Example Bool Config", false, "This is simply a example of any configs that require a bool.");

            NotificationMessageConfig = config.Bind("UI", "Notification Message", true, "The togglable feature for 'Notification Message' class.");
            NotificationMessageTimer = config.Bind("UI", "Notification Message Timer", 4f, "The timer for how long the messages are on screen.");

            TruckScreenMessageConfig = config.Bind("TruckScreen", "Truck Screen Message", 1f, ".");
            TruckScreenFullMessageConfig = config.Bind("TruckScreen", "Truck Screen String Message", "e", ".");

            ButtonXPosition = config.Bind("UI Button", "Button X Position", 455f, ".");
            ButtonYPosition = config.Bind("UI Button", "Button Y Position", 340f, ".");
            UseAbsolutePosition = config.Bind("UI Button", "Use Absolute Position", true, ".");
        }
    }
}