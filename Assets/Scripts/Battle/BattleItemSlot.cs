using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleItemSlot : MonoBehaviour
{
    public Item item;
    public int amount;

    private Button button;
    private Image buttonPanel;
    private bool isHealItem = false;
    private Battle battle;
    private BattleUI battleUI;

    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnItemClick());
        battle = FindFirstObjectByType<Battle>();
        battleUI = FindFirstObjectByType<BattleUI>();
        buttonPanel = GetComponent<Image>();
        Color buttonPanelColor = buttonPanel.color;
        buttonPanelColor.a = 0f;
        buttonPanel.color = buttonPanelColor;
    }

    public void FillSlot(HealItem newItem) {
        item = (Item)newItem;
        amount = newItem.amount;
        isHealItem = true;
        transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = item.displayName;
        transform.Find("ItemStats").GetComponent<TextMeshProUGUI>().text = newItem.amount + " HP";
    }

    public void FillSlot(WeaponItem newItem) {
        item = (Item)newItem;
        amount = newItem.damage;
        isHealItem = false;
        transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = item.displayName;
        transform.Find("ItemStats").GetComponent<TextMeshProUGUI>().text = newItem.damage + " HP";
    }

    public void ClearSlot() {
        item = null;
        Destroy(gameObject);
    }

    public void OnItemClick() {
        if (item != null) {
            if (isHealItem) {
                battle.OnHealItemSelected(item.name);
            } else {
                battle.OnWeaponSelected(item.name);
            }
        }
    }

    public void OnItemHover() {
        if (item != null) {
            battleUI.SetItemIcon(item.icon);
        }
        Color buttonPanelColor = buttonPanel.color;
        buttonPanelColor.a = 1f;
        buttonPanel.color = buttonPanelColor;
    }

    public void OnItemHoverExit() {
        if (item != null) {
            battleUI.HideItemIcon();
        }
        Color buttonPanelColor = buttonPanel.color;
        buttonPanelColor.a = 0f;
        buttonPanel.color = buttonPanelColor;
    }
}
