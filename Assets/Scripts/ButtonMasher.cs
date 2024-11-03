using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMasher : MonoBehaviour
{
    [Header("Minigame Variables")]
    [SerializeField] private float minigameLength;
    [SerializeField] private int winMashesThreshold;
    private int buttonMashes;
    private float timeRemaining;
    private bool minigameRunning;

    [Header("Minigame UI")]
    [SerializeField] private TMP_Text timer;
    [SerializeField] private TMP_Text score;

    [Header("Minigame Animation")]
    [SerializeField] private GameObject loveInterest;
    [SerializeField] private GameObject secondLoveInterest;
    private CharacterManager loveInterestCM;
    private CharacterManager secondLoveInterestCM;

    WaitForSecondsRealtime delay = new WaitForSecondsRealtime(3);

    void Start()
    {
        UpdateDisplays();
        score.text = "";
        minigameRunning = false;
        timeRemaining = minigameLength;
        buttonMashes = 0;
        ChangeButtonFunc("WRESTLE!", MashButton, false);
        loveInterestCM = loveInterest.GetComponent<CharacterManager>();
        secondLoveInterestCM = secondLoveInterest.GetComponent<CharacterManager>();
        secondLoveInterestCM.FlipCharacter(false);
    }

    void Update()
    {
        if (minigameRunning && timeRemaining > 0) // Game is running
        {
            timeRemaining -= Time.deltaTime;
            UpdateDisplays();
            loveInterestCM.ChangeAnimationSpeed("armSpinSpeed", buttonMashes*3/winMashesThreshold);
        }
        else if (minigameRunning && timeRemaining <= 0) // Game finished
        {
            minigameRunning = false;
            timeRemaining = 0;
            UpdateDisplays();
            ResetLIs();
        }
        else // Game hasn't started yet
        {
            minigameRunning = false;
        }
    }

    IEnumerator DelayButtonChange(bool delayChange)
    {
        if (delayChange)
        {
            this.gameObject.GetComponent<Button>().enabled = false;
            this.gameObject.GetComponent<Image>().enabled = false;
            this.gameObject.GetComponentInChildren<TMP_Text>().enabled = false;
            yield return delay;
            this.gameObject.GetComponent<Button>().enabled = true;
            this.gameObject.GetComponent<Image>().enabled = true;
            this.gameObject.GetComponentInChildren<TMP_Text>().enabled = true;
        }
    }

    private void MashButton()
    {
        if (minigameRunning)
        {
            buttonMashes++;
        }
        else
        {
            minigameRunning = true;
            AnimateLIs();
        }
    }

    private void RestartMinigame()
    {
        minigameRunning = false;
        timeRemaining = minigameLength;
        buttonMashes = 0;
        UpdateDisplays();
        score.text = "";
        ChangeButtonFunc("WRESTLE!", MashButton, false);
    }

    private void UpdateDisplays()
    {
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        float milliSeconds = (timeRemaining % 1) * 100;
        timer.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);
        if (minigameRunning)
        {
            score.text = buttonMashes.ToString();
        }
        else if (timeRemaining <= 0)
        {
            if (buttonMashes > winMashesThreshold)
            {
                score.text = "Mashes: " + buttonMashes.ToString() + "\nYou Win!";
            }
            else
            {
                score.text = "Mashes: " + buttonMashes.ToString() + "\nYou Lose!";
            }
            ChangeButtonFunc("Try Again!", RestartMinigame, true);
        }
    }

    private void AnimateLIs()
    {
        loveInterestCM.AnimateCharacter("wrestle");
        loveInterestCM.AnimateCharacter("arm_spin", 1);
        secondLoveInterestCM.AnimateCharacter("wrestle");
        secondLoveInterestCM.AnimateCharacter("arm_spin", 1);
    }

    private void ResetLIs()
    {
        loveInterestCM.AnimateCharacter("idle");
        loveInterestCM.AnimateCharacter("none", 1);
        secondLoveInterestCM.AnimateCharacter("idle");
        secondLoveInterestCM.AnimateCharacter("none", 1);
    }

    private void ChangeButtonFunc(string buttonText, UnityEngine.Events.UnityAction call, bool delayChange)
    {
        this.gameObject.GetComponentInChildren<TMP_Text>().text = buttonText;
        this.GetComponent<Button>().onClick.RemoveAllListeners();
        this.GetComponent<Button>().onClick.AddListener(call);
        StartCoroutine(DelayButtonChange(delayChange));
    }
}
