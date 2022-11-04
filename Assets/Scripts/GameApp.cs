using Owlet;
using UnityEngine;

public class GameApp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.PushView<UILoadingView>();
    }
}
