/*
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace The_Weed_Server_Mod.UIElement
{
    [HarmonyPatch(typeof(MenuPageEsc), "Start")]
    public class MenuPageEscPatch
    {
        [HarmonyPostfix]
        static void Postfix(MenuPageEsc __instance)
        {
            // Add custom buttons after the original Start method runs
            AddCustomButtons(__instance);
        }

        static void AddCustomButtons(MenuPageEsc menuPageEsc)
        {
            // Find an existing button to clone
            MenuButton templateButton = menuPageEsc.GetComponentInChildren<MenuButton>();
            if (templateButton == null) return;

            // Create a new button GameObject
            GameObject newButtonObj = Object.Instantiate(templateButton.gameObject, templateButton.transform.parent);

            // Position it appropriately (you'll need to adjust this based on the existing layout)
            RectTransform rectTransform = newButtonObj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition += new Vector2(0, -50); // Move it down from the template

            // Configure the button
            MenuButton newButton = newButtonObj.GetComponent<MenuButton>();
            newButton.buttonTextString = "MY CUSTOM BUTTON";

            // Get the TextMeshProUGUI component and update it
            TextMeshProUGUI buttonText = newButtonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = "MY CUSTOM BUTTON";
            }

            // Add a click handler
            Button unityButton = newButtonObj.GetComponent<Button>();
            unityButton.onClick.RemoveAllListeners(); // Clear existing listeners
            unityButton.onClick.AddListener(() => OnCustomButtonClick(menuPageEsc));

            // Make sure it has the necessary components
            MenuSelectableElement selectableElement = newButtonObj.GetComponent<MenuSelectableElement>();
            if (selectableElement != null)
            {
                // Ensure it has a unique menuID
                selectableElement.menuID = GetUniqueMenuID(menuPageEsc);
            }
        }

        static void OnCustomButtonClick(MenuPageEsc menuPageEsc)
        {
            Debug.Log("Custom button clicked!");
            // Implement your custom functionality here

            // Example: Open a custom popup
            MenuManager.instance.PagePopUp(
                "Custom Action",
                Color.green,
                "This is a custom action from the mod!",
                "OK"
            );
        }

        static int GetUniqueMenuID(MenuPageEsc menuPageEsc)
        {
            // Find the highest menuID and add 1
            int highestID = 0;
            foreach (MenuSelectableElement element in menuPageEsc.GetComponentsInChildren<MenuSelectableElement>())
            {
                if (element.menuID > highestID)
                    highestID = element.menuID;
            }
            return highestID + 1;
        }
    }
}
*/