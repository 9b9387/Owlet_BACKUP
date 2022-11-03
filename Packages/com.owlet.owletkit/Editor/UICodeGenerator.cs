// UICodeGenerator.cs
// Author: shihongyang shihongyang@weile.com
// Data: 10/31/2022
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Owlet
{
    public class UICodeGenerator
    {
        private static readonly string KEY = "owlet_editor_last_generate_uicode_file_path";

        [MenuItem("Assets/Owlet/生成UI代码", false)]
        public static void GenerateUICode()
        {

            var ids = Selection.assetGUIDs;
            var prefab_path = AssetDatabase.GUIDToAssetPath(ids[0]);
            var fi = new FileInfo(prefab_path);
            var filename = fi.Name.Split('.')[0];
            var path = PlayerPrefs.GetString(KEY);
            path = EditorUtility.SaveFilePanel("选择路径", path, filename, "cs");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            fi = new FileInfo(path);

            PlayerPrefs.SetString(KEY, fi.Directory.ToString());
            PlayerPrefs.Save();

            var code = GeneratorCode(prefab_path);
            SaveFile(path, code);

            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Owlet/生成UI代码", true)]
        public static bool ValidateGenerateUICode()
        {
            var activeObj = Selection.activeGameObject;
            if (activeObj == null)
            {
                return false;
            }

            return PrefabUtility.IsPartOfPrefabAsset(activeObj);
        }

        public static string GeneratorCode(string prefab)
        {
            var sb = new StringBuilder();

            var fi = new FileInfo(prefab);
            var class_name = fi.Name.Split('.')[0];

            sb.Append("using System;\n");
            sb.Append("using Owlet;\n");
            sb.Append("using UnityEngine;\n\n");

            var res_path = fi.FullName.Replace("\\", "/");
            res_path = res_path.Replace($"{Application.dataPath.Replace("Assets", "")}", "");
            var owletSetting = AssetDatabase.LoadAssetAtPath<OwletSettings>(OwletSettings.DEFAULT_SETTINGS_FILE);
            res_path = res_path.Replace($"{owletSetting.assetRootPath}/", "");
            sb.Append($"[UI(PrefabPath=\"{res_path}\")]\n");
            sb.Append($"public class {class_name} : UIBaseView\n");
            sb.Append("{\n");

            sb.Append("\tprotected override void OnLoad()\n\t{\n\t}\n\n");
            sb.Append("\tprotected override void OnUnload()\n\t{\n\t}\n\n");
            var obj = AssetDatabase.LoadAssetAtPath<GameObject>(prefab);
            var buttons = obj.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                var methodName = $"OnClick_{button.gameObject.name}";
                sb.Append($"\tpublic void {methodName}() \n");
                sb.Append("\t{\n\t}\n");

                if (i < buttons.Length - 1)
                {
                    sb.Append("\n");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static void SaveFile(string path, string content)
        {
            var sw = new StreamWriter(path, false, Encoding.UTF8);
            sw.Write(content);
            sw.Flush();
            sw.Close();
        }
    }
}