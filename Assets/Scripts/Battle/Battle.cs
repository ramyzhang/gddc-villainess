using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle : MonoBehaviour
{
    public BattleState state;
    public BattleUnit playerUnit;
    public BattleUnit enemyUnit;
    public BattleUI battleUI;

    // Inventory
    public Dictionary<string, HealItem> healItems = new Dictionary<string, HealItem>();
    public Dictionary<string, WeaponItem> attackItems = new Dictionary<string, WeaponItem>();

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;

        // Set up inventory
        foreach (Item item in Inventory.items) {
            if (item is HealItem) {
                healItems.Add(item.name, (HealItem)item);
            } else if (item is WeaponItem) {
                attackItems.Add(item.name, (WeaponItem)item);
            }
        }
        
        battleUI.PopulateBattleItemUI(attackItems.Values.ToList());
        battleUI.PopulateBattleItemUI(healItems.Values.ToList());

        StartBattle();
    }

    public void StartBattle() {
        state = BattleState.PLAYER_TURN;
        battleUI.SetHP(playerUnit.currentHP, true);
        battleUI.SetHP(enemyUnit.currentHP, false);
        battleUI.SetAnnouncerText("Choose an action");
        battleUI.ShowBattleOptions(true);
        battleUI.ShowWeaponOptions(false);
        battleUI.ShowHealItemOptions(false);

        // TODO: Remove this eventually
        enemyUnit.gameObject.GetComponent<CharacterManager>().FlipCharacter(true);
    }

    public void PlayerTurn(string option) {
        playerUnit.isDefending = false; // Reset defending

        BattleOptions battleOption = (BattleOptions)Enum.Parse(typeof(BattleOptions), option);

        switch (battleOption) {
            case BattleOptions.ATTACK:
                StartCoroutine(PlayerTurn(playerUnit.baseAttack));
                break;
            case BattleOptions.SPECIAL:
                battleUI.ShowWeaponOptions(true);
                break;
            case BattleOptions.HEALITEM:
                battleUI.ShowHealItemOptions(true);
                break;
            case BattleOptions.DEFEND:
                StartCoroutine(PlayerDefend());
                break;
        }
    }

    public void OnWeaponSelected(string moveName) {
        battleUI.ShowWeaponOptions(false);
        StartCoroutine(PlayerSpecialAttack(moveName));
    }

    public void OnHealItemSelected(string itemName) {
        battleUI.ShowHealItemOptions(false);
        StartCoroutine(PlayerHeal(itemName));
    }

    IEnumerator EnemyTurn() {
        state = BattleState.ENEMY_TURN;
        battleUI.ShowBattleOptions(false);
        battleUI.SetAnnouncerText(enemyUnit.unitName + " is attacking");
        yield return new WaitForSeconds(2f);

        if (playerUnit.TakeDamage(enemyUnit.baseAttack)) {
            battleUI.SetAnnouncerText(playerUnit.unitName + " has fainted");
            yield return new WaitForSeconds(2f);
            EndBattle(BattleState.LOST);
        } else {
            battleUI.SetAnnouncerText("You have " + playerUnit.currentHP + " HP remaining");
            battleUI.SetHP(playerUnit.currentHP, true);
            state = BattleState.PLAYER_TURN;
            battleUI.ShowBattleOptions(true);
        }
    }

    IEnumerator PlayerTurn(int damage) {
        state = BattleState.PLAYER_TURN;
        battleUI.ShowBattleOptions(false);
        battleUI.SetAnnouncerText("You attacked " + enemyUnit.unitName + " for " + damage + " damage");
        yield return new WaitForSeconds(1f);
        if (enemyUnit.TakeDamage(damage)) {
            battleUI.SetHP(enemyUnit.currentHP, false);
            battleUI.SetAnnouncerText(enemyUnit.unitName + " has fainted!");
            yield return new WaitForSeconds(2f);
            EndBattle(BattleState.WON);
        } else {
            battleUI.SetHP(enemyUnit.currentHP, false);
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerSpecialAttack(string moveName) {
        state = BattleState.PLAYER_TURN;
        battleUI.ShowBattleOptions(false);
        // TODO: Hard code some animation for special moves
        battleUI.SetAnnouncerText(playerUnit.unitName + " used Special Attack: " + moveName);
        yield return new WaitForSeconds(2f);
        int damage = attackItems[moveName].damage;
        StartCoroutine(PlayerTurn(damage));
    }

    IEnumerator PlayerHeal(string itemName) {
        state = BattleState.PLAYER_TURN;
        battleUI.ShowBattleOptions(false);
        battleUI.SetAnnouncerText(playerUnit.unitName + " used Heal Item: " + itemName);
        yield return new WaitForSeconds(2f);
        int healAmount = healItems[itemName].amount;
        playerUnit.Heal(healAmount);
        battleUI.SetHP(playerUnit.currentHP, true);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerDefend() {
        playerUnit.isDefending = true;
        battleUI.ShowBattleOptions(false);
        battleUI.SetAnnouncerText(playerUnit.unitName + " is defending!");
        yield return new WaitForSeconds(2f);
        // TODO: Hard code some animation for defending
        StartCoroutine(EnemyTurn());
    }

    public void EndBattle(BattleState result) {
        state = result;
        battleUI.SetAnnouncerText("Battle over");
        battleUI.ShowBattleOptions(false);
    }
}
