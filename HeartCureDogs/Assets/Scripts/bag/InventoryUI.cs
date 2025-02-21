using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    public RectTransform itemsPanel;
    public GameObject itemPrefab;

    private List<GameObject> itemUIList = new List<GameObject>();

    public void UpdateUI()
    {
        ClearInventoryUI();

        List<ItemData> items = Inventory.Instance.items;
        foreach (ItemData item in items)
        {
            GameObject itemUI = Instantiate(itemPrefab, itemsPanel);
            itemUI.SetActive(true);

            Image itemIcon = itemUI.GetComponentInChildren<Image>();
            Text itemNameText = itemUI.GetComponentInChildren<Text>();

            if (itemIcon != null)
            {
                itemIcon.sprite = item.icon;
            }
            if (itemNameText != null)
            {
                itemNameText.text = item.itemName;
            }

            itemUIList.Add(itemUI);
        }
    }

    private void ClearInventoryUI()
    {
        foreach (GameObject itemUI in itemUIList)
        {
            Destroy(itemUI);
        }
        itemUIList.Clear();
    }

    private void Start()
    {
        UpdateUI();
    }
}