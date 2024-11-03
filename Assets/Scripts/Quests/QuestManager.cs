using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class QuestManager : MonoBehaviour
{
    // Static instance variable to store the singleton instance
    private static QuestManager _instance;

    /**
    Public property to access the instance.
    Access the QuestManager singleton instance like so:

        QuestManager questManager = QuestManager.Instance;
    **/
    public static QuestManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        // Ensure there is only one instance of the GameStateManager
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject); // Destroy the duplicate instance
        }
        else
        {
            _instance = this; // Set the instance to this object
            DontDestroyOnLoad(this.gameObject); // Prevent it from being destroyed when loading scenes
        }
    }

    [SerializeField]
    private List<Quest> quests = new List<Quest>();

    private int currQuestId;
    private UIManager uiManager;

    void Start() {
        currQuestId = 0;
        uiManager = GameObject.Find("UI Canvas").GetComponent<UIManager>();
    }

    [YarnCommand("newQuest")]
    public void newQuest(int questId) {
        Debug.Log("Calling newQuest with questId: " + questId);
        if (questId >= quests.Count) {
            Debug.Log("Quest ID does not exist!");
        } else if ((questId != currQuestId + 1) && (questId == 0 && currQuestId != 0)) {
            Debug.Log("Quest IDs must be sequential!");
        } else if (!quests[currQuestId].isCompleted && questId != 0) {
            Debug.Log("Previous quest must be completed before starting a new quest!");
        } else {
            currQuestId = questId;
            uiManager.NewQuestNotification(quests[currQuestId].questTitle, quests[currQuestId].questDescription);
        }
    }

    public Quest getQuest(int questId) {
        return quests[questId];
    }

    [YarnCommand("completeQuest")]
    public void completeQuest(int questId) {
        if (questId != currQuestId) {
            Debug.Log("Quest IDs must be sequential!");
        } else {
            quests[currQuestId].isCompleted = true;
            uiManager.CompletedQuestNotification(quests[currQuestId].questTitle, quests[currQuestId].questDescription);
        }
    }
}
