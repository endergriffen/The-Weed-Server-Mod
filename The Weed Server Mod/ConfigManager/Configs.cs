using BepInEx.Configuration;
using System;

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

        public void Setup(ConfigFile config)
        {
            ExampleFloatConfig = config.Bind("Float", "Example Float Config", 0f, "This is simply a example of any configs that require a float.");
            ExampleIntConfig = config.Bind("Int", "Example Int Config", 0, "This is simply a example of any configs that require a int.");
            ExampleStringConfig = config.Bind("String", "Example String Config", "apple", "This is simply a example of any configs that require a string.");
            ExampleBoolConfig = config.Bind("Bool", "Example Bool Config", false, "This is simply a example of any configs that require a bool.");
            // Smol Change
        }
    }
}