using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Quest Notification")]
    [SerializeField]
    private GameObject questNotificationPrefab;
    private GameObject questNotification;

    [Space(10)]
    [Header("Inventory")]
    [SerializeField]
    private GameObject inventoryButton;
    [SerializeField]
    private GameObject inventoryPanel;
    [SerializeField]
    private GameObject itemNotificationPrefab;
    private GameObject itemNotification;
    private GameObject inventoryUI;


    // Start is called before the first frame update
    void Start()
    {
        // Load in the Inventory UI game objects
        inventoryUI = transform.Find("Inventory").gameObject;
        inventoryButton = inventoryUI.transform.Find("InventoryButton").gameObject;
        inventoryPanel = inventoryUI.transform.Find("InventoryScreen").gameObject;

        inventoryPanel.SetActive(false);

        Inventory.instance.onNewItemCallback += NewItemNotification; //calls updateui on event
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    public void ToggleInventory() {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void NewItemNotification(Item item) {
        itemNotification = Instantiate(itemNotificationPrefab, transform);

        TextMeshProUGUI itemName = itemNotification.transform.Find("NewItem").GetComponent<TextMeshProUGUI>();
        itemName.text = itemName.text + " " + item.displayName;

        TextMeshProUGUI itemDescription = itemNotification.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>();
        itemDescription.text = item.description;

        Image itemImage = itemNotification.transform.Find("ItemImage").GetComponent<Image>();
        itemImage.sprite = item.icon;
    }

    public void CloseItemNotification() {
        Destroy(itemNotification);
    }

    public void NewQuestNotification(string _questTitle, string _questDescription) {
        questNotification = Instantiate(questNotificationPrefab, transform);

        TextMeshProUGUI newQuestTitle = questNotification.transform.Find("NewQuest").GetComponent<TextMeshProUGUI>();
        questNotification.transform.Find("CompletedQuest").gameObject.SetActive(false);
        TextMeshProUGUI questDescription = questNotification.transform.Find("QuestDescription").GetComponent<TextMeshProUGUI>();

        newQuestTitle.text = newQuestTitle.text + " " + _questTitle;
        questDescription.text = _questDescription;
    }

    public void CompletedQuestNotification(string _questTitle, string _questDescription) {
        questNotification = Instantiate(questNotificationPrefab, transform);

        TextMeshProUGUI completedQuestTitle = questNotification.transform.Find("CompletedQuest").GetComponent<TextMeshProUGUI>();
        questNotification.transform.Find("NewQuest").gameObject.SetActive(false);
        TextMeshProUGUI questDescription = questNotification.transform.Find("QuestDescription").GetComponent<TextMeshProUGUI>();

        completedQuestTitle.text = completedQuestTitle.text + " " + _questTitle;
        questDescription.text = _questDescription;
    }

    public void CloseQuestNotification() {
        Destroy(questNotification);
    }
}
