using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using Object = UnityEngine.Object;

namespace QuickMenuLib.UI.Elements
{
    /// <summary>
    /// Credit to https://github.com/RequiDev/RemodCE
    /// </summary>
    public class QuickMenuWingMenu : UIElement
    {
        private static GameObject WingMenuTemplate => QuickMenuTemplates.GetWingMenuTemplate();

        private readonly Wing MyWing;
        private readonly string MyName;
        private readonly Transform MyContainer;

        public QuickMenuWingMenu(string text, bool left = true) : base(WingMenuTemplate, (left ? QuickMenuExtensions.LeftWing : QuickMenuExtensions.RightWing).field_Public_RectTransform_0, "WingMenu_{text}", false)
        {
            MyName = $"WingMenu_{text}";
            MyWing = left ? QuickMenuExtensions.LeftWing : QuickMenuExtensions.RightWing;

            var headerTransform = RectTransform.GetChild(0);
            var titleText = headerTransform.GetComponentInChildren<TextMeshProUGUI>();
            titleText.text = text;
            titleText.richText = true;

            var backButton = headerTransform.GetComponentInChildren<Button>(true);
            backButton.gameObject.SetActive(true);

            var backIcon = backButton.transform.Find("Icon");
            backIcon.gameObject.SetActive(true);
            var components = new Il2CppSystem.Collections.Generic.List<Behaviour>();
            backButton.GetComponents(components);

            foreach (var comp in components)
            {
                comp.enabled = true;
            }

            var content = RectTransform.GetComponentInChildren<ScrollRect>().content;
            foreach (var obj in content)
            {
                var control = obj.Cast<Transform>();
                if (control == null)
                {
                    continue;
                }

                Object.Destroy(control.gameObject);
            }

            MyContainer = content;

            var uiPage = GameObject.GetComponent<UIPage>();
            uiPage.name = $"WingMenu_{text}";
            uiPage.field_Public_String_0 = $"WingMenu_{text}";
            uiPage.field_Private_Boolean_0 = false;
            uiPage.field_Public_Boolean_1 = false;
            uiPage.field_Protected_MenuStateController_0 = MyWing.GetComponent<MenuStateController>();
            uiPage.field_Private_List_1_UIPage_0 = new Il2CppSystem.Collections.Generic.List<UIPage>();
            uiPage.field_Private_List_1_UIPage_0.Add(uiPage);

            MyWing.field_Private_MenuStateController_0.field_Private_Dictionary_2_String_UIPage_0.Add(uiPage.field_Public_String_0, uiPage);
        }

        public void Open()
        {
            MyWing.field_Private_MenuStateController_0.Method_Public_Void_String_UIContext_Boolean_0(MyName);
        }

        public QuickMenuWingButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
        {
            return new QuickMenuWingButton(text, tooltip, onClick, MyContainer, sprite, arrow, background, separator);
        }

        public QuickMenuWingToggleButton AddToggleButton(string onText, Action onAction, string offText, Action offAction, string tooltip, bool defaultState = false, Sprite img = null)
        {
            return new QuickMenuWingToggleButton(onText, onAction, offText, offAction, MyContainer, tooltip, defaultState, img);
        }
        
        public QuickMenuWingMenu AddSubMenu(string text, string tooltip, Sprite image = null, bool button = true)
        {
            var menu = new QuickMenuWingMenu(text, MyWing.field_Public_WingPanel_0 == Wing.WingPanel.Left);
            if(button) AddButton(text, tooltip, menu.Open, image);
            return menu;
        }
    }

    public class QuickMenuWingButton : UIElement
    {
        private static GameObject WingButtonTemplate => QuickMenuTemplates.GetWingButtonTemplate();
        protected TextMeshProUGUI buttonText;

        public QuickMenuWingButton(string text, string tooltip, Action onClick, Sprite sprite = null, bool left = true, bool arrow = true, bool background = true,
            bool separator = false) : base(WingButtonTemplate, (left ? QuickMenuExtensions.LeftWing : QuickMenuExtensions.RightWing).field_Public_RectTransform_0.Find("WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"), $"Button_{text}")
        {
            var container = RectTransform.Find("Container").transform;
            container.Find("Background").gameObject.SetActive(background);
            container.Find("Icon_Arrow").gameObject.SetActive(arrow);
            RectTransform.Find("Separator").gameObject.SetActive(separator);
            var iconImage = container.Find("Icon").GetComponent<Image>();
            if (sprite != null)
            {
                iconImage.sprite = sprite;
                iconImage.overrideSprite = sprite;
            }
            else
            {
                iconImage.gameObject.SetActive(false);
            }

            buttonText = container.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = text;
            buttonText.richText = true;

            var button = GameObject.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(new Action(onClick));

            var uiTooltip = GameObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
            uiTooltip.field_Public_String_0 = tooltip;
            uiTooltip.field_Public_String_1 = tooltip;
        }

        public QuickMenuWingButton(string text, string tooltip, Action onClick, Transform parent, Sprite sprite = null, bool arrow = true, bool background = true,
            bool separator = false) : base(WingButtonTemplate, parent, $"Button_{text}")
        {
            var container = RectTransform.Find("Container").transform;
            container.Find("Background").gameObject.SetActive(background);
            container.Find("Icon_Arrow").gameObject.SetActive(arrow);
            RectTransform.Find("Separator").gameObject.SetActive(separator);
            var iconImage = container.Find("Icon").GetComponent<Image>();
            if (sprite != null)
            {
                iconImage.sprite = sprite;
                iconImage.overrideSprite = sprite;
            }
            else
            {
                iconImage.gameObject.SetActive(false);
            }

            buttonText = container.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = text;
            buttonText.richText = true;

            var button = GameObject.GetComponent<Button>();
            button.onClick = new Button.ButtonClickedEvent();
            button.onClick.AddListener(new Action(onClick));

            var uiTooltip = GameObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
            uiTooltip.field_Public_String_0 = tooltip;
            uiTooltip.field_Public_String_1 = tooltip;
        }

        public void SetButtonText(string newText)
        {
            buttonText.text = newText;
        }
    }

    public class QuickMenuWingToggleButton
    {
        protected QuickMenuWingButton Button;
        protected bool CurrentState;
        protected Action onAction;
        protected Action offAction;
        protected string onText;
        protected string offText;
        protected Sprite onIcon = QuickMenuTemplates.GetToggleOnIconTemplate();
        protected Sprite offIcon = QuickMenuTemplates.GetToggleOffIconTemplate();
        
        public QuickMenuWingToggleButton(string OnText, Action OnAction, string OffText, Action OffAction, Transform parent, string tooltip, bool left = true, bool defaultState = false)
        {
            CurrentState = defaultState;
            var img = CurrentState ? onIcon : offIcon;
            onAction = OnAction;
            offAction = OffAction;
            onText = $"<color=green>{OnText}</color>";
            offText = $"<color=red>{OffText}</color>";
            Button = new QuickMenuWingButton(defaultState ? onText : offText, tooltip, ProcessClick, parent, img, left);
            UpdateValue(CurrentState);
        }
        
        /// <summary>
        /// This Method is meant for updating button state without invoking its logic. This is useful for when a Keybind does the same thing as a button.
        /// </summary>
        /// <param name="value"></param>
        public void UpdateValue(bool value)
        {
            CurrentState = value;
            Button.RectTransform.Find("Container").transform.Find("Icon").GetComponent<Image>().sprite = CurrentState ? onIcon : offIcon;
            Button.SetButtonText(value ? onText : offText);
        }
        
        private void ProcessClick()
        {
            CurrentState = !CurrentState;
            if (CurrentState)
            {
                onAction.Invoke();
                Button.RectTransform.Find("Container").transform
                    .Find("Icon").GetComponent<Image>().sprite = onIcon;
                Button.SetButtonText(onText);
            }
            else
            {
                offAction.Invoke();
                Button.RectTransform.Find("Container").transform
                    .Find("Icon").GetComponent<Image>().sprite = offIcon;
                Button.SetButtonText(offText);
            }
        }
    }
}
