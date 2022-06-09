
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Object = UnityEngine.Object;

namespace QuickMenuLib.UI.Elements
{
    /// <summary>
    /// Credit to https://github.com/RequiDev/RemodCE
    /// </summary>
    public class QuickMenuHeader : UIElement
    {
        private static GameObject HeaderTemplate() => QuickMenuTemplates.GetHeaderTemplate();

        public QuickMenuHeader(string title, Transform parent) : base(HeaderTemplate(), (parent == null ? HeaderTemplate().transform.parent : parent), $"Header_{title}")
        {
            var tmp = GameObject.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = title;
            tmp.richText = true;

            tmp.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;

        }
    }
    public class QuickMenuButtonContainer : UIElement
    {
        private static GameObject ButtonRowTemplate => QuickMenuTemplates.GetButtonRowTemplate();

        public QuickMenuButtonContainer(string name, Transform parent = null, bool alignLeft = true) : base(ButtonRowTemplate, (parent == null ? ButtonRowTemplate.transform.parent : parent), $"Buttons_{name}")
        {
            foreach (var button in RectTransform)
            {
                var control = button.Cast<Transform>();
                if (control == null)
                {
                    continue;
                }
                Object.Destroy(control.gameObject);
            }

            var gridLayout = GameObject.GetComponent<GridLayoutGroup>();
            if (alignLeft)
                gridLayout.childAlignment = TextAnchor.UpperLeft;

            gridLayout.padding.top = 8;
            gridLayout.padding.left = 64;

            if (!alignLeft)
            {
                gridLayout.padding.left = 56;
            }
        }
    }

    public class QuickMenuCategory
    {
        public QuickMenuHeader MyHeader;
        public QuickMenuButtonContainer MyButtonContainer;

        public readonly List<QuickMenuPage> SubPages = new List<QuickMenuPage>();

        public QuickMenuCategory(string title, Transform parent = null, int? siblingIndex = null, bool alignLeft = true)
        {
            MyHeader = new QuickMenuHeader(title, parent);
            MyButtonContainer = new QuickMenuButtonContainer(title, parent, alignLeft);
            
            if (siblingIndex != null)
            {
                MyHeader.RectTransform.SetSiblingIndex(siblingIndex.Value);
                MyButtonContainer.RectTransform.SetSiblingIndex(siblingIndex.Value + 1);
            }
        }

        public QuickMenuButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null)
        {
            var button = new QuickMenuButton(text, tooltip, onClick, MyButtonContainer.RectTransform, sprite);
            return button;
        }

        public QuickMenuToggleButton AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultPosition = false)
        {
            var toggle = new QuickMenuToggleButton(text, tooltip, onToggle, MyButtonContainer.RectTransform, defaultPosition);
            return toggle;
        }

        public QuickMenuPage AddMenuPage(string text, string tooltip = "", Sprite sprite = null, bool grid = false, bool button = true)
        {
            var menu = new QuickMenuPage(text, false, grid);
            if (button)
            {
                AddButton(text, string.IsNullOrEmpty(tooltip) ? $"Open the {text} menu" : tooltip, menu.Open, sprite);
            }
            SubPages.Add(menu);
            return menu;
        }
    }
}
