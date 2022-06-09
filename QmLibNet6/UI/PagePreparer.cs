using System;
using QuickMenuLib.UI.Elements;
using UnityEngine.UI;
using VRC.UI.Elements;
using static QuickMenuLib.Logger;

namespace QuickMenuLib.UI
{
    internal class PagePreparer
    {
        internal static void PrepareEverything()
        {
            InitializeQuickMenu();
            InitializeTargetMenu();
            InitializeWingMenus();
        }

        private static void InitializeQuickMenu()
        {
            try
            {
                Msg("Setting Up Launchpad.");
                
                FixLaunchpadScrolling();
                
                var MainCategory = new QuickMenuCategory("QMLib", null, null, false);

                QuickMenuLibMod.MainMenu = new QuickMenuPage("QuickMenuLib", false, true);

                MainCategory.AddButton("Mods", "Mod Menus using QuickMenuLib", QuickMenuLibMod.MainMenu.Open);

                foreach (var menu in QuickMenuLibMod.ModMenus)
                {
                    menu.MyModMenu = QuickMenuLibMod.MainMenu.AddSubMenu(menu.MenuName, $"Content for {menu.MenuName}", false, menu.Logo);
                    try
                    {
                        menu.OnQuickMenuInitialized();
                    }
                    catch (Exception e)
                    {
                        Error($"Failed to Initialize QuickMenu for {menu.MenuName}!");
                        Error(e.Message + e.StackTrace);
                    }
                }
            } catch(Exception e)
            {
                Error($"Failed to initialize Launchpad! Exception: {e}");
            }
        }

        private static void InitializeTargetMenu()
        {
            try
            {
                Msg("Setting Up Target Menu.");

                var MainCategory = new QuickMenuCategory("QMLib-Targets", QuickMenuExtensions.SelectedUserMenu.transform.Find("ScrollRect").GetComponent<ScrollRect>().content, null, false);

                QuickMenuLibMod.TargetMenu = new QuickMenuPage("QuickMenuLib-Targets", false, true);

                MainCategory.AddButton("Mods", "Mod Menus using QuickMenuLib", QuickMenuLibMod.TargetMenu.Open);

                foreach (var menu in QuickMenuLibMod.ModMenus)
                {
                    menu.MyTargetMenu = QuickMenuLibMod.TargetMenu.AddSubMenu($"{menu.MenuName}-Targets", $"Content for {menu.MenuName}", false, menu.Logo, false);
                    QuickMenuLibMod.TargetMenu.AddButton(menu.MenuName, $"Functions for {menu.MenuName}", menu.MyTargetMenu.Open, menu.Logo);
                    try
                    {
                        menu.OnTargetMenuInitialized();
                    }
                    catch (Exception e)
                    {
                        Error($"Failed to Initialize TargetMenu for {menu.MenuName}!");
                        Error(e.Message + e.StackTrace);
                    }
                }
            }
            catch (Exception e)
            {
                Error($"Failed to initialize Target Menu! Exception: {e}");
            }
        }

        private static void InitializeWingMenus()
        {
            try
            {
                QuickMenuLibMod.LeftWingMenu = new QuickMenuWingMenu("QMLib_Left");
                new QuickMenuWingButton("Mods", "Mod Menus using QuickMenuLib", QuickMenuLibMod.LeftWingMenu.Open);

                QuickMenuLibMod.RightWingMenu = new QuickMenuWingMenu("QMLib_Right", false);
                new QuickMenuWingButton("Mods", "Mod Menus using QuickMenuLib", QuickMenuLibMod.RightWingMenu.Open, null,
                    false);

                foreach (var menu in QuickMenuLibMod.ModMenus)
                {
                    menu.MyLeftWingMenu = QuickMenuLibMod.LeftWingMenu.AddSubMenu($"{menu.MenuName}_Left",
                        $"Functions for {menu.MenuName}", menu.Logo, false);
                    menu.MyRightWingMenu = QuickMenuLibMod.RightWingMenu.AddSubMenu($"{menu.MenuName}_Right",
                        $"Functions for {menu.MenuName}", menu.Logo, false);
                    
                    QuickMenuLibMod.LeftWingMenu.AddButton(menu.MenuName, "Functions for " + menu.MenuName,
                        menu.MyLeftWingMenu.Open, menu.Logo);
                    QuickMenuLibMod.RightWingMenu.AddButton(menu.MenuName, "Functions for " + menu.MenuName,
                        menu.MyRightWingMenu.Open, menu.Logo);
                    try
                    {
                        menu.OnWingMenuLeftInitialized();
                        menu.OnWingMenuRightInitialized();
                    }
                    catch (Exception e)
                    {
                        Error($"Failed to Initialize Wing Menus for {menu.MenuName}!");
                        Error(e.Message + e.StackTrace);
                    }
                }
            }
            catch (Exception e)
            {
                Error($"Failed to initialize Wing Menus! Exception: {e}");
            }
        }
        
        private static void FixLaunchpadScrolling()
        {
            var dashboard = QuickMenuExtensions.GetQuickMenu.field_Public_Transform_0.Find("Window/QMParent/Menu_Dashboard").GetComponent<UIPage>();
            var scrollRect = dashboard.GetComponentInChildren<ScrollRect>();
            var dashboardScrollbar = scrollRect.transform.Find("Scrollbar").GetComponent<Scrollbar>();

            var dashboardContent = scrollRect.content;
            dashboardContent.GetComponent<VerticalLayoutGroup>().childControlHeight = true;

            scrollRect.enabled = true;
            scrollRect.verticalScrollbar = dashboardScrollbar;
            scrollRect.viewport.GetComponent<RectMask2D>().enabled = true;
        }
    }
}
