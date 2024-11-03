using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using Yarn.Unity;

public class GameStateManager : MonoBehaviour
{
    // Static instance variable to store the singleton instance
    private static GameStateManager _instance;

    /**
    Public property to access the instance.
    Access the GameStateManager singleton instance like so:

        GameStateManager gameStateManager = GameStateManager.Instance;
    **/
    public static GameStateManager Instance
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

    // TODO: create GET functions for current state variables

    // Game state management variables
    public int currentStateIndex = 0;
    [Header("CONSTANTS BELOW - PLEASE DO NOT MODIFY")]
    public GameObject NPCPrefab;
    public GameObject ItemPrefab;

    [System.Serializable]
    public struct Location {
        public string locationName;
        public Vector3 locationPosition;
    }
    public Location[] locations;
    public GameState currentState { get; private set; }
    private MainCamera gameGamera;
    private GameObject currentPlayer;
    private DialogueRunner dialogueRunner;
    private List<GameObject> toDestroy = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        gameGamera = GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>();

        // Instantiate current state
        UpdateState(currentStateIndex);
    }

    // TODO: it might be better to be using one of yarn's built in event handlers instead here
    [YarnCommand("updateState")]
    public void UpdateState(int newStateIndex) {
        Debug.Log($"New state index: {newStateIndex}");

        string statePath = $"Constants/States/State{newStateIndex}";
        GameState newState = Resources.Load<GameState>(statePath);

        if (newState == null) {
            Debug.Log($"State {newStateIndex} doesn't exist...");
            return;
        }

        // 0. Teardown if there was a previous state
        if (currentState != null) {
            Debug.Log("Tearing down previous state...");
            TearDown(newState);
        }

        // 1. Move player character to correct location and make it the player
        // TODO: deal with other player related components as well
        currentPlayer = GameObject.Find(newState.playerCharacter);
        currentPlayer.tag = "Player";
        if (!newState.doNotUpdateLocation) {
            currentPlayer.transform.position = newState.initLocation;
        }
        currentPlayer.AddComponent<PlayerMovement>();
        currentPlayer.GetComponent<CharacterManager>().FlipCharacter(newState.playerIsFacingRight);

        // 2. Instantiate all NPCs
        // TODO: do something about loading in dialogue for the NPCs
        for (int i = 0; i < newState.availableNPCs.Length; i++) {
            GameObject currentNPC = GameObject.Find(newState.availableNPCs[i].NPCCharacter);
            if (currentNPC == null) {
                Debug.Log($"Failed to find NPC {newState.availableNPCs[i].NPCCharacter}. Instantiating...");
                currentNPC = Instantiate(NPCPrefab, newState.availableNPCs[i].NPCLocation, Quaternion.identity);
                currentNPC.name = newState.availableNPCs[i].NPCCharacter;
                currentNPC.GetComponent<AnimatorReskinner>().ReSkin((Character)System.Enum.Parse(typeof(Character), newState.availableNPCs[i].NPCCharacter));
                toDestroy.Add(currentNPC);
            } else {
                currentNPC.transform.position = newState.availableNPCs[i].NPCLocation;
            }
            currentNPC.GetComponent<CharacterManager>().FlipCharacter(newState.availableNPCs[i].isFacingRight);
        }

        // 3. Instantiate all collectibles
        for (int j = 0; j < newState.collectibles.Length; j++) {
            GameObject collectible = Instantiate(ItemPrefab, newState.collectibles[j].collectibleLocation, Quaternion.identity);
            collectible.GetComponent<ItemTrigger>().item = newState.collectibles[j].item;
            Debug.Log(newState.collectibles[j].item.icon.name);
            if (collectible.GetComponent<SpriteRenderer>().sprite) {
                collectible.GetComponent<SpriteRenderer>().sprite = newState.collectibles[j].item.icon;
            }
            toDestroy.Add(collectible);
        }

        // 4. Game triggers (create a tag for these)
        // TODO: do something to handle if it's just a game state trigger, not a dialogue one
        for (int k = 0; k < newState.gameEventTriggers.Length; k++) {
            GameTrigger gt = GameObject.Find(newState.gameEventTriggers[k]).GetComponentInChildren<GameTrigger>();
            if (gt.triggerType == TriggerType.Dialogue) {
                DialogueTrigger dt = gt.gameObject.GetComponent<DialogueTrigger>();
                if (dt) {
                    dt.dialogueToStart = $"{newState.gameEventTriggers[k]}_{newStateIndex}";
                }
            }
        }

        // 5. Switch camera subject
        gameGamera.playerCharacter = currentPlayer;

        // 6. Trigger relevant cutscene dialogue
        string cutsceneName = $"Cutscene_{newStateIndex}";
        if (dialogueRunner.NodeExists(cutsceneName)) {
            dialogueRunner.StartDialogue(cutsceneName);
        } else {
            Debug.Log($"No opening cutscene in Game State {newStateIndex}");
        }

        // 9. Update state
        currentState = newState;
        currentStateIndex = newStateIndex;
    }

    /**
    State teardown involves:
    0. Stop any running dialogue
    1. Handle previous player (destroy PlayerMovement component and remove Player tag if character is changing)
    2. Destroy all tracked game objects in toDestroy list
    3. Move all NPCs back to loading dock position (-325, 80, 0)
    4. Clear dialogue references from all GameTriggers
    5. Clean up all Sticker prefab clones (except original prefab)
    **/
    private void TearDown(GameState newState) {
        // 0. Dismiss and stop current dialogue
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue("Stop");

        // 1. Remove previous player
        if (!newState.playerCharacter.Equals(currentState.playerCharacter)) {
            GameObject prevPlayer = GameObject.FindWithTag("Player");
            if (prevPlayer == null) {
                Debug.Log("No previous player has existed!");
            } else {
                prevPlayer.tag = "Character";
                Destroy(prevPlayer.GetComponent<PlayerMovement>());
            }
        }
        
        // 2. Destroy all game objects in toDestroy
        foreach (GameObject killedObject in toDestroy) {
            Destroy(killedObject);
        }

        toDestroy.Clear();

        // 3. Move all NPCs in this gamestate back to the loading dock (or destroy them)
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject npc in npcs) {
            npc.transform.position = locations.Last().locationPosition;
        }

        // 4. Remove dialogues stored in game triggers
        GameObject[] diaTrigger = GameObject.FindGameObjectsWithTag("GameTrigger");
        foreach (GameObject gt in diaTrigger) {
            DialogueTrigger trigger = gt.GetComponent<DialogueTrigger>();
            if (trigger) {
                trigger.dialogueToStart = null;
            }
        }

        // 5. Clean up all stickers (prefab clones)
        GameObject[] stickers = GameObject.FindGameObjectsWithTag("Sticker");
        foreach (GameObject stkr in stickers) {
            if (!stkr.name.Equals("Sticker")) {
                // Don't delete the original instance
                Destroy(stkr);
            }
        }
    }
}
