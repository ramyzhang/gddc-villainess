using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GameTrigger : MonoBehaviour
{
    public enum TriggerType {
        Dialogue,
        Door,
        Item
    }

    private DialogueRunner dialogueRunner;
    public TriggerType triggerType;
    [HideInInspector]
    public string dialogueToStart;
    public GameObject teleportTarget;
    private bool inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    /**
    TODO: Make option to choose between trigger methods (collision, onclick, etc.) so it's a sexier gameplay experience
    **/
    void OnMouseDown() {
        if (!inRange) return;

        switch (triggerType) {
            case TriggerType.Dialogue:
                StartDialogue();
                break;
            case TriggerType.Door:
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                DoorTeleport(player, teleportTarget);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        inRange = true;
    }

    void OnTriggerExit2D(Collider2D collision) {
        inRange = false;
    }

    private void StartDialogue() {
        if (dialogueToStart == null) {
            Debug.Log($"A dialogue was not loaded in to {gameObject.name}.");
        } else if (dialogueRunner.IsDialogueRunning) {
            Debug.Log("Dialogue is already running, hold on");
        } else {
            Debug.Log("Starting dialogue!");
            FindObjectOfType<DialogueRunner>().StartDialogue(dialogueToStart);
        }
    }

    private void DoorTeleport(GameObject player, GameObject target) {
        player.GetComponent<CharacterManager>().teleportCharacter(target);
    }
}
