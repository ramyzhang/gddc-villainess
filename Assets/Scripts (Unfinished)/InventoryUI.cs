using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    InventorySlot[] slots;
    Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.Instance;
        // inventory.onItemChangedCallback += UpdateUI; //calls updateui on event

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

    }

    void UpdateUI() //iterate through inventoryslots
    {
        Debug.Log("Updating UI");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].addItemToUI(inventory.items[i]);
            } else
            {
                slots[i].clearSlot();
            }
        }
    }
}
