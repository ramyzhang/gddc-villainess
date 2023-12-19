using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GameTrigger : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    public string dialogueToStart;
    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
    TODO: Make option to choose between trigger methods (collision, onclick, etc.) so it's a sexier gameplay experience
    **/
    void OnMouseDown() {
        if (dialogueToStart == null) {
            Debug.Log($"A dialogue was not loaded in to {gameObject.name}.");
        } else if (dialogueRunner.IsDialogueRunning) {
            Debug.Log("Dialogue is already running, hold on");
        } else {
            Debug.Log("Starting dialogue!");
            FindObjectOfType<DialogueRunner>().StartDialogue(dialogueToStart);
        }
    }
}
