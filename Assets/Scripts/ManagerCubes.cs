using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Создает/уничтожает кубы
/// </summary>
public class ManagerCubes : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform center;
    [SerializeField] List<GameObject> cubesSpawned;
    [SerializeField] List<GameObject> cubesToDestroy;

    [Space]
    [SerializeField] InputField inputField;
    [SerializeField] SystemLifeSpan systemLifeSpan;
    [SerializeField] SystemRotation systemRotation;

    #region Unity Message
    private void Reset()
    {
        center = GameObject.Find("Center")?.transform;
#if UNITY_EDITOR
        prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/PrefabCube.prefab", typeof(GameObject)) as GameObject;
#endif

        systemLifeSpan = FindObjectOfType<SystemLifeSpan>();
        systemRotation = FindObjectOfType<SystemRotation>();
        inputField = FindObjectOfType<InputField>();
    }
    private void Awake()
    {
        ManagerMaterials.OnMaterialCreatedEvent += SpawnCubeWithMaterialRadial;
    }

    public void LateUpdate()
    {
        DestroyCubes();
    }
    #endregion

    /// <summary>
    /// Создаать куб с заданной текстурой вокруг точки
    /// </summary>
    /// <param name="texture"></param>
    public void SpawnCubeWithMaterialRadial(Material material)
    {
        GameObject copy = Instantiate(prefab);

        MeshRenderer meshRenderer = copy.GetComponent<MeshRenderer>();

        meshRenderer.sharedMaterial = material;

        cubesSpawned.Add(copy);

        copy.transform.position = center.forward * cubesSpawned.Count + center.position;

        ComponentRotation componentRotation = copy.GetComponent<ComponentRotation>();

        componentRotation.speed = Random.Range(1f, 10f);

        componentRotation.center = center.position;

        ComponentLifeSpan componentLifeSpan = copy.GetComponent<ComponentLifeSpan>();

        componentLifeSpan.SetLifeSpan(int.Parse(inputField.text));

        systemLifeSpan.Regist(componentLifeSpan);

        systemRotation.Regist(componentRotation);

        copy.SetActive(true);
    }

    /// <summary>
    /// Планирует уничтожение кубов
    /// </summary>
    public void DeSpawnCube(GameObject gameObjectIn)
    {
        cubesToDestroy.Add(gameObjectIn);
    }
    /// <summary>
    /// Уничтожает кубы из списка <see cref="cubesToDestroy"/>
    /// </summary>
    public void DestroyCubes()
    {
        foreach (var item in cubesToDestroy)
        {
            cubesSpawned.Remove(item);

            systemLifeSpan.RegistDe(item.GetComponent<ComponentLifeSpan>());

            systemRotation.RegistDe(item.GetComponent<ComponentRotation>());

            Destroy(item);
        }
        cubesToDestroy.Clear();
    }
}




