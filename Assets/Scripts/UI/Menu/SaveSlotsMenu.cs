using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
    [Header("Menu Navigation")]
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private DeleteSaveConfirm deleteConfirm;
    [SerializeField] private Button backButtom;
    
    [Header("Slots")]
    [SerializeField] private List<SaveSlot> saveSlots;

    [Header("Profile Deletion")]
    [SerializeField] private GameObject deletionWindow;


    private void Awake()
    {
        saveSlots = GetComponentsInChildren<SaveSlot>().ToList();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        DataPersistenceManager.Instance.SetProfileId(saveSlot.GetProfileId());
        if (!saveSlot.hasData)
            DataPersistenceManager.Instance.NewGame();
        else
        {
            DataPersistenceManager.Instance.LoadSaveData();
            DataPersistenceManager.Instance.LoadScene();
        }
    }
    
    public void ShowDeleteConfirmation(string profileId)
    {
        deleteConfirm.ActivateMenu(profileId);
    }

    public void DeleteSaveData(string profileId)
    {
        DataPersistenceManager.Instance.DeleteData(profileId);
        deleteConfirm.DeactivateMenu();
    }

    public override void ActivateMenu()
    {
        base.ActivateMenu();

        Dictionary<string, GameData> profileGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        foreach (var saveSlot in saveSlots)
        {
            profileGameData.TryGetValue(saveSlot.GetProfileId(), out GameData profileData);
            saveSlot.SetData(profileData);
        }
    }

    public override void DeactivateMenu()
    {
        base.DeactivateMenu();
        mainMenu.ActivateMenu();
    }

    private void DisableMenuButtons()
    {
        foreach (var saveSlot in saveSlots)
            saveSlot.SetInteractable(false);
        backButtom.interactable = false;
    }
}
