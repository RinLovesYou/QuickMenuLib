using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuickMenuLib.UI.Elements
{
    public class QuickMenuSlider : UIElement
    {

        private static GameObject SliderTemplate => QuickMenuTemplates.GetSliderPrefab();

        private readonly TextMeshProUGUI MyText;

        public string Text
        {
            get => MyText.text;
            set => MyText.SetText(value);
        }

        private readonly Slider MySlider;

        public QuickMenuSlider(string text, string tooltip, Action<float> onValueChanged, Transform parent, float minValue = 0, float maxValue = 1) : base(SliderTemplate, parent, $"Slider_{text}")
        {
            MyText = GameObject.GetComponentInChildren<TextMeshProUGUI>();
            MyText.text = text;
            MyText.richText = true;

            MySlider = GameObject.GetComponentInChildren<Slider>();
            MySlider.minValue = minValue;
            MySlider.maxValue = maxValue;
            MySlider.onValueChanged = new Slider.SliderEvent();
            MySlider.onValueChanged.AddListener(new Action<float>(onValueChanged));
            
            var uiTooltip = GameObject.GetComponent<VRC.UI.Elements.Tooltips.UiTooltip>();
            uiTooltip.field_Public_String_0 = tooltip;
            uiTooltip.field_Public_String_1 = tooltip;
        }
    }
}
