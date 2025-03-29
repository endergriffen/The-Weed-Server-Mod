using The_Weed_Server_Mod.ConfigManager;
using UnityEngine.InputSystem;
using UnityEngine;
using HarmonyLib;

namespace The_Weed_Server_Mod.BigMessage
{
    [HarmonyPatch(typeof(BigMessageUI))]
    [HarmonyPatch("Update")]
    public class Big_Message_Handler
    {
        [HarmonyPostfix]
        private static void UpdatePatch()
        {
            if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                if (!Configs.Instance.BigMessageConfig.Value)
                {
                    Plugin.Instance.mls.LogInfo("'Big_Message_Handler' Class Disabled!");
                    return;
                }

                Plugin.Instance.mls.LogInfo("Keybind 'g' pressed!");

                string configValue = Configs.Instance.ExampleStringConfig.Value;
                string message = $"CONFIG VALUE: {configValue}";

                if (BigMessageUI.instance != null)
                {
                    Color textColor = new Color(0.5f, 1f, 0f);
                    Color outlineColor = Color.black;

                    BigMessageUI.instance.BigMessage(message, "!", 42f, textColor, outlineColor);
                    Traverse.Create(BigMessageUI.instance).Field("bigMessageTimer").SetValue(2f);
                }
            }
        }
    }
}