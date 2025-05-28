using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace The_Weed_Server_Mod.UIElement
{
    public class UI_Base : MonoBehaviour
    {
        public bool isBaseWindowVisible = false;
        public static bool toggleCooldown = false;

        public const int MENUWIDTH = 630;
        public const int MENUHEIGHT = 885;
        public const int MENUX = 1565;
        public const int MENUY = 135;

        public static bool isBackgroundWindowVisible = false;
        public static Rect backgroundWindowRect = new Rect(300, 200, 600, 400);

        public static Rect menuRect;
        public static Texture2D customBackgroundTexture = null;

        private List<Rect> dynamicButtons = new List<Rect>();

        // Scroll variables
        private Vector2 scrollPosition = Vector2.zero;
        private float scrollWidth = 0; // Total width needed for buttons

        private static bool visibleScrollBar = false;

        private Vector2 rightClickPosition;
        private bool showRightClickMenu = false;
        private int rightClickedButtonIndex = -1;
        private Rect rightClickMenuRect = new Rect(0, 0, 120, 70);

        void OnGUI()
        {
            if (!isBaseWindowVisible) return;

            // Plugin.Instance.mls.LogInfo($"Menu has passed the return variable!");

            DrawWindowBorder(menuRect, 4, Color.yellow);

            GUIStyle solidBackground = new GUIStyle(GUI.skin.box);
            solidBackground.normal.background = MakeTex(2, 2, new Color(0.62f, 0.07f, 0f, 0.3f));

            GUI.Box(menuRect, GUIContent.none, solidBackground); // Draw the opaque background

            GUIStyle transparentWindow = new GUIStyle();
            transparentWindow.normal.background = null;

            // Main window
            menuRect = GUI.Window(0, menuRect, DrawMenuWindow, "", transparentWindow);

            // Right-click menu logic
            if (showRightClickMenu)
            {
                rightClickMenuRect = GUI.Window(1, rightClickMenuRect, DrawRightClickMenu, "");
            }

            // Detect clicks outside all UI elements
            if (Event.current.type == EventType.MouseDown)
            {
                bool clickedOutsideRightClickMenu = !rightClickMenuRect.Contains(Event.current.mousePosition);

                if (clickedOutsideRightClickMenu)
                {
                    showRightClickMenu = false; // Close the right-click menu
                }
            }
        }

        void DrawMenuWindow(int windowID)
        {
            Vector2 mousePosition = Event.current.mousePosition;
            GUIStyle titleBarStyle = new GUIStyle();
            titleBarStyle.normal.background = MakeTex(2, 2, new Color(0.25f, 0.1f, 0f, 0.65f));

            if (!visibleScrollBar)
            {
                GUI.Box(new Rect(5, 5, menuRect.width - 10, 60), GUIContent.none, titleBarStyle);
            }
            else
            {
                GUI.Box(new Rect(5, 5, menuRect.width - 10, 80), GUIContent.none, titleBarStyle);
            }

            // Plugin.Instance.mls.LogInfo($"Menu has passed the 'DrawMenuWindow' variable!");

            float buttonWidth = 60;
            float buttonHeight = 45;
            float spacing = 10;
            float startX = 15;

            GUI.backgroundColor = new Color(1f, 0.55f, 0f);

            scrollWidth = (dynamicButtons.Count + 1) * (buttonWidth + spacing);
            float scrollViewWidth = visibleScrollBar ? menuRect.width - 30 : menuRect.width;

            scrollPosition = GUI.BeginScrollView(
                new Rect(startX, 11, scrollViewWidth, buttonHeight + 20),
                scrollPosition,
                new Rect(0, 0, scrollWidth, buttonHeight),
                visibleScrollBar,
                false
            );

            for (int i = 0; i < dynamicButtons.Count; i++)
            {
                dynamicButtons[i] = new Rect(i * (buttonWidth + spacing), 0, buttonWidth, buttonHeight);

                if (GUI.Button(dynamicButtons[i], $"Button {i + 1}"))
                {
                    if (Event.current.button == 0) // Left-click
                    {
                        showRightClickMenu = false;
                        Plugin.Instance.mls.LogInfo($"Button {i + 1} clicked!");
                    }
                    else if (Event.current.button == 1) // Right-click
                    {
                        // Close and reopen the right-click menu
                        StartCoroutine(ReopenRightClickMenuWithDelay());

                        rightClickPosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                        rightClickMenuRect.position = rightClickPosition; // Move the menu
                        rightClickedButtonIndex = i;
                        Event.current.Use();
                    }
                }
            }

            // "+" button to add more buttons
            float plusButtonX = dynamicButtons.Count * (buttonWidth + spacing);
            if (GUI.Button(new Rect(plusButtonX, 0, buttonWidth, buttonHeight), "+"))
            {
                if (Event.current.button == 0)
                {
                    showRightClickMenu = false;
                    AddDynamicButton();
                }
                if (Event.current.button == 1)
                {
                    showRightClickMenu = false;
                }
            }

            GUI.EndScrollView();

            if (showRightClickMenu)
            {
                GUI.DragWindow(new Rect(0, 0, 0, 0));
            }
            else
            {
                GUI.DragWindow(new Rect(0, 0, MENUWIDTH, 80));
            }
        }

        void DrawWindowBorder(Rect rect, int thickness, Color color)
        {
            Color originalColor = GUI.color;
            GUI.color = new Color(1f, 0.6f, 0f);

            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, thickness), Texture2D.whiteTexture); // Top
            GUI.DrawTexture(new Rect(rect.x, rect.yMax - thickness, rect.width, thickness), Texture2D.whiteTexture); // Bottom
            GUI.DrawTexture(new Rect(rect.x, rect.y, thickness, rect.height), Texture2D.whiteTexture); // Left
            GUI.DrawTexture(new Rect(rect.xMax - thickness, rect.y, thickness, rect.height), Texture2D.whiteTexture); // Right

            GUI.color = originalColor;
        }

        private Texture2D MakeTex(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        void AddDynamicButton()
        {
            dynamicButtons.Add(new Rect(0, 0, 0, 0));

            if (dynamicButtons.Count >= 8)
            {
                visibleScrollBar = true;
            }
            else
            {
                visibleScrollBar = false;
            }
        }

        void DrawRightClickMenu(int windowID)
        {
            GUI.Box(new Rect(0, 0, rightClickMenuRect.width, rightClickMenuRect.height), "Options");

            if (GUI.Button(new Rect(10, 20, 100, 20), "Edit Button"))
            {
                Plugin.Instance.mls.LogInfo("Edit Button Pressed!");
                showRightClickMenu = false;
            }

            if (GUI.Button(new Rect(10, 45, 100, 20), "Remove Button"))
            {
                RemoveButton(rightClickedButtonIndex);
                showRightClickMenu = false;
            }


            GUI.DragWindow(new Rect(0, 0, rightClickMenuRect.width, 20));
        }

        private IEnumerator ReopenRightClickMenuWithDelay()
        {
            showRightClickMenu = false; // Close the menu first
            Plugin.Instance.mls.LogInfo("Timer Started!");
            yield return new WaitForSeconds(0.1f);  // Wait for 0.5 seconds
            Plugin.Instance.mls.LogInfo("Timer Ended!");
            showRightClickMenu = true;  // Reopen the menu
        }

        void RemoveButton(int index)
        {
            if (index >= 0 && index < dynamicButtons.Count)
            {
                dynamicButtons.RemoveAt(index);
            }

            showRightClickMenu = false;
            rightClickedButtonIndex = -1;

            // Update scrollbar visibility
            visibleScrollBar = dynamicButtons.Count >= 8;
        }
    }
}