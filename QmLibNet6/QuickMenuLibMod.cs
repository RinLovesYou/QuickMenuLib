using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using QuickMenuLib.UI;
using QuickMenuLib.UI.Elements;
using VRC.UI.Core;
using UnityEngine;

[assembly: MelonInfo(typeof(QuickMenuLib.QuickMenuLibMod), "QuickMenuLib", "1.6.0", "RinLovesYou")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace QuickMenuLib
{
    public class QuickMenuLibMod : MelonMod
    {
        /// <summary>
        /// Every Mod Registered in QuickMenuLib
        /// </summary>
        public static List<ModMenu> ModMenus = new List<ModMenu>();

        //We expose these to the Mods in case they want to add content to these pages. Unlikely but it can't hurt.
        public static QuickMenuPage MainMenu = null;
        public static QuickMenuPage TargetMenu = null;
        public static QuickMenuWingMenu LeftWingMenu = null;
        public static QuickMenuWingMenu RightWingMenu = null;

        public override void OnApplicationStart()
        {
            LoggerInstance.Msg("OnApplicationStart");
            OnUIManagerInitialized(delegate
            {
                PagePreparer.PrepareEverything();
            });
        }

        /// <summary>
        /// This Method registers your Mod Menu. You should call this OnApplicationStart
        /// </summary>
        /// <param name="menu">Your Mod's class, should inherit and override methods of ModMenu</param>
        public static void RegisterModMenu(ModMenu menu)
        {
            ModMenus.Add(menu);
        }

        private void OnUIManagerInitialized(Action code)
        {
            MelonCoroutines.Start(OnUiManagerInitCoroutine(code));
        }

        private IEnumerator OnUiManagerInitCoroutine(Action code)
        {
            while (VRCUiManager.prop_VRCUiManager_0 is null)
            {
                LoggerInstance.Msg("It's null");
                yield return null;
            }

            string ModValue = ModMenus.Count == 1 ? "Mod" : "Mods";
            LoggerInstance.Msg($"Found {ModMenus.Count} {ModValue} using QuickMenuLib.");

            while (UIManager.field_Private_Static_UIManager_0 is null) 

            while (GameObject.Find("UserInterface").GetComponentInChildren<VRC.UI.Elements.QuickMenu>(true) == null) 
                
            while (QuickMenuExtensions.MenuStateController is null) 
                
            //Don't create anything if nobody likes us
            if (ModMenus.Count is 0)
            {
                LoggerInstance.Msg("No Mod Menus found. Not creating pages.");
                yield break;
            }
            code();
        }
    }

    public static class Logger
    {
        private static MelonLogger.Instance MyLogger = new MelonLogger.Instance("QuickMenuLib", ConsoleColor.Magenta);

        public static void Msg(string msg) => MyLogger.Msg(msg);
        public static void Error(string msg) => MyLogger.Error(msg);
    }
}
