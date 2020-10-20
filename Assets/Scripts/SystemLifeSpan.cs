using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// <see cref="ComponentLifeSpan"/>
/// </summary>
public class SystemLifeSpan : MonoBehaviour
{
    [SerializeField] List<ComponentLifeSpan> componentLifeSpansActive = new List<ComponentLifeSpan>();
    [SerializeField] ManagerCubes managerCubes;

    #region Unity Message
    private void Reset()
    {
        managerCubes = FindObjectOfType<ManagerCubes>();
    }
    private void Awake()
    {
        ComponentLifeSpan.systemLifeSpan = this;
    }
    private void Update()
    {
        CheckLifeSpan();
    }
    #endregion

    public void Regist(ComponentLifeSpan componentLifeSpan)
    {
        componentLifeSpansActive.Add(componentLifeSpan);
    }

    public void RegistDe(ComponentLifeSpan componentLifeSpan)
    {
        componentLifeSpansActive.Remove(componentLifeSpan);
    }

    public void CheckLifeSpan()
    {
        foreach (var item in componentLifeSpansActive)
        {
            if (item.SelfCheck())
            {
                managerCubes.DeSpawnCube(item.gameObject);
            }
        }
    }
}