using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    
    [Space(10)]
    [Header("Inventory Details")]
    public GameObject detailsPanel;
    public TextMeshProUGUI detailsTitle;
    public TextMeshProUGUI detailsDescription;
    public Image detailsImage;

    InventorySlot[] slots;
    Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI; //calls updateui on event

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        detailsPanel.SetActive(false);
    }

    void UpdateUI() //iterate through inventoryslots
    {
        if (Inventory.items.Count > 0) {
            detailsPanel.SetActive(true);
        } else {
            detailsPanel.SetActive(false);
        }

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

        detailsTitle.text = Inventory.items[0].name;
        detailsDescription.text = Inventory.items[0].description;
        detailsImage.sprite = Inventory.items[0].icon;
    }

    public void UpdateDetails(Item item) {
        detailsPanel.SetActive(true);
        detailsTitle.text = item.name;
        detailsDescription.text = item.description;
        detailsImage.sprite = item.icon;
    }
}
