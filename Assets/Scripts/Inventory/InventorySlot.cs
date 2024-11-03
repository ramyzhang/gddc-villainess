using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    [HideInInspector]
    public Item item; // keeps track of item in slot

    private Button button;

    void Start() {
        icon = gameObject.transform.Find("Image").GetComponent<Image>();
        icon.enabled = false;
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => OnItemClick());
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

    public void OnItemClick() {
        if (item != null) {
            SendMessageUpwards("UpdateDetails", item);
        }
    }
}
