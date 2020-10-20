using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Вращает те объекты вокруг центра, которые имеют
/// <see cref="ComponentRotation"/>
/// </summary>
public class SystemRotation : MonoBehaviour
{
    [SerializeField] List<ComponentRotation> componentRotationsActive;

    #region Unity Message

    private void Awake()
    {
        ComponentRotation.systemRotation = this;
    }
    private void Update()
    {
        foreach (var item in componentRotationsActive)
        {
            item.RotateAroundCenter();
        }
    }

    #endregion

    public void Regist(ComponentRotation componentRotation)
    {
        componentRotationsActive.Add(componentRotation);
    }

    public void RegistDe(ComponentRotation componentRotation)
    {
        componentRotationsActive.Remove(componentRotation);
    }
}
