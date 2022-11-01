using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBaseView : MonoBehaviour
{
    private Dictionary<string, GameObject> children;
    private Dictionary<string, MethodInfo> buttonEvents; 

    private void Start()
    {
        OnLoad();
    }

    public GameObject FindGameObject(string name)
    {
        if(children.ContainsKey(name))
        {
            return children[name];
        }

        return null;
    }

    public T FindComponent<T>(string name) where T : Component
    {
        var go = FindGameObject(name);
        if(go == null)
        {
            return null;
        }
        return go.GetComponent<T>();
    }


    public void BindUI()
    {
        if(children != null)
        {
            Debug.LogWarning($"{gameObject.name} has Binded.");
            return;
        }

        // Cache game objects
        children = new Dictionary<string, GameObject>();
        var transforms = transform.GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < transforms.Length; i++)
        {
            var obj = transforms[i].gameObject;
            if (children.ContainsKey(obj.name))
            {
                Debug.LogWarning($"{obj.name} duplication.");
            }
            children[obj.name] = obj;
        }

        // Bind Button Events
        var buttons = gameObject.GetComponentsInChildren<Button>(true);
        buttonEvents = new Dictionary<string, MethodInfo>();
        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate ()
            {
                MethodInfo method;
                var methodName = $"OnClick_{button.name}";

                if (buttonEvents.ContainsKey(methodName))
                {
                    method = buttonEvents[methodName];
                }
                else
                {
                    method = GetType().GetMethod(methodName);
                    buttonEvents[methodName] = method;
                }

                method?.Invoke(this, null);
            });
        }
    }

    private void OnDestroy()
    {
        OnUnload();

        children.Clear();
        buttonEvents.Clear();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    protected virtual void OnUnload()
    {

    }

    protected virtual void OnLoad()
    {

    }
}
