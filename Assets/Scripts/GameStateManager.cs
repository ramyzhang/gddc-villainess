using System.Collections;
using System.Collections.Generic;
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
    public GameObject NPCPrefab;
    private GameState currentState;
    private MainCamera gameGamera;
    private GameObject currentPlayer;
    private DialogueRunner dialogueRunner;
    private DialogueViewBase dialogueViewBase;
    private List<GameObject> toDestroy = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        dialogueViewBase = FindObjectOfType<DialogueViewBase>();
        gameGamera = GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>();

        // Instantiate current state
        updateState(currentStateIndex);
    }

    public (Sprite, Vector3) GETSticker(string name) {
        foreach (GameState.StickerState stck in currentState.stickers) {
            if (stck.stickerName.Equals(name)) {
                return (stck.stickerSprite, stck.stickerLocation);
            }
        }
        Debug.Log("This sticker should not exist in this gamestate!");
        return (null, new Vector3(0f, 0f, 0f));
    }

    // TODO: it might be better to be using one of yarn's built in event handlers instead here
    [YarnCommand("updateState")]
    public void updateState(int newStateIndex) {
        Debug.Log($"New state index: {newStateIndex}");
        int prevStateIndex = currentStateIndex;

        string statePath = $"Constants/States/State{newStateIndex}";
        GameState newState = Resources.Load<GameState>(statePath);

        if (newState == null) {
            Debug.Log($"State {newStateIndex} doesn't exist...");
            return;
        }

        // 0. Teardown if there was a previous state
        if (currentState != null) {
            Debug.Log("Tearing down previous state...");
            tearDown(newState);
        }

        // 1. Move player character to correct location and make it the player
        // TODO: deal with other player related components as well
        currentPlayer = GameObject.Find(newState.playerCharacter);
        currentPlayer.tag = "Player";
        currentPlayer.transform.position = newState.initLocation;
        currentPlayer.AddComponent<PlayerMovement>();
        currentPlayer.GetComponent<CharacterManager>().flipCharacter(newState.playerIsFacingRight);

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
            currentNPC.GetComponent<CharacterManager>().flipCharacter(newState.availableNPCs[i].isFacingRight);
        }

        // 3. Instantiate all collectibles
        for (int j = 0; j < newState.collectibles.Length; j++) {
            // TODO: Check if collectible in fact exists (maybe with an enum?)
            Instantiate(newState.collectibles[j].collectible, newState.collectibles[j].collectibleLocation, Quaternion.identity);
        }

        // 4. Game triggers (create a tag for these)
        // TODO: do something to handle if it's just a game state trigger, not a dialogue one
        for (int k = 0; k < newState.gameEventTriggers.Length; k++) {
            GameTrigger gt = GameObject.Find(newState.gameEventTriggers[k]).GetComponent<GameTrigger>();
            Debug.Log("game trigger: " + gt + $" {newState.gameEventTriggers[k]}_{newStateIndex}");
            gt.dialogueToStart = $"{newState.gameEventTriggers[k]}_{newStateIndex}";
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
    1. Handle previous player (move code from updateState)
    2. Move all NPCs on the scene back to loading dock
    3. Remove all dialogues loaded into previous gametriggers (if they're diff from the next state's)
    4. TODO: Clean up all collectibles (while checking game logic)
    5. Clean up all stickers
    **/
    private void tearDown(GameState newState) {
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

        // 2. Move all NPCs in this gamestate back to the loading dock (or destroy them)
        foreach (GameObject toDestroyNPC in toDestroy) {
            Destroy(toDestroyNPC);
        }

        toDestroy.Clear();

        GameObject[] npcs = GameObject.FindGameObjectsWithTag("Character");
        foreach (GameObject npc in npcs) {
            npc.transform.position = new Vector3(-325f, 80f, 0f);
        }

        // 3. Remove dialogues stored in game triggers
        GameObject[] gameTriggers = GameObject.FindGameObjectsWithTag("GameTrigger");
        foreach (GameObject gt in gameTriggers) {
            GameTrigger trigger = gt.GetComponent<GameTrigger>();
            trigger.dialogueToStart = null;
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
