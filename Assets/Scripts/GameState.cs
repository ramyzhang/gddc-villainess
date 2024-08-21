using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameState : ScriptableObject
{
    /**
    This is a game state class. Use this class to create game state objects by right-clicking in the Project file explorer in the Unity editor. Store all game state objects in the Resources/Constants/States folder.
    **/

    [System.Serializable]
    public struct NPCState {
        public string NPCCharacter;
        public Vector3 NPCLocation;
        public bool isFacingRight;
    }

    [System.Serializable]
    public struct CollectibleState {
        public GameObject collectible;
        public Vector3 collectibleLocation;
    }

    /**
    Stickers are any non-interactable environment objects that need to be dynamically swapped in and out depending on game state
    **/
    [System.Serializable]
    public struct StickerState {
        public string stickerName;
        public Sprite stickerSprite;
        public Vector3 stickerLocation;
    }

    /**
    Triggers are any interactable GameObjects that either start dialogue or update some kind of game state upon collision on upon click or whateverrrr
    **/
    [System.Serializable]
    public struct TriggerState {
        public string triggerName;
        public string dialogueName;
    }

    public int stateIndex;
    public bool doNotUpdateLocation;
    public Vector3 initLocation;
    public NPCState[] availableNPCs;
    public CollectibleState[] collectibles;
    public StickerState[] stickers;
    public string[] gameEventTriggers;
    public bool playerIsFacingRight;
    public string playerCharacter;
    public bool[] battlesWon;
    // TODO: Better to find some way to check this programmatically inside GameStateManager instead of having this variable

    public GameState(int _stateIndex, bool _doNotUpdateLocation, Vector3 _initLocation, NPCState[] _availableNPCs, CollectibleState[] _collectibles, StickerState[] _stickers, string[] _gameEventTriggers, bool _playerIsFacingRight, string _playerCharacter, bool _hasCutscene, bool[] _battlesWon) {
        stateIndex = _stateIndex;
        doNotUpdateLocation = _doNotUpdateLocation;
        initLocation = _initLocation;   
        availableNPCs = _availableNPCs;
        collectibles = _collectibles;
        stickers = _stickers;
        gameEventTriggers = _gameEventTriggers;
        playerIsFacingRight = _playerIsFacingRight;
        playerCharacter = _playerCharacter;
        battlesWon = _battlesWon;
    }
}
