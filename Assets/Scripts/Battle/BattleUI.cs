using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI enemyHPText;

    public TextMeshProUGUI announcerText;
    public GameObject battleOptions;

    public void SetHP(int amount, bool isPlayer) {
        if (isPlayer) playerHPText.text = amount.ToString();
        else enemyHPText.text = amount.ToString();
    }

    public void SetAnnouncerText(string text) {
        announcerText.text = text;
    }

    public void ShowBattleOptions(bool show) {
        battleOptions.SetActive(show);
    }
}
