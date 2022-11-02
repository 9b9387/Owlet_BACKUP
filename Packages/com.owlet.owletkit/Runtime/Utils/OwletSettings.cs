// OwletSettings.cs
// Author: shihongyang shihongyang@weile.com
// Data: 10/26/2022
using UnityEngine;
using System.IO;

namespace Owlet
{
    [System.Serializable]
    public class OwletSettings : ScriptableObject
    {
        public static readonly string DEFAULT_SETTINGS_PATH = Path.Combine("Assets", "Settings", "Owlet");
        public static readonly string DEFAULT_SETTINGS_FILE = Path.Combine(DEFAULT_SETTINGS_PATH, "OwletSettings.asset");

        public string assetRootPath;
    }
}