#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;

namespace QuickMenuLib
{
    public static class Utils
    {
        public static System.Random random = new System.Random();

        public static string RandomNumbers()
        {
            return random.Next(1000, 9999).ToString();
        }
        
        // https://github.com/knah/
        public static GameObject? FindInactive(string path)
        {
            var split = path.Split(new[]{'/'}, 2);
            var rootObject = GameObject.Find($"/{split[0]}")?.transform;
            if (rootObject == null) return null;
            return Transform.FindRelativeTransformWithPath(rootObject, split[1], false)?.gameObject;
        }

        public static bool CheckMethod(MethodInfo method, string match)
        {
            try
            {
                foreach (var instance in XrefScanner.XrefScan(method))
                {
                    if (instance.Type == XrefType.Global && instance.ReadAsObject().ToString().Contains(match))
                        return true;
                }

                return false;
            }
            catch
            {
                // ignored
            }

            return false;
        }

    }
    

    //https://github.com/RequiDev/ReMod.Core/blob/1d00b095a1ab8255fb58b1df53df216ea24d4b15/Unity/EnableDisableListener.cs
    internal class EnableDisableListener : MonoBehaviour
    {
        [method: HideFromIl2Cpp]
        public event Action OnEnableEvent;
        [method: HideFromIl2Cpp]
        public event Action OnDisableEvent;

        public EnableDisableListener(IntPtr obj) : base(obj) { }
        public void OnEnable() => OnEnableEvent?.Invoke();
        public void OnDisable() => OnDisableEvent?.Invoke();
    }

    //https://github.com/RequiDev/ReMod.Core/blob/1d689a3f52ddc35287059d2adc4eed14822aa6fd/AssemblyExtensions.cs#L12
    internal static class AssemblyExtensions
    {
        public static IEnumerable<Type> TryGetTypes(this Assembly asm)
        {
            try
            {
                return asm.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                try
                {
                    return asm.GetExportedTypes();
                }
                catch
                {
                    return e.Types.Where(t => t != null);
                }
            }
            catch
            {
                return Enumerable.Empty<Type>();
            }
        }
    }
}
