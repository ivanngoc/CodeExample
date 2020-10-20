using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Компонент-метка для 
/// <see cref="SystemRotation"/>
/// </summary>
public class ComponentRotation : MonoBehaviour
{
    public static SystemRotation systemRotation;

    [SerializeField] public Vector3 center;
    [SerializeField] public float speed;

    #region Unity Message

    #endregion

    public void RotateAroundCenter()
    {
        transform.RotateAround(center, Vector3.up, Time.deltaTime * speed);
    }
}
