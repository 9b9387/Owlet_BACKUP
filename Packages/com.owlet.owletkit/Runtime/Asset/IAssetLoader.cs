using UnityEngine;

namespace Owlet
{
    public interface IAssetLoader
    {
        public T Load<T>(string assetPath) where T : Object;
    }
}