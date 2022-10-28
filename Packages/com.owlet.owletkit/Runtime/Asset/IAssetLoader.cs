using UnityEngine;

namespace Owlet.Asset
{
    public interface IAssetLoader
    {
        public T Load<T>(string assetPath) where T : Object;
    }
}