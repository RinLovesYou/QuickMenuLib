using UnityEngine;

using Object = UnityEngine.GameObject;

namespace QuickMenuLib
{

    /// <summary>
    /// The UIElement class provides a great template for Menu Controls.
    /// Credit to https://github.com/RequiDev/RemodCE
    /// </summary>
    public class UIElement
    {
        public string Name { get; }
        public Object GameObject { get; }
        public RectTransform RectTransform { get; }

        public UIElement(Object original, Transform parent, string name, bool defaultState = true)
        {
            GameObject = UnityEngine.Object.Instantiate(original, parent);
            GameObject.name = name;
            Name = GameObject.name;

            GameObject.SetActive(defaultState);
            RectTransform = GameObject.GetComponent<RectTransform>();
        }
    }
}
