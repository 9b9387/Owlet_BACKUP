using UnityEngine;

namespace Owlet
{
    [CreateAssetMenu(fileName = "AssetConfig", menuName = "Owlet/AssetConfigSO")]
    public class AssetConfigSO : ScriptableObject
    {
        public string assetRootPath;
        public AssetLoadType loadType;
    }

}