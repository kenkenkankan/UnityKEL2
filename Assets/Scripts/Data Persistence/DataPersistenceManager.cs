using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("Debuging Mode")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool overrideProfileID = false;
    [SerializeField] private bool debugMode = false;

    [Header("File Storage Configurations")]
    [SerializeField] private string fileName;
    private string selectedProfileId;
    private string testProfileID = "test";
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

        if (disableDataPersistence) Debug.LogWarning("Data persistence is disable");

        Instance = this;
        dataHandler = new(Application.persistentDataPath, fileName, useEncryption);
        selectedProfileId = dataHandler.GetMostRecentUpdatedProfile();

        if (overrideProfileID)
        {
            selectedProfileId = testProfileID;
            Debug.LogWarning("Override profile id active");
        }
        LoadSaveData();
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
        LoadSaveData();// ?
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded");
        SaveGame(); //temp
    }

    public void SetProfileId(string newProfileId)
    {
        selectedProfileId = newProfileId;
    }

    public void LoadSaveData()
    {
        data = dataHandler.Load(selectedProfileId); // Load any save data            
    }

    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(data.sceneName);
    }

    public void NewGame()
    {
        data = new GameData();
        LoadScene();
    }

    public void LoadGame()
    {
        if (disableDataPersistence) return;


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
        if (disableDataPersistence) return;

        foreach (IDataPersistence dataPersistence in dataPersistencesObjects)
            dataPersistence.SaveData(data);

        dataHandler.Save(data, selectedProfileId);
    }

    public void DeleteData(string profileId)
    {
        dataHandler.DeleteData(profileId);
        selectedProfileId = dataHandler.GetMostRecentUpdatedProfile();
        LoadSaveData();
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

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfile();
    }
}
