using UnityEngine;

public class NPCStats : CharacterStats
{
    [SerializeField] private DialogueObject dialogueObject;

    public class DialogueActivator : IInteractable
    {
        public DialogueObject dialogueObject;
        public Vector2 initPosition;
        public Vector2 InitPosition => initPosition;

        [SerializeField] public GameObject confirmNotif;
        public GameObject ConfirmNotif => confirmNotif;

        public void Interact(PlayerInput player)
        {
            player.DialogueUI.ShowDialogue(dialogueObject);
        }
    }

    public DialogueActivator dialogueActivator;

    void Start()
    {
        dialogueActivator = new()
        {
            dialogueObject = dialogueObject,
            initPosition = transform.position,
            confirmNotif = transform.GetChild(0).gameObject
        };

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerInput player))
        {
            player.Interactable = dialogueActivator;
            dialogueActivator.confirmNotif.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerInput player))
        {
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this.dialogueActivator)
            {
                player.Interactable = null;
                dialogueActivator.confirmNotif.SetActive(false);
            }
        }
    }

    
}
