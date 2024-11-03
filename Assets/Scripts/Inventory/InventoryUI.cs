using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    InventorySlot[] slots;
    Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI; //calls updateui on event

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        Debug.Log("Slots: " + slots.Length);

    }

    void UpdateUI() //iterate through inventoryslots
    {
        Debug.Log("Updating UI");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.items.Count)
            {
                slots[i].addItemToUI(Inventory.items[i]);
            } else
            {
                slots[i].clearSlot();
            }
        }
    }
}
