using UnityEngine;

public abstract class UIBaseView : MonoBehaviour
{
    public void BindUI()
    {

    }

    private void OnDestroy()
    {
        
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
