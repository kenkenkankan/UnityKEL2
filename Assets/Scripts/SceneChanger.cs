using UnityEngine;

public class SceneChanger : MonoBehaviour, IInteractable
{
    private Vector2 targetPos;
    private Vector2 initPosition;
    public Vector2 InitPosition => initPosition;

    [SerializeField] private GameObject confirmNotif;
    public GameObject ConfirmNotif => confirmNotif;

    public string ConnectedSceneName;

    void Start()
    {
        initPosition = transform.position;
        confirmNotif = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerInput player))
        {
            player.Interactable = this;
            confirmNotif.SetActive(true);

            Debug.Log($"Player {player.name} entered the trigger for scene change to {ConnectedSceneName}.");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerInput player))
        {
            player.Interactable = null;
            confirmNotif.SetActive(false);
        }
    }
    
    public void Interact(PlayerInput player)
    {
        Debug.Log($"Interacting: Changing to scene {ConnectedSceneName}");
        GameManager.Instance.LoadScene(ConnectedSceneName);
    }
}