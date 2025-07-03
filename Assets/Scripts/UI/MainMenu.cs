using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [SerializeField] private List<Button> menuButtons;

    public void NewGame()
    {
        DataPersistenceManager.Instance.NewGame();
        DisableMenuButtons();
        SceneManager.LoadSceneAsync("level0");
    }

    public void Continue()
    {
        DisableMenuButtons();
        string scene = DataPersistenceManager.Instance.GetSceneName();
        SceneManager.LoadSceneAsync(scene);
    }

    public void Quit()
    {
        // Buka window confirmasi dulu next
        Application.Quit();
    }

    private void DisableMenuButtons()
    {
        foreach (Button button in menuButtons)
            button.interactable = false;
    }
}
