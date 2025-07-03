using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new();
    public bool proceedToNextSection = false;
}

