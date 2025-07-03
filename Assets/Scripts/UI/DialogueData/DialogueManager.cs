using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;
    private Dialogue currentDialog;
    private DialogueTrigger activeSpeaker;
    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public Animator animator;

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextDialogueLine();
        }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
    }

    public void SetActiveSpeaker(DialogueTrigger trigger)
    {
        activeSpeaker = trigger;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (isDialogueActive) return; // Cegah double start

        isDialogueActive = true;

        animator.Play("show"); // hanya dipanggil sekali di awal

        lines.Clear();

        currentDialog = dialogue;

        lines.Enqueue(new()); // blank entry agar first entry muncul
        
        foreach (var dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }


    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;

        if (lines.Count <= 0)
            CheckNextDialogueSection();

        animator.Play("hide");

        currentDialog = null;
        activeSpeaker = null;
    }

    private void CheckNextDialogueSection()
    {
        if (currentDialog.proceedToNextSection)
            activeSpeaker.SetNextDialogueSection();
    }
}