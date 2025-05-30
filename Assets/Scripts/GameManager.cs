using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Scene currentScene, previousScreen;
    public Scene CurrentScene => currentScene;
    public Scene PreviousScreen => previousScreen;



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
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        currentScene = SceneManager.GetActiveScene();

        InitState();
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadSceneAsync(SceneName);
        previousScreen = currentScene;
        currentScene = SceneManager.GetActiveScene();
    }

    #region GameState
    private void InitState()
    {
        SetGameState(GameState.Gameplay);
    }

    public void SetGameState(GameState newState)
    {
        if (currentState == newState) return;
        
        OnGameStateChanged?.Invoke(newState);

        currentState = newState;
        switch (currentState)
        {
            case GameState.Gameplay:
                Time.timeScale = 1f;
                // PlayerInput.Instance.EnableInput(true);
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                // PlayerInput.Instance.EnableInput(false);
                break;
            case GameState.SceneTransition:
                Time.timeScale = 0f;
                // PlayerInput.Instance.EnableInput(true);
                // Scene Transition stuff
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                // PlayerInput.Instance.EnableInput(true);
                // Open Game Over Screen / Transition
                break;
            default:
                Debug.LogWarning("Unknown Game State");
                break;
        }
    }
    #endregion
}
