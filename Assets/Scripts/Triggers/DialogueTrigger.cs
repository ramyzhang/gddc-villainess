using UnityEngine;
using Yarn.Unity;

public class DialogueTrigger : GameTrigger
{
    private DialogueRunner dialogueRunner;
    public string dialogueToStart;

    // Start is called before the first frame update
    void Start()
    {
        triggerType = TriggerType.Dialogue;
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
    }

    /**
    TODO: Make option to choose between trigger methods (collision, onclick, etc.) so it's a sexier gameplay experience
    **/
    public override void Interact() {
        Debug.Log("I was clicked on.");
        if (dialogueToStart == null || dialogueToStart == "") {
            Debug.Log($"A dialogue was not loaded in to {gameObject.name}.");
        } else if (dialogueRunner.IsDialogueRunning) {
            Debug.Log("Dialogue is already running, hold on");
        } else {
            Debug.Log("Starting dialogue!");
            FindFirstObjectByType<DialogueRunner>().StartDialogue(dialogueToStart);
        }
    }
}