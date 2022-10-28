// UIAttribute.cs
// Author: shihongyang shihongyang@weile.com
// Data: 10/26/2022
using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class UIAttribute : Attribute
{
    private string prefabPath;

    public string PrefabPath { get => prefabPath; set => prefabPath = value; }
}
