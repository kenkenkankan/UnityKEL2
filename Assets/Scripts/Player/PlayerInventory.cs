using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Serializable]
    public class Slot
    {
        public Item item;
        public Image image;
        public Button itemButton;
        public void AssignItem(Item item)
        {

        }

        public void ClearSlot()
        {

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

        public void AssignItem(Item item)
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
    public List<Item> itemJournal = new();
    public ItemDescriptionUI itemUI;

    private void Awake()
    {
        // Singleton Instance check
        // if (Instance != null && Instance != this)
        // {
        //     Debug.LogWarning("Extra is deleted");
        //     Destroy(gameObject);
        //     return;
        // }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PopulateItemSlotElemets();
        PopulateItemUI();
    }

    private void PopulateItemSlotElemets()
    {
        GameObject go = GameObject.FindWithTag("Item Slots"); 
        foreach (var slot in itemSlots.Select((value, index) => new {value, index} ))
        {
            GameObject goChild = go.transform.GetChild(slot.index).gameObject;
            slot.value.image = goChild.GetComponent<Image>();
            slot.value.itemButton = goChild.GetComponent<Button>();
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

    public void StoreItem(Item item)
    {
        if (itemSlots.Count >= 3)
        {
            // Open monologue
            return;
        }

        foreach (var slot in itemSlots)
        {
            if (!slot.item)
            {
                slot.item = item;
                break;
            }
        }
    }

    // For item slot reference when it clicked
    public void GetItem(int itemSlotIndex)
    {
        var item = itemSlots[itemSlotIndex].item;
        ShowDescription(item);
    }

    void ShowDescription(Item item)
    {
        // itemUI.image.sprite = item.
        itemUI.image.gameObject.SetActive(true);
    }

    public void CloseDescription()
    {
        itemUI.image.gameObject.SetActive(false);
    }

    public void RemoveItem()
    {
        
    }
    
}
