using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Slot Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TMP_Text chapterName;
    [SerializeField] private TMP_Text lastSaveTime;
    public bool hasData = false;

    private SaveSlotsMenu saveMenu;
    private Button saveSlotButton;

    void Awake()
    {
        saveMenu = GetComponentInParent<SaveSlotsMenu>();
        saveSlotButton = GetComponent<Button>();
    }

    public void SetData(GameData data)
    {
        if (data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            hasData = false;
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            hasData = true;
            chapterName.text = $"\"{data.chapterName}\"";
            DateTime date = DateTime.FromBinary(data.lastSaveTime);
            lastSaveTime.text = $"Last Time Saved:<br> {date:dd/MM/yyyy}";
        }
    }

    public void OnDeleteData()
    {
        saveMenu.ShowDeleteConfirmation(profileId);
    }

    public string GetProfileId()
    {
        return profileId;
    }

    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
    }
}
