using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

/// <summary>
/// Запрос картинок, преобразование двоичного результата в текстуру, хранение созданных текстур, генерация событий при получениее данных и создании текстур
/// </summary>
public class ManagerWebTextures : MonoBehaviour
{
    [SerializeField] int oneTimeTextureRequestCount;
    [SerializeField] List<string> uriSource;
    [Space]
    [SerializeField] List<Texture2D> texture2Ds;
    [SerializeField] List<byte[]> rawWebRequestDatas = new List<byte[]>();

    public static event Action<byte[]> OnTextureRawRecivedEvent;
    public static event Action<Texture> OnTextureCreatedEvent;
    public static event Action OnRequestCompleteEvent;

    int requestCountToCallback;
    #region Unity Message
    private void Awake()
    {
        // GeneratorSignal.OnSignalEvent += TriggerGetTextureTaskAsync;

        GeneratorSignal.OnSignalEvent += TriggerGetTextureCoroutine;
    }
    private void Reset()
    {
        oneTimeTextureRequestCount = 6;

        uriSource = new List<string>()
        {
           "https://img2.freepng.ru/20180412/ctw/kisspng-cube-transparency-and-translucency-download-sugar-cubes-5acf0e5e1034b2.3745075815235190700664.jpg",
           "https://upload.wikimedia.org/wikipedia/commons/4/48/Uniform_polyhedron-43-t0.png",
           "https://www.pngitem.com/pimgs/m/57-576140_rubiks-cube-transparent-background-hd-png-download.png",
           "https://atlas-content-cdn.pixelsquid.com/stock-images/pyramid-QJGakq9-600.jpg",
           "https://atlas-content-cdn.pixelsquid.com/stock-images/pyramid-mdQoqr6-600.jpg",
           "https://e7.pngegg.com/pngimages/242/562/png-clipart-maze-jigsaw-puzzles-v-cube-7-rubik-s-cube-cube.png"
        };
    }


    private void Update()
    {
        while (rawWebRequestDatas.Any())
        {
            byte[] b = rawWebRequestDatas.First();

            rawWebRequestDatas.Remove(b);

            CreateTextureAndNotify(b);
        }
    }

    #endregion

    public void TriggerGetTextureCoroutine()
    {
        requestCountToCallback = oneTimeTextureRequestCount;

        for (int i = 0; i < oneTimeTextureRequestCount; i++)
        {
            Coroutine coroutine = StartCoroutine(GetTextureCoroutine(uriSource[i]));
        }
    }

    public IEnumerator GetTextureCoroutine(string uri)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(uri))
        {
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
            {
                Debug.LogError(unityWebRequest.error, this);
            }
            else
            {
                Texture texture = DownloadHandlerTexture.GetContent(unityWebRequest);

                FindObjectOfType<ManagerMaterials>()?.CreateMaterialWithTextureAndNotify(texture);
            }
        }
        CheckCallback();
    }

    public void CheckCallback()
    {
        requestCountToCallback--;

        if (requestCountToCallback == 0)
        {
            OnRequestCompleteEvent?.Invoke();
        }
    }

    /// <summary>
    /// Веб запрос с помощью <see cref="Task"/>
    /// TODO: avoid UnitySynchronizationContext.EecuteTasks()
    /// </summary>
    public async void TriggerGetTextureTaskAsync()
    {
        Debug.Log("TriggerGetTextureAsync");

        List<Task<byte[]>> tasks = new List<Task<byte[]>>();

        for (int i = 0; i < oneTimeTextureRequestCount; i++)
        {
            Task<byte[]> task = ManagerWebRequest.GetRawBinFromWebRequestAsync(uriSource[i]);

            tasks.Add(task);
        }

        while (tasks.Any())
        {
            Task<byte[]> taskCompleted = await Task.WhenAny(tasks);

            tasks.Remove(taskCompleted);

            rawWebRequestDatas.Add(taskCompleted.Result);

            OnTextureRawRecivedEvent?.Invoke(taskCompleted.Result);

            //Debug.Log($"{tasks.Count} Time: {Time.realtimeSinceStartup} Size: {taskCompleted.Result.Length}");
        }

        OnRequestCompleteEvent?.Invoke();
    }

    public void CreateTextureAndNotify(byte[] rawData)
    {
        Texture2D texture2D = new Texture2D(default, default, TextureFormat.ARGB32, false);

        texture2D.LoadImage(rawData);

        texture2Ds.Add(texture2D);

        OnTextureCreatedEvent?.Invoke(texture2D);
    }
}


