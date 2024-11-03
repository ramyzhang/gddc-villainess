using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Quest Notification")]
    private GameObject questNotification;

    [SerializeField]
    private TextMeshProUGUI newQuestTitle;
    [SerializeField]
    private TextMeshProUGUI completedQuestTitle;
    [SerializeField]
    private TextMeshProUGUI questDescription;

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
        // Load in the Quest Notification UI game objects
        questNotification = transform.Find("QuestNotification").gameObject;
        newQuestTitle = questNotification.transform.Find("NewQuest").GetComponent<TextMeshProUGUI>();
        completedQuestTitle = questNotification.transform.Find("CompletedQuest").GetComponent<TextMeshProUGUI>();
        questDescription = questNotification.transform.Find("QuestDescription").GetComponent<TextMeshProUGUI>();

        questNotification.SetActive(false);
        newQuestTitle.gameObject.SetActive(false);
        completedQuestTitle.gameObject.SetActive(false);

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

    public void toggleInventory() {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void newQuestNotification(string _questTitle, string _questDescription) {
        questNotification.SetActive(true);
        newQuestTitle.gameObject.SetActive(true);
        newQuestTitle.text = newQuestTitle.text + " " + _questTitle;
        questDescription.text = _questDescription;
    }

    public void completedQuestNotification(string _questTitle, string _questDescription) {
        questNotification.SetActive(true);
        completedQuestTitle.gameObject.SetActive(true);
        completedQuestTitle.text = completedQuestTitle.text + " " + _questTitle;
        questDescription.text = _questDescription;
    }

    public void closeQuestNotification() {
        newQuestTitle.gameObject.SetActive(false);
        completedQuestTitle.gameObject.SetActive(false);
        questNotification.SetActive(false);
    }
}
