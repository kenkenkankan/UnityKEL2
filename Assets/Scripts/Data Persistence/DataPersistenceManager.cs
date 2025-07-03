using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debuging Mode")]
    [SerializeField] private bool debugMode;

    [Header("File Storage Configurations")]
    [SerializeField] private string fileName;
    [SerializeField] private string profileName = "tmpName";
    [SerializeField] private bool useEncryption;
    private GameData data;
    [SerializeField] private List<IDataPersistence> dataPersistencesObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Instance already exist, destroy the extra");
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);

        Instance = this;
        dataHandler = new(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPersistencesObjects = FindAllDataPersistenceObjects();
        Debug.Log("OnSceneLoaded");
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded");
        SaveGame();
    }

    public void NewGame()
    {
        data = new GameData();
    }

    public void LoadGame()
    {
        data = dataHandler.Load(profileName); // Load any save data

        if (data == null)
        {
            Debug.Log("No data was found. A new game must be starte first");
            return;
        }

        if (debugMode) NewGame();

        foreach (IDataPersistence dataPersistence in dataPersistencesObjects)
            dataPersistence.LoadData(data);
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistence in dataPersistencesObjects)
            dataPersistence.SaveData(ref data);

        dataHandler.Save(data, profileName);
    }

    public string GetSceneName()
    {
        return data.sceneName;
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistencesObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistencesObjects);
    }

    public bool HasGameData()
    {
        return data != null;
    }

    public Dictionary<string, GameData> GetAllProfileGameData()
    {
        return dataHandler.LoadAllProfile();
    }
}
