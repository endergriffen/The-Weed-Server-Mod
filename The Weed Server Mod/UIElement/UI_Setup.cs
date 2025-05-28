using UnityEngine.InputSystem;
using UnityEngine;

namespace The_Weed_Server_Mod.UIElement
{
    public class UI_Setup : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame && !UI_Base.toggleCooldown)
            {
                var menu = Plugin.Instance.Menu;
                if (menu != null)
                {
                    menu.isBaseWindowVisible = !menu.isBaseWindowVisible;
                    Plugin.Instance.mls.LogInfo($"Menu is now {(menu.isBaseWindowVisible ? "open" : "closed")}!");
                }

                UI_Base.toggleCooldown = true;
            }

            if (!Keyboard.current.f1Key.isPressed)
            {
                UI_Base.toggleCooldown = false;
            }
        }
    }
}