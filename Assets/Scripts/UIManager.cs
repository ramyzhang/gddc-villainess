using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameObject questNotification;

    [SerializeField]
    private TextMeshProUGUI newQuestTitle;
    [SerializeField]
    private TextMeshProUGUI completedQuestTitle;
    [SerializeField]
    private TextMeshProUGUI questDescription;

    // Start is called before the first frame update
    void Start()
    {
        questNotification = transform.Find("QuestNotification").gameObject;
        newQuestTitle = questNotification.transform.Find("NewQuest").GetComponent<TextMeshProUGUI>();
        completedQuestTitle = questNotification.transform.Find("CompletedQuest").GetComponent<TextMeshProUGUI>();
        questDescription = questNotification.transform.Find("QuestDescription").GetComponent<TextMeshProUGUI>();

        questNotification.SetActive(false);
        newQuestTitle.gameObject.SetActive(false);
        completedQuestTitle.gameObject.SetActive(false);
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
