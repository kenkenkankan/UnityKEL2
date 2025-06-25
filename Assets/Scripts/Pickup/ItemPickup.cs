using UnityEditor.Rendering;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public ItemObject item;

    [SerializeField] private GameObject confirmNotif;
    public GameObject ConfirmNotif => confirmNotif;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory.Instance.StoreItem(item);
            PlayerInventory.Instance.ShowDescription(item);
            Destroy(gameObject); // atau bisa gunakan SetActive(false) jika ingin tampil efek pickup dulu
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            confirmNotif.SetActive(true);
            Debug.Log("Press E to pickup - Enter");
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayerInRange)
        {
            isPlayerInRange = false;
            confirmNotif.SetActive(false);
            Debug.Log($"Press E to pickup - Exit");
        }
    }
}
