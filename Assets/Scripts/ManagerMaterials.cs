using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ManagerMaterials : MonoBehaviour
{
    [SerializeField] List<Material> materials;

    static public event Action<Material> OnMaterialCreatedEvent;

    #region Unity Message

    private void Awake()
    {
        ManagerWebTextures.OnTextureCreatedEvent += CreateMaterialWithTextureAndNotify;
    }
    #endregion

    /// <summary>
    /// Создает материал с стандартным шейдером и устанавливает основную текстуру по заданной
    /// </summary>
    /// <param name="texture"></param>
    public void CreateMaterialWithTextureAndNotify(Texture texture)
    {
        Material material = new Material(Shader.Find("Standard"));

        material.SetTexture("_MainTex", texture);

        materials.Add(material);

        OnMaterialCreatedEvent?.Invoke(material);
    }
}


