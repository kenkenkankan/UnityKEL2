using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public ItemObject item;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory.Instance.StoreItem(item);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerInput player))
        {
            isPlayerInRange = true;
            Debug.Log($"Press E to pickup");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayerInRange)
        {
            isPlayerInRange = false;
            Debug.Log($"Press E to pickup - Exit");
        }
    }
}
