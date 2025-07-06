using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu SaveSlotMenu;
    [SerializeField] private OptionsMenu OptionsMenu;
    [SerializeField] private List<Button> menuButtons;// First to the last: start,continue, chapter, options, quite

    private void Start()
    {
        DeactivateContinueButtonDependingOnData();   
    }

    private void DeactivateContinueButtonDependingOnData()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            menuButtons[1].gameObject.SetActive(false);
            var nav = menuButtons[0].navigation;
            nav.selectOnUp = menuButtons[2];
            
            nav = menuButtons[2].navigation;
            nav.selectOnDown = menuButtons[0];
        }
    }

    public void ContinueFromTheMostRecentSave()
    {
        DataPersistenceManager.Instance.LoadScene();
        DisableMenuButtons();
    }

    public void GoToSaveMenu()
    {
        SaveSlotMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void GoToOptionMenu()
    {
        OptionsMenu.ActivateMenu();
        DeactivateMenu();
    }

    public void Quit()
    {
        // Buka window confirmasi dulu next
        Application.Quit();
    }

    public override void ActivateMenu()
    {
        DeactivateContinueButtonDependingOnData();
        base.ActivateMenu();
        Debug.Log("active!");
    }

    private void DisableMenuButtons()
    {
        foreach (Button button in menuButtons)
            button.interactable = false;
    }
}
