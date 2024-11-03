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
    private GameObject inventoryUI;
    [SerializeField]
    private GameObject inventoryButton;
    [SerializeField]
    private GameObject inventoryPanel;

    // Start is called before the first frame update
    void Start()
    {
        // Load in the Inventory UI game objects
        inventoryUI = transform.Find("Inventory").gameObject;
        inventoryButton = inventoryUI.transform.Find("InventoryButton").gameObject;
        inventoryPanel = inventoryUI.transform.Find("InventoryScreen").gameObject;

        inventoryPanel.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    public void ToggleInventory() {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
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
