using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;

public class Battle : MonoBehaviour
{
    public BattleState myState;
    public BattleUnit myPlayerUnit;
    public BattleUnit myEnemyUnit;

    // Inventory
    public Dictionary<string, HealItem> myHealItems = new();
    public Dictionary<string, WeaponItem> myAttackItems = new();

    // Events
    public UnityEvent myInventoryLoadedEvent;
    public UnityEvent myBattleStartEvent;
    public UnityEvent myRequestShowWeaponsEvent;
    public UnityEvent myRequestShowHealsEvent;

    // Start is called before the first frame update
    void Start()
    {
        myInventoryLoadedEvent ??= new UnityEvent();
        myBattleStartEvent ??= new UnityEvent();
        myRequestShowWeaponsEvent ??= new UnityEvent();
        myRequestShowHealsEvent ??= new UnityEvent();

        myState = BattleState.START;

        // Set up inventory
        for (int i = 0; i < Inventory.GetCount(); i++)
        {
            Item item = Inventory.GetItem(i);
            if (item is HealItem healItem) {
                myHealItems.Add(healItem.name, healItem);
            } else if (item is WeaponItem weaponItem) {
                myAttackItems.Add(weaponItem.name, weaponItem);
            }
        }

        myInventoryLoadedEvent.Invoke();

        StartBattle();
    }

    public void StartBattle() {
        myState = BattleState.PLAYER_TURN;
        // battleUI.SetHP(myPlayerUnit.currentHP, true);
        // battleUI.SetHP(myEnemyUnit.currentHP, false);
        // battleUI.SetAnnouncerText("Choose an action");
        // battleUI.ShowBattleOptions(true);
        // battleUI.ShowWeaponOptions(false);
        // battleUI.ShowHealItemOptions(false);

        // TODO: Remove this eventually
        myEnemyUnit.gameObject.GetComponent<CharacterManager>().FlipCharacter(true);

        myBattleStartEvent.Invoke();
    }

    public void PlayerTurn(string option) {
        myPlayerUnit.isDefending = false; // Reset defending

        BattleOptions battleOption = (BattleOptions)Enum.Parse(typeof(BattleOptions), option);

        switch (battleOption) {
            case BattleOptions.ATTACK:
                StartCoroutine(PlayerTurn(myPlayerUnit.baseAttack));
                break;
            case BattleOptions.SPECIAL:
                myRequestShowWeaponsEvent.Invoke();
                break;
            case BattleOptions.HEALITEM:
                myRequestShowHealsEvent.Invoke();
                break;
            case BattleOptions.DEFEND:
                StartCoroutine(PlayerDefend());
                break;
        }
    }

    public void OnWeaponSelected(string moveName) {
        // battleUI.ShowWeaponOptions(false);
        StartCoroutine(PlayerSpecialAttack(moveName));
    }

    public void OnHealItemSelected(string itemName) {
        // battleUI.ShowHealItemOptions(false);
        StartCoroutine(PlayerHeal(itemName));
    }

    IEnumerator EnemyTurn() {
        myState = BattleState.ENEMY_TURN;
        // battleUI.ShowBattleOptions(false);
        // battleUI.SetAnnouncerText(myEnemyUnit.unitName + " is attacking");
        yield return new WaitForSeconds(2f);

        if (myPlayerUnit.TakeDamage(myEnemyUnit.baseAttack)) {
            // battleUI.SetAnnouncerText(myPlayerUnit.unitName + " has fainted");
            yield return new WaitForSeconds(2f);
            EndBattle(BattleState.LOST);
        } else {
            // battleUI.SetAnnouncerText("You have " + myPlayerUnit.currentHP + " HP remaining");
            // battleUI.SetHP(myPlayerUnit.currentHP, true);
            myState = BattleState.PLAYER_TURN;
            // battleUI.ShowBattleOptions(true);
        }
    }

    IEnumerator PlayerTurn(int damage) {
        myState = BattleState.PLAYER_TURN;
        // battleUI.ShowBattleOptions(false);
        // battleUI.SetAnnouncerText("You attacked " + myEnemyUnit.unitName + " for " + damage + " damage");
        yield return new WaitForSeconds(1f);
        if (myEnemyUnit.TakeDamage(damage)) {
            // battleUI.SetHP(myEnemyUnit.currentHP, false);
            // battleUI.SetAnnouncerText(myEnemyUnit.unitName + " has fainted!");
            yield return new WaitForSeconds(2f);
            EndBattle(BattleState.WON);
        } else {
            // battleUI.SetHP(myEnemyUnit.currentHP, false);
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerSpecialAttack(string moveName) {
        myState = BattleState.PLAYER_TURN;
        // battleUI.ShowBattleOptions(false);
        // // TODO: Hard code some animation for special moves
        // battleUI.SetAnnouncerText(myPlayerUnit.unitName + " used Special Attack: " + moveName);
        yield return new WaitForSeconds(2f);
        int damage = myAttackItems[moveName].damage;
        StartCoroutine(PlayerTurn(damage));
    }

    IEnumerator PlayerHeal(string itemName) {
        myState = BattleState.PLAYER_TURN;
        // battleUI.ShowBattleOptions(false);
        // battleUI.SetAnnouncerText(myPlayerUnit.unitName + " used Heal Item: " + itemName);
        yield return new WaitForSeconds(2f);
        int healAmount = myHealItems[itemName].amount;
        myPlayerUnit.Heal(healAmount);
        // battleUI.SetHP(myPlayerUnit.currentHP, true);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerDefend() {
        myPlayerUnit.isDefending = true;
        // battleUI.ShowBattleOptions(false);
        // battleUI.SetAnnouncerText(myPlayerUnit.unitName + " is defending!");
        yield return new WaitForSeconds(2f);
        // TODO: Hard code some animation for defending
        StartCoroutine(EnemyTurn());
    }

    public void EndBattle(BattleState result) {
        myState = result;
        // battleUI.SetAnnouncerText("Battle over");
        // battleUI.ShowBattleOptions(false);
    }
}
