using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int id;
    public string itemName;
    public Sprite icon;

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ItemData itemToAdd = new ItemData
            {
                id = this.id,
                itemName = this.itemName,
                icon = this.icon
            };

            if (Inventory.Instance != null)
            {
                Inventory.Instance.AddItem(itemToAdd);
                Debug.Log($"Added item: {itemToAdd.itemName}");
            }
            else
            {
                Debug.LogError("Inventory Instance is not initialized!");
            }

            Destroy(gameObject);
        }
    }
}