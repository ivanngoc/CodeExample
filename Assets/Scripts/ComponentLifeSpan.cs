using UnityEngine;
/// <summary>
/// <see cref="SystemLifeSpan"/>
/// </summary>
public class ComponentLifeSpan : MonoBehaviour
{
    public static SystemLifeSpan systemLifeSpan;

    public float lifeTimeMax;
    public float lifeTimeCurrent;
    /// <summary>
    /// Флаг: время жизни объекта закончилось
    /// </summary>
    public bool endLife;

    #region Unity Message


    #endregion

    public void SetLifeSpan(int lifeInSeconds)
    {
        lifeTimeMax = lifeInSeconds;
        lifeTimeCurrent = lifeInSeconds;
    }
    public bool SelfCheck()
    {
        lifeTimeCurrent -= Time.deltaTime;

        return endLife = lifeTimeCurrent < 0;
    }
}
