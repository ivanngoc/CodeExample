using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Создает сигнал через определенное время
/// </summary>
public class GeneratorSignal : MonoBehaviour
{
    [SerializeField] GeneratorType generatorType;
    [Space]
    [SerializeField] float randomMin;
    [SerializeField] float randomMax;
    [SerializeField] float strict;
    [Space]
    [SerializeField] float leftToNewSignal;

    public static event Action OnSignalEvent;

    #region Unity Message
    private void Awake()
    {
        ManagerWebTextures.OnRequestCompleteEvent += ScheduleNewSignal;
    }

    private void Reset()
    {
        generatorType = GeneratorType.PeriodicalRandom;
    }
    private void Update()
    {
        leftToNewSignal -= Time.deltaTime;

        if (leftToNewSignal < 0)
        {
            OnSignalEvent?.Invoke();

            enabled = false;
        }
    }
    #endregion

    public void ScheduleNewSignal()
    {
        switch (generatorType)
        {
            case GeneratorType.PeriodicalStrict:
                leftToNewSignal = strict;
                break;
            case GeneratorType.PeriodicalRandom:
                GenerateRandom();
                break;
            default:
                break;
        }
        enabled = true;
    }
    public void GenerateRandom()
    {
        leftToNewSignal = Random.Range(randomMin, randomMax);

        //Debug.Log($"New time: {leftToNewSignal}");
    }
}


public enum GeneratorType
{
    PeriodicalStrict,
    PeriodicalRandom
}
