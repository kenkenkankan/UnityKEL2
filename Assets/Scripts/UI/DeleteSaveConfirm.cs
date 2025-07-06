using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSaveConfirm : Menu
{
    [SerializeField] SaveSlotsMenu saveSlotsMenu;

    [SerializeField] TMP_Text confirmText;
    [SerializeField] Button deleteButton;

    public void ActivateMenu(string profileId)
    {
        base.ActivateMenu();
        confirmText.text = $"Are you sure want to delete save file in the '{profileId}?";
        deleteButton.onClick.AddListener(() => saveSlotsMenu.DeleteSaveData(profileId));
    }

    public override void DeactivateMenu()
    {
        base.DeactivateMenu();
        saveSlotsMenu.ActivateMenu();
    }

}
