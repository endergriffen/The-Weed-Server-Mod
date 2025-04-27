using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using The_Weed_Server_Mod.ConfigManager;

namespace The_Weed_Server_Mod.UIElement
{
    [HarmonyPatch(typeof(MenuPageEsc), "Start")]
    public class New_Test_Button
    {
        public static float ButtonXPosition = Configs.Instance.ButtonXPosition.Value;
        public static float ButtonYPosition = Configs.Instance.ButtonYPosition.Value;
        public static bool UseAbsolutePosition = Configs.Instance.UseAbsolutePosition.Value;

        [HarmonyPostfix]
        static void Postfix(MenuPageEsc __instance)
        {
            __instance.StartCoroutine(AddCustomButtonDelayed(__instance));
        }

        static IEnumerator AddCustomButtonDelayed(MenuPageEsc menuPageEsc)
        {
            yield return null;

            Plugin.Instance.mls.LogInfo("Adding custom button to MenuPageEsc");

            Transform buttonTransform = null;
            foreach (Transform child in menuPageEsc.transform)
            {
                MenuButton buttonComponent = child.GetComponent<MenuButton>();
                if (buttonComponent != null)
                {
                    buttonTransform = child;
                    Plugin.Instance.mls.LogInfo($"Found button to clone: {child.name} at position {child.localPosition}");
                    break;
                }
            }

            if (buttonTransform == null)
            {
                Plugin.Instance.mls.LogError("Could not find a button to clone!");
                yield break;
            }

            GameObject newButtonObj = Object.Instantiate(buttonTransform.gameObject, menuPageEsc.transform);
            newButtonObj.name = "CustomModButton";

            RectTransform rectTransform = newButtonObj.GetComponent<RectTransform>();

            if (UseAbsolutePosition)
            {
                rectTransform.localPosition = new Vector3(ButtonXPosition, ButtonYPosition, 0f);
                Plugin.Instance.mls.LogInfo($"Positioned button at absolute position: {rectTransform.localPosition}");
            }
            else
            {
                float lowestY = 0;
                GameObject lowestButton = null;

                foreach (Transform child in menuPageEsc.transform)
                {
                    MenuButton btn = child.GetComponent<MenuButton>();
                    if (btn != null && child != newButtonObj.transform)
                    {
                        if (child.localPosition.y < lowestY)
                        {
                            lowestY = child.localPosition.y;
                            lowestButton = child.gameObject;
                        }
                    }
                }

                if (lowestButton != null)
                {
                    Plugin.Instance.mls.LogInfo($"Lowest button found: {lowestButton.name} at Y={lowestY}");
                    Vector3 position = rectTransform.localPosition;
                    position.y = lowestY - 50f;
                    rectTransform.localPosition = position;
                    Plugin.Instance.mls.LogInfo($"Positioned button relative to lowest button: {rectTransform.localPosition}");
                }
                else
                {
                    rectTransform.localPosition = new Vector3(ButtonXPosition, ButtonYPosition, 0f);
                    Plugin.Instance.mls.LogInfo($"No other buttons found, using absolute position: {rectTransform.localPosition}");
                }
            }

            MenuButtonColor colorComponent = newButtonObj.GetComponent<MenuButtonColor>();
            if (colorComponent != null)
            {
                Object.Destroy(colorComponent);
                Plugin.Instance.mls.LogInfo("Removed MenuButtonColor component from cloned button");
            }

            MenuButtonPopUp popupComponent = newButtonObj.GetComponent<MenuButtonPopUp>();
            if (popupComponent != null)
            {
                Object.Destroy(popupComponent);
                Plugin.Instance.mls.LogInfo("Removed MenuButtonPopUp component from cloned button");
            }

            MenuButton customButton = newButtonObj.GetComponent<MenuButton>();
            if (customButton != null)
            {
                customButton.buttonTextString = "MOD OPTIONS";

                TextMeshProUGUI buttonText = newButtonObj.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = "MOD OPTIONS";
                    Plugin.Instance.mls.LogInfo("Updated button text directly");
                }

                customButton.customColors = true;
                customButton.colorNormal = new Color(0.85f, 0.85f, 0.85f);

                Button button = newButtonObj.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick = new Button.ButtonClickedEvent();
                    button.onClick.AddListener(() => OnCustomButtonClick(menuPageEsc));
                    Plugin.Instance.mls.LogInfo("Replaced button click event");
                }
            }

            MenuPage menuPage = menuPageEsc.GetComponent<MenuPage>();

            MenuSelectableElement selectableElement = newButtonObj.GetComponent<MenuSelectableElement>();
            if (selectableElement != null && menuPage != null)
            {
                FieldInfo selectableElementsField = typeof(MenuPage).GetField("selectableElements", BindingFlags.Instance | BindingFlags.NonPublic);

                if (selectableElementsField != null)
                {
                    var selectableElements = selectableElementsField.GetValue(menuPage) as List<MenuSelectableElement>;
                    if (selectableElements != null && !selectableElements.Contains(selectableElement))
                    {
                        selectableElements.Add(selectableElement);
                        Plugin.Instance.mls.LogInfo("Added button to selectable elements via reflection");
                    }
                }
                else
                {
                    Plugin.Instance.mls.LogError("Could not find selectableElements field via reflection");
                }
            }

            newButtonObj.SetActive(true);

            Plugin.Instance.mls.LogInfo("Custom button added successfully!");
        }

        static void OnCustomButtonClick(MenuPageEsc menuPageEsc)
        {
            Plugin.Instance.mls.LogInfo("Custom button clicked!");

            MenuManager.instance.PagePopUp("Mod Options", Color.cyan, "This button was added by a BepInEx mod!\n\nYou can add your mod configuration options here.", "OK");
        }
    }
}