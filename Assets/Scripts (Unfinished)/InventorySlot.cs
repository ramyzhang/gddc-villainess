using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;

    private Item item; // keeps track of item in slot 

    void Start() {
        icon = gameObject.GetComponentInChildren<Image>();
    }

    public void addItemToUI(Item newitem)
    {
        item = newitem;

        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void clearSlot()
    {
        // TODO: animation here
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }
}
