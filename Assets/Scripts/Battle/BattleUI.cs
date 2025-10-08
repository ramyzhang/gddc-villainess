using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    public Battle battle;

    [Header("Announcer")]
    public GameObject announcerPanel;
    public TextMeshProUGUI announcerText;

    [Space(10)]
    [Header("Battle Item Icon")]
    public GameObject battleItemIconBox;
    public Image battleItemIconImage;

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

    void Start() {
        announcerPanel.SetActive(false);
        announcerText = announcerPanel.GetComponentsInChildren<TextMeshProUGUI>()[0];

        battleItemIconBox.SetActive(false);
        battleItemIconImage = battleItemIconBox.GetComponentsInChildren<Image>()[1];
        battleItemIconImage.sprite = null;
        battleItemIconImage.color = new Color(1f, 1f, 1f, 0f); // transparent
    }

    public void SetHP(int amount, bool isPlayer) {
        if (isPlayer) playerHPText.text = amount.ToString();
        else enemyHPText.text = amount.ToString();
    }

    public void SetAnnouncerText(string text) {
        announcerText.text = text;

        // set announcer panel active for 1 second
        announcerPanel.SetActive(true);
        Invoke("HideAnnouncerPanel", 1f);
    }

    public void SetItemIcon(Sprite itemIcon) {
        battleItemIconImage.sprite = itemIcon;
        battleItemIconImage.color = new Color(1f, 1f, 1f, 1f);
    }

    public void HideItemIcon() {
        battleItemIconImage.sprite = null;
        battleItemIconImage.color = new Color(1f, 1f, 1f, 0f);
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
        battleItemIconBox.SetActive(!show);
        battleOptions.SetActive(show);
    }

    public void ShowWeaponOptions(bool show) {
        weaponOptions.SetActive(show);
        battleItemIconBox.SetActive(true);
    }

    public void ShowHealItemOptions(bool show) {
        healItemOptions.SetActive(show);
        battleItemIconBox.SetActive(true);
    }

    public void HideAnnouncerPanel() {
        announcerPanel.SetActive(false);
    }
}
