using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;
    public string Id { get => id; set => id = value; }
    
    [ContextMenu("Generate uid for id")] // Klik kanan di nama item di inspector, opsional
    public void GenerateGuID()
    {
        id = Guid.NewGuid().ToString();
    }

    private bool isPlayerInRange = false;
    public ItemObject item;

    private bool isCollected = false;

    [SerializeField] private GameObject confirmNotif;
    public GameObject ConfirmNotif => confirmNotif;


    public void LoadData(GameData data)
    {
        data.keyItemsCollected.TryGetValue(id, out isCollected);
        if (isCollected)
        {
            gameObject.SetActive(false);
        }    
    }

    public void SaveData(ref GameData data)
    {
        if (data.keyItemsCollected.ContainsKey(id))
        {
            data.keyItemsCollected.Remove(id);
        }
        data.keyItemsCollected.Add(id, isCollected);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory.Instance.StoreItem(item);
            PlayerInventory.Instance.ShowDescription(item);
            // Destroy(gameObject); // atau bisa gunakan SetActive(false) jika ingin tampil efek pickup dulu
            gameObject.SetActive(false);
            isCollected = true;
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
