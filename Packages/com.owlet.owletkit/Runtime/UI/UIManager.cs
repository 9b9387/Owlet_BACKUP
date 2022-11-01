// UIManager.cs
// Author: shihongyang shihongyang@weile.com
// Data: 10/24/2022
using UnityEngine;

namespace Owlet
{
    public static class UIManager
    {
        private static Transform uiroot;

        public static Transform UIRoot
        {
            get
            {
                if (uiroot != null)
                {
                    return uiroot;
                }

                var obj = GameObject.Find("UI");
                if (obj == null)
                {
                    obj = new GameObject("UI");
                }
                uiroot = obj.transform;
                return uiroot;
            }
        }

        public static T PushView<T>() where T : UIBaseView
        {
            var t = typeof(T);
            var attributes = t.GetCustomAttributes(typeof(UIAttribute), false);
            if (attributes.Length == 0)
            {
                Debug.LogWarning($"UIManager.PushView prefab path is null, Use UI attribute to class.");
                return null;
            }
            var attribute = attributes[0] as UIAttribute;
            string prefabPath = attribute.PrefabPath;

            Debug.Log($"UIManager.PushView Instantiate {t.Name} prefab path is {prefabPath}");
            var obj = AssetLoader.Instantiate(prefabPath);
            if (obj == null)
            {
                Debug.LogWarning($"UIManager.PushView can't find prefab at {prefabPath}");
                return null;
            }
            obj.transform.SetParent(UIRoot, false);
            var view = obj.AddComponent<T>();

            return view;
        }
    }
}