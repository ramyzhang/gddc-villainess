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
    private bool isHealItem = false;
    private Battle battle;

    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnItemClick());
        battle = FindObjectOfType<Battle>();
    }

    public void FillSlot(HealItem newItem) {
        item = (Item)newItem;
        amount = newItem.amount;
        isHealItem = true;
        transform.Find("ItemIcon").GetComponent<Image>().sprite = item.icon;
        transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = item.displayName + ": " + newItem.amount + " HP";
    }

    public void FillSlot(WeaponItem newItem) {
        item = (Item)newItem;
        amount = newItem.damage;
        isHealItem = false;
        transform.Find("ItemIcon").GetComponent<Image>().sprite = item.icon;
        transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = item.displayName + ": " + newItem.damage + " HP";
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
}
