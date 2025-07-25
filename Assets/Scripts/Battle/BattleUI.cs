using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleUI : MonoBehaviour
{
    [Header("Announcer")]
    public TextMeshProUGUI announcerText;

    [Space(10)]
    [Header("HP Bars")]
    public TextMeshProUGUI playerHPText;
    public TextMeshProUGUI enemyHPText;

    [Space(10)]
    [Header("Battle Options")]
    public GameObject battleOptions;
    public GameObject weaponOptions;
    public GameObject healItemOptions;
    public GameObject battleItemPrefab;


    public void SetHP(int amount, bool isPlayer) {
        if (isPlayer) playerHPText.text = amount.ToString();
        else enemyHPText.text = amount.ToString();
    }

    public void SetAnnouncerText(string text) {
        announcerText.text = text;
    }

    public void PopulateBattleItemUI(List<WeaponItem> items) {
        foreach (WeaponItem item in items) {
            GameObject itemSlot = Instantiate(battleItemPrefab, weaponOptions.transform.Find("Viewport/Content").transform);
            itemSlot.GetComponent<BattleItemSlot>().FillSlot(item);
        }
    }

    public void PopulateBattleItemUI(List<HealItem> items) {
        foreach (HealItem item in items) {
            GameObject itemSlot = Instantiate(battleItemPrefab, healItemOptions.transform.Find("Viewport/Content").transform);
            itemSlot.GetComponent<BattleItemSlot>().FillSlot(item);
        }
    }

    public void ShowBattleOptions(bool show) {
        battleOptions.SetActive(show);
    }

    public void ShowWeaponOptions(bool show) {
        weaponOptions.SetActive(show);
    }

    public void ShowHealItemOptions(bool show) {
        healItemOptions.SetActive(show);
    }
}
