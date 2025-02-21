using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel; // 背包面板的引用
    private bool isInventoryOpen = false; // 背包是否打开

    // 切换背包面板的显示状态
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen; // 切换状态
        inventoryPanel.SetActive(isInventoryOpen); // 根据状态显示或隐藏背包面板
    }

    // 关闭背包面板
    public void CloseInventory()
    {
        if (isInventoryOpen)
        {
            isInventoryOpen = false;
            inventoryPanel.SetActive(false); // 隐藏背包面板
        }
    }
}

