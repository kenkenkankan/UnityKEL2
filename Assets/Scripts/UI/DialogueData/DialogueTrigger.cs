using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string characterId;
    public string Id { get => characterId; set => characterId = value ; }

    [Header("Dialogue Lines Configurations")]
    [SerializeField] private string currentDialogueKey; // Default section dialogue pertama
    [SerializeField] private List<string> completedDialogueKeys; // Referensi untuk persistence check

    // Pengganti single dialogue lines
    [SerializeField] SerializableDictionary<string, Dialogue> dialogues = new ();
    
    
    private bool isPlayerInRange = false;

    [SerializeField] private GameObject confirmNotif;
    public GameObject ConfirmNotif => confirmNotif;

    void Update()
    {
        // Cegah memicu dialog berulang saat dialog masih aktif
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.isDialogueActive)
        {
            TriggerDialogue();
            Debug.Log("Dialogue Triggered: ");
        }
    }

    public void TriggerDialogue()
    {
        if (!DialogueManager.Instance.isDialogueActive)
        {
            foreach (var dialogue in dialogues)
            if (dialogue.Key == currentDialogueKey)
            {
                DialogueManager.Instance.StartDialogue(dialogue.Value);
                DialogueManager.Instance.SetActiveSpeaker(this);
                
                Debug.Log("Dialogue Triggered: " + dialogue.Value.dialogueLines.Count + " lines available.");
            }
            // DialogueManager.Instance.StartDialogue(dialogue);
        }
    }

    public void SetNextDialogueSection()
    {
        completedDialogueKeys.Add(currentDialogueKey);
        
        foreach (var dialogueSection in dialogues)
        {
            if (!completedDialogueKeys.Contains(dialogueSection.Key))
            {
                currentDialogueKey = dialogueSection.Key;
                break;
            }
        }
    }    

    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            isPlayerInRange = true;
            confirmNotif.SetActive(true);
            Debug.Log("Player entered dialogue area");
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            isPlayerInRange = false;
            confirmNotif.SetActive(false);
            Debug.Log("Player left dialogue area");
        }
    }

    public void LoadData(GameData data)
    {
        var dialogueKeys = data.dialoguesKeyPoints.GetValueOrDefault(currentDialogueKey);
        if (dialogueKeys != null)
        {
            foreach (var key in dialogueKeys)
                completedDialogueKeys.Add(key);
        }
    }

    public void SaveData(GameData data)
    {
        if (!data.dialoguesKeyPoints.ContainsKey(Id))
            data.dialoguesKeyPoints.Add(Id, new());

        foreach (var key in completedDialogueKeys)
        {
            if (!data.dialoguesKeyPoints[Id].Contains(key))
               data.dialoguesKeyPoints[Id].Add(key);
        }
    }
}