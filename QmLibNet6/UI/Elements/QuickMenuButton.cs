using Il2CppSystem.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using VRC.UI.Elements.Controls;

using static QuickMenuLib.QuickMenuLibMod;

using Object = UnityEngine.Object;

namespace QuickMenuLib.UI.Elements
{
    /// <summary>
    /// Credit to https://github.com/RequiDev/RemodCE
    /// </summary>
    public class QuickMenuButton : UIElement
    {
        private static GameObject ButtonTemplate => QuickMenuTemplates.GetSingleButtonTemplate();

        private readonly TextMeshProUGUI MyText;

        private readonly Button MyButton;

        public QuickMenuButton( string text, string tooltip, Action onClick, Transform parent, Sprite sprite = null) : base(ButtonTemplate, parent,
            $"Button_{text}")
        {
            MyText = GameObject.GetComponentInChildren<TextMeshProUGUI>();
            MyText.text = text;
            MyText.richText = true;

            if (sprite == null)
            {
                //_text.fontSize = 35;
                MyText.enableAutoSizing = true;
                MyText.color = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
                MyText.m_fontColor = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
                MyText.m_htmlColor = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
                MyText.transform.localPosition = new Vector3(MyText.transform.localPosition.x, -30f);

                var layoutElement = RectTransform.Find("Background").gameObject.AddComponent<LayoutElement>();
                layoutElement.ignoreLayout = true;

                var horizontalLayout = GameObject.AddComponent<HorizontalLayoutGroup>();
                horizontalLayout.padding.right = 10;
                horizontalLayout.padding.left = 10;
                Object.DestroyImmediate(RectTransform.Find("Icon").gameObject);
            }
            else
            {
                var iconImage = RectTransform.Find("Icon").GetComponent<Image>();
                iconImage.sprite = sprite;
                iconImage.overrideSprite = sprite;
            }

            Object.DestroyImmediate(RectTransform.Find("Icon_Secondary").gameObject);
            Object.DestroyImmediate(RectTransform.Find("Badge_Close").gameObject);
            Object.DestroyImmediate(RectTransform.Find("Badge_MMJump").gameObject);

            var uiTooltip = GameObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
            uiTooltip.field_Public_String_0 = tooltip;
            uiTooltip.field_Public_String_1 = tooltip;

            MyButton = GameObject.GetComponent<Button>();
            MyButton.onClick = new Button.ButtonClickedEvent();
            MyButton.onClick.AddListener(new Action(onClick));
        }
    }

    public class QuickMenuToggleButton : UIElement
    {
        private static GameObject ToggleButtonTemplate => QuickMenuTemplates.GetToggleButtonTemplate();



        private static Sprite OnIconSprite => QuickMenuTemplates.GetToggleOnIconTemplate();

        private readonly Toggle MyToggle;
        private object MyToggleIcon;

        private bool LastValue;

        public QuickMenuToggleButton(string text, string tooltip, Action<bool> onToggle, Transform parent, bool defaultValue = false) : base(ToggleButtonTemplate, parent, $"Button_Toggle{text}")
        {
            var iconOn = RectTransform.Find("Icon_On").GetComponent<Image>();
            iconOn.sprite = OnIconSprite;

            FindToggleIcon();

            MyToggle = GameObject.GetComponent<Toggle>();
            MyToggle.onValueChanged = new Toggle.ToggleEvent();
            MyToggle.onValueChanged.AddListener(new Action<bool>(OnValueChanged));
            MyToggle.onValueChanged.AddListener(new Action<bool>(onToggle));

            var tmp = GameObject.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = text;
            tmp.richText = true;
            tmp.color = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
            tmp.m_fontColor = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
            tmp.m_htmlColor = new Color(0.4157f, 0.8902f, 0.9765f, 1f);

            var uiTooltip = GameObject.GetComponent<VRC.UI.Elements.Tooltips.UiToggleTooltip>();
            uiTooltip.field_Public_String_0 = tooltip;
            uiTooltip.field_Public_String_1 = tooltip;

            Toggle(defaultValue);

            var edl = GameObject.AddComponent<EnableDisableListener>();
            edl.OnEnableEvent += UpdateToggle;
        }

        //https://github.com/RequiDev/ReMod.Core/commit/1d689a3f52ddc35287059d2adc4eed14822aa6fd
        private void FindToggleIcon()
        {
            var components = new Il2CppSystem.Collections.Generic.List<Component>();
            GameObject.GetComponents(components);

            foreach (var c in components)
            {
                var il2CppType = c.GetIl2CppType();
                var il2CppFields = il2CppType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                if (il2CppFields.Count != 1)
                    continue;

                if (!il2CppFields.Any(t => t.IsPublic && t.FieldType == Il2CppType.Of<Toggle>()))
                    continue;

                var realType = GetUnhollowedType(il2CppType);
                if (realType == null)
                {
                    Logger.Error("SHITS FUCKED!");
                    break;
                }
                MyToggleIcon = Activator.CreateInstance(realType, c.Pointer);
                break;
            }
        }

        private static readonly Dictionary<string, Type> DeobfuscatedTypes = new Dictionary<string, Type>();
        private static readonly Dictionary<string, string> ReverseDeobCache = new Dictionary<string, string>();

        private static void BuildDeobfuscationCache()
        {
            if (DeobfuscatedTypes.Count > 0)
                return;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in asm.TryGetTypes())
                    TryCacheDeobfuscatedType(type);
            }
        }

        private static void TryCacheDeobfuscatedType(Type type)
        {
            try
            {
                if (!type.CustomAttributes.Any())
                    return;

                foreach (var att in type.CustomAttributes)
                {
                    // Thanks to Slaynash for this

                    if (att.AttributeType == typeof(ObfuscatedNameAttribute))
                    {
                        string obfuscatedName = att.ConstructorArguments[0].Value.ToString();

                        DeobfuscatedTypes.Add(obfuscatedName, type);
                        ReverseDeobCache.Add(type.FullName, obfuscatedName);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        public static Type GetUnhollowedType(Il2CppSystem.Type cppType)
        {
            if (DeobfuscatedTypes.Count == 0)
            {
                BuildDeobfuscationCache();
            }

            var fullname = cppType.FullName;

            if (DeobfuscatedTypes.TryGetValue(fullname, out var deob))
                return deob;

            if (fullname.StartsWith("System."))
                fullname = $"Il2Cpp{fullname}";

            return null;
        }


        public void Toggle(bool value, bool callback = true)
        {
            LastValue = value;
            MyToggle.Set(value, callback);
        }

        private void UpdateToggle()
        {
            OnValueChanged(LastValue);
        }

        private List<Action<bool>> _onValueChanged;

        private void OnValueChanged(bool arg0)
        {
            if (_onValueChanged == null)
            {
                _onValueChanged = new List<Action<bool>>();
                foreach (var methodInfo in MyToggleIcon.GetType().GetMethods().Where(m =>
                             m.Name.StartsWith("Method_Private_Void_Boolean_PDM_") && Utils.CheckMethod(m, "Toggled")))
                {
                    _onValueChanged.Add((Action<bool>)Delegate.CreateDelegate(typeof(Action<bool>), MyToggleIcon, methodInfo));
                }
            }

            foreach (var onValueChanged in _onValueChanged)
            {
                onValueChanged(arg0);
            }
        }
    }
}
