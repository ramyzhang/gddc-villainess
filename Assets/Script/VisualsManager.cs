using System.Collections;
using System.Collections.Generic;
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
            screenEffect // the method to run
        );
        dr.AddCommandHandler<string>(
            "addSticker",     // the name of the command
            addSticker // the method to run
        );
        dr.AddCommandHandler<string>(
            "triggerCG",     // the name of the command
            triggerCG // the method to run
        );
        dr.AddCommandHandler<GameObject, GameObject>(
            "moveCharacter",     // the name of the command
            moveCharacter // the method to run
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void addSticker(string stickerName) {
        (Sprite, Vector3) sticker = gameStateManager.GETSticker(stickerName);

        if (sticker.Item1 != null) {
            GameObject newSticker = Instantiate(stickerPrefab, sticker.Item2, Quaternion.identity);
            StickerManager sm = newSticker.GetComponent<StickerManager>();
            sm.stickyStick(sticker.Item1, sticker.Item2, stickerName);
        }
    }

    [YarnCommand("removeSticker")]
    public void removeSticker(string stickerName) {
        GameObject sticker = GameObject.Find(stickerName);
        Destroy(sticker);
    }

    /**
    TODO: enum for all CGname options
    **/
    private void triggerCG(string CGname) {
        Sprite newCG = Resources.Load<Sprite>($"CGs/{CGname}");
        cgc.setupCG(newCG);
    }

    [YarnCommand("takedownCG")]
    public static void takedownCG() {
        cgc.removeCG();
    }

    /**
    TODO: enum for all effect options??
    **/
    public Coroutine screenEffect(string effect) {
        if (effect.Equals("screenShake")) {
            Debug.Log("Playing sceen shake...");
            return StartCoroutine(playerCamera.screenShake());
        } else if (effect.Equals("fadeBlack")) {
            Debug.Log("Fading to black right now...");
            return StartCoroutine(cgc.fadeBlack());
        }
        return null;
    }

    public Coroutine moveCharacter(GameObject chara, GameObject target) {
        CharacterManager cm = chara.GetComponent<CharacterManager>();
        return StartCoroutine(cm.moveCharacter(target));
    }
}
