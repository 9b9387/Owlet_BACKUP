// OwletSettings.cs
// Author: shihongyang shihongyang@weile.com
// Data: 10/26/2022
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.IO;

namespace Owlet
{
    public class OwletSettingsProvider : SettingsProvider
    {
        private SerializedObject config;

        [SettingsProvider]
        public static SettingsProvider CreateOwletSettingsProvider()
        {
            var provider = new OwletSettingsProvider("Project/Owlet Settings", SettingsScope.Project);
            return provider;
        }

        public OwletSettingsProvider(string path, SettingsScope scopes,
            IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
        }
        
        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            if (config == null)
            {
                config = GetSerializedSettings();
            }
            if (config == null)
            {
                return;
            }
            EditorGUILayout.PropertyField(config.FindProperty("assetRootPath"), new GUIContent("Asset Root Path"));
            EditorGUILayout.PropertyField(config.FindProperty("loadType"), new GUIContent("Load Type"));
            config.ApplyModifiedProperties();
        }


        public static OwletSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<OwletSettings>(OwletSettings.DEFAULT_SETTINGS_FILE);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<OwletSettings>();
                settings.assetRootPath = Path.Combine("Assets", "GameAssets");

                if (Directory.Exists(OwletSettings.DEFAULT_SETTINGS_PATH) == false)
                {
                    Directory.CreateDirectory(OwletSettings.DEFAULT_SETTINGS_PATH);
                }
                AssetDatabase.CreateAsset(settings, OwletSettings.DEFAULT_SETTINGS_FILE);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}