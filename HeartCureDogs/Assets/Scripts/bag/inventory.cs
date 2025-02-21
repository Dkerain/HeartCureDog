using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public List<ItemData> items = new List<ItemData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogError("Trying to add a null item to inventory!");
            return;
        }

        items.Add(item);
        Debug.Log($"Item added to inventory: {item.itemName}");

        // ¸üÐÂ±³°üUI
        InventoryUI ui = FindObjectOfType<InventoryUI>();
        if (ui != null)
        {
            ui.UpdateUI();
        }
    }
}