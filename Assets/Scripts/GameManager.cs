using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager Instance;

    [SerializeField] private string chapterName;

    private Scene currentScene, previousScreen;
    public Scene CurrentScene => currentScene;
    public Scene PreviousScreen => previousScreen;

    public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public static event Action<GameState> OnGameStateChanged = delegate { };

    public enum GameState
    {
        Gameplay, Paused, SceneTransition, GameOver,
    }

    [Header("State")]
    public GameState currentState;
    public GameState previousState;

    private void Awake()
    {
        // Singleton Instance check
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Extra is deleted");
            Destroy(gameObject);
            return;

            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        currentScene = SceneManager.GetActiveScene();

        InitState();

        Debug.Log("Start PlayerInput");
    }

    public void LoadScene(string SceneName)
    {
        previousScreen = currentScene;
        SceneManager.LoadSceneAsync(SceneName);
        // currentScene akan diatur ulang otomatis oleh OnSceneLoaded
    }


    #region GameState
    private void InitState()
    {
        SetGameState(GameState.Gameplay);
    }

    public void SetGameState(GameState newState)
    {
        if (currentState == newState)
            return;
        
        OnGameStateChanged?.Invoke(newState);

        currentState = newState;
        switch (currentState)
        {
            case GameState.Gameplay:
                Time.timeScale = 1f;
                // PlayerInput.Instance.EnableInput(true);
                break;
            case GameState.Paused:
                Time.timeScale = 1f;
                // PlayerInput.Instance.EnableInput(false);
                break;
            case GameState.SceneTransition:
                Time.timeScale = 1f;
                // PlayerInput.Instance.EnableInput(true);
                // Scene Transition stuff
                break;
            case GameState.GameOver:
                Time.timeScale = 1f;
                // PlayerInput.Instance.EnableInput(true);
                // Open Game Over Screen / Transition
                break;
            default:
                Debug.LogWarning("Unknown Game State");
                break;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene;
        SetGameState(GameState.Gameplay);
    }

    public void LoadData(GameData data)
    {
        throw new NotImplementedException();
    }

    public void SaveData(GameData data)
    {
        throw new NotImplementedException();
    }

    #endregion
}
