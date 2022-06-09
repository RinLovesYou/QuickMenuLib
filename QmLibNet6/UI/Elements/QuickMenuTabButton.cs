using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;

namespace QuickMenuLib.UI.Elements
{
    /// <summary>
    /// Credit to https://github.com/RequiDev/RemodCE
    /// </summary>
    public class QuickMenuTabButton : UIElement
    {
        private static GameObject TabButtonTemplate => QuickMenuTemplates.GetPageButtonTemplate();

        public QuickMenuTabButton(string name, string tooltip, string pageName, Sprite sprite) : base(TabButtonTemplate, TabButtonTemplate.transform.parent, $"Page_{name}")
        {
            var menuTab = RectTransform.GetComponent<MenuTab>();
            menuTab.field_Public_String_0 = $"QuickMenu{pageName}";
            menuTab.field_Private_MenuStateController_0 = QuickMenuExtensions.MenuStateController;

            var button = GameObject.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(new Action(menuTab.ShowTabContent));

            var uiTooltip = GameObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
            uiTooltip.field_Public_String_0 = tooltip;
            uiTooltip.field_Public_String_1 = tooltip;

            var iconImage = RectTransform.Find("Icon").GetComponent<Image>();
            iconImage.sprite = sprite;
            iconImage.overrideSprite = sprite;
        }
    }
}
