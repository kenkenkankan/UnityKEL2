using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    Vector2 initPosition;
    public Vector2 InitPosition => initPosition;
    
    [SerializeField]
    GameObject confirmNotif;
    public GameObject ConfirmNotif => confirmNotif;

    [SerializeField]
    string[] monologueLines;
    public string[] MonologueLines => monologueLines;

    void Start()
    {
        initPosition = transform.position;
        confirmNotif = transform.GetChild(0).gameObject;
    }


    public void Interact(PlayerInput p)
    {
        Debug.Log("Interacting ");
        
        if (MonologueLines.Length > 0) { }

        if (!PlayerInventory.Instance.itemJournal.Contains(this))
        {
            PlayerInventory.Instance.itemJournal.Add(this);
        }

        PlayerInventory.Instance.StoreItem(this);
    }
}