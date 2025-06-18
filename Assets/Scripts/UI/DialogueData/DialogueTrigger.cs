using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool isPlayerInRange = false;
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
            DialogueManager.Instance.StartDialogue(dialogue);
            Debug.Log("Dialogue Triggered: " + dialogue.dialogueLines.Count + " lines available.");
        }
    }


    private void OnTriggerEnter(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player entered dialogue area");
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player left dialogue area");
        }
    }
}