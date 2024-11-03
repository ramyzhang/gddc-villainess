using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class VisualsManager : MonoBehaviour
{
    // TODO: make this a singleton
    public static MainCamera playerCamera;
    public GameObject stickerPrefab;
    public static CGController cgc;
    public GameStateManager gameStateManager;
    private DialogueRunner dr;

    void Awake()
    {
        playerCamera = GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>();

        stickerPrefab = GameObject.Find("Sticker");

        cgc = GameObject.Find("CG Canvas").GetComponent<CGController>();

        gameStateManager = GameStateManager.Instance;

        dr = GameObject.FindObjectOfType<DialogueRunner>();
        dr.AddCommandHandler<string>(
            "screenEffect",     // the name of the command
            ScreenEffect // the method to run
        );
        dr.AddCommandHandler<string>(
            "addSticker",     // the name of the command
            AddSticker // the method to run
        );
        dr.AddCommandHandler<string>(
            "triggerCG",     // the name of the command
            TriggerCG // the method to run
        );
        dr.AddCommandHandler<GameObject, GameObject>(
            "moveCharacter",     // the name of the command
            MoveCharacter // the method to run
        );
    }
    
    private void AddSticker(string stickerName) {
        GameState.StickerState sticker = gameStateManager.currentState.stickers.FirstOrDefault(s => s.stickerName == stickerName);

        if (sticker.stickerSprite != null) {
            GameObject newSticker = Instantiate(stickerPrefab, sticker.stickerLocation, Quaternion.identity);
            StickerManager sm = newSticker.GetComponent<StickerManager>();
            sm.stickyStick(sticker.stickerSprite, sticker.stickerLocation, stickerName);
        }
    }

    [YarnCommand("removeSticker")]
    public void RemoveSticker(string stickerName) {
        GameObject sticker = GameObject.Find(stickerName);
        Destroy(sticker);
    }

    /**
    TODO: enum for all CGname options
    **/
    private void TriggerCG(string CGname) {
        Sprite newCG = Resources.Load<Sprite>($"CGs/{CGname}");
        cgc.setupCG(newCG);
    }

    [YarnCommand("takedownCG")]
    public static void TakedownCG() {
        cgc.removeCG();
    }

    /**
    TODO: enum for all effect options??
    **/
    public Coroutine ScreenEffect(string effect) {
        if (effect.Equals("screenShake")) {
            Debug.Log("Playing sceen shake...");
            return StartCoroutine(playerCamera.screenShake());
        } else if (effect.Equals("fadeBlack")) {
            Debug.Log("Fading to black right now...");
            return StartCoroutine(cgc.fadeBlack());
        }
        return null;
    }

    public Coroutine MoveCharacter(GameObject chara, GameObject target) {
        CharacterManager cm = chara.GetComponent<CharacterManager>();
        return StartCoroutine(cm.MoveCharacter(target));
    }
}
