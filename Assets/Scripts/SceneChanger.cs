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
        GameManager.Instance.LoadScene(ConnectedSceneName);
    }
}