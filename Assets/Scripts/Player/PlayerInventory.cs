using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Serializable]
    public class Slot
    {
        public int index; // Optional, for reference
        public ItemObject item;
        public Image image;
        public Button itemButton;
        public Sprite icon;
        public void AssignItem(ItemObject item)
        {
            this.item = item;
            image.sprite = item.icon;
            image.enabled = true;
        }


        public void ClearSlot()
        {
            item = null;
            image.sprite = null;
            image.enabled = false;
        }


        public void IsEmpty()
        {

        }
    }

    [Serializable]
    public class ItemDescriptionUI
    {
        public TMP_Text itemName;
        public TMP_Text itemDescription;
        public Image image;
        public Button applyItemButton;
        public Button discradButton;
        public Button closeUIButton;

        public void AssignItem(ItemObject item)
        {
           
        }


        public void ClearSlot()
        {

        }

        public void IsEmpty()
        {

        }
    }

    public List<Slot> itemSlots = new(3);
    public List<ItemObject> itemJournal = new();
    public ItemDescriptionUI itemUI;

    private void Awake()
    {
        // Singleton Instance check
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Extra is deleted");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PopulateItemSlotElemets();
        PopulateItemUI();
    }

    private void PopulateItemSlotElemets()
    {
        GameObject go = GameObject.FindWithTag("Item Slots");
        foreach (var slot in itemSlots.Select((value, index) => new { value, index }))
        {
            GameObject goChild = go.transform.GetChild(slot.index).gameObject;
            slot.value.image = goChild.GetComponent<Image>();
            slot.value.itemButton = goChild.GetComponent<Button>();
            slot.value.index = slot.index; // simpan index ke dalam slot

            int capturedIndex = slot.index; // hindari closure problem
            slot.value.itemButton.onClick.AddListener(() =>
            {
                UseItem(capturedIndex); // atau bisa ganti ke RemoveItem(capturedIndex);
            });
        }
    }

    private void PopulateItemUI()
    {
        int index = 0;
        GameObject go = GameObject.FindWithTag("Inventory").transform.GetChild(0).gameObject;
        itemUI.itemName = go.transform.GetChild(index++).GetComponent<TMP_Text>();
        itemUI.itemDescription = go.transform.GetChild(index++).GetComponent<TMP_Text>();
        itemUI.image = go.transform.GetChild(index++).GetComponent<Image>();
        itemUI.applyItemButton = go.transform.GetChild(index++).GetComponent<Button>();
        itemUI.discradButton = go.transform.GetChild(index++).GetComponent<Button>();
        itemUI.closeUIButton = go.transform.GetChild(index).GetComponent<Button>();
    }

    public void StoreItem(ItemObject item)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.item == null)
            {
                slot.AssignItem(item);
                return;
            }
        }

        Debug.Log("Inventory penuh!");
        // Bisa tambahkan notifikasi di UI
    }


    // For item slot reference when it clicked
    public void GetItem(int itemSlotIndex)
    {
        var item = itemSlots[itemSlotIndex].item;
        ShowDescription(item);

    }

    public void ShowDescription(ItemObject item)
    {
        itemUI.itemName.text = item.itemName;
        itemUI.itemDescription.text = item.description;
        itemUI.image.sprite = item.icon;
        itemUI.image.gameObject.SetActive(true);

        itemUI.itemName.transform.parent.gameObject.SetActive(true);

        PauseGame(); // ⏸ Pause gameplay di sini

        itemUI.applyItemButton.onClick.RemoveAllListeners();
        itemUI.applyItemButton.onClick.AddListener(() =>
        {
            item.ApplyEffect(gameObject);
            RemoveItemFromInventory(item);
            CloseDescription();
        });

        itemUI.discradButton.onClick.RemoveAllListeners();
        itemUI.discradButton.onClick.AddListener(() =>
        {
            RemoveItemFromInventory(item);
            CloseDescription();
        });

        itemUI.closeUIButton.onClick.RemoveAllListeners();
        itemUI.closeUIButton.onClick.AddListener(() =>
        {
            CloseDescription();
        });
    }

    private void RemoveItemFromInventory(ItemObject item)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.item == item)
            {
                slot.ClearSlot();
                break;
            }
        }
    }

    public void CloseDescription()
    {
        itemUI.itemName.transform.parent.gameObject.SetActive(false);
        ResumeGame(); // ▶ Lanjutkan gameplay
    }


    public void RemoveItem()
    {
        
    }

    public void UseItem(int index)
    {
        var item = itemSlots[index].item;
        if (item != null)
        {
            item.ApplyEffect(gameObject); // Kirim player sebagai 'user'
            itemSlots[index].ClearSlot();
        }
    }

    public void ApplySelectedItem()
{
    var selectedItem = itemUI.image.sprite;
    for (int i = 0; i < itemSlots.Count; i++)
    {
        if (itemSlots[i].item != null && itemSlots[i].item.icon == selectedItem)
        {
            itemSlots[i].item.ApplyEffect(gameObject); // <-- Panggil efek item
            itemSlots[i].ClearSlot();
            CloseDescription();
            break;
        }
    }
}

    public bool TryUseStick()
    {
        foreach (var slot in itemSlots)
        {
            if (slot.item != null && slot.item.itemName == "Stick") // pastikan nama cocok
            {
                itemJournal.Remove(slot.item); // opsional: jika perlu log
                slot.ClearSlot(); // kosongkan slot
                return true;
            }
        }

        return false;
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }

}
