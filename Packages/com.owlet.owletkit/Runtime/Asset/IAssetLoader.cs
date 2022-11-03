using UnityEngine;

namespace Owlet
{
    public interface IAssetLoader
    {
        public void SetAssetRootPath(string path);
        public T Load<T>(string assetPath) where T : Object;
        public void UnloadAll(bool unloadAllLoadedObjects);
    }
}