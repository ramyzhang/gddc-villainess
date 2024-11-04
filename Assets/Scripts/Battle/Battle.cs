using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public BattleState state;
    public BattleUnit playerUnit;
    public BattleUnit enemyUnit;
    public BattleUI battleUI;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattle() {
        state = BattleState.PLAYER_TURN;
        battleUI.SetAnnouncerText("Choose an action");
        battleUI.ShowBattleOptions(true);
    }

    public void PlayerTurn(string option) {
        BattleOptions battleOption = (BattleOptions)Enum.Parse(typeof(BattleOptions), option);

        switch (battleOption) {
            case BattleOptions.ATTACK:
                StartCoroutine(PlayerTurn());
                break;
        }
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
            battleUI.SetAnnouncerText(playerUnit.unitName + " has " + playerUnit.currentHP + " HP remaining");
            battleUI.SetHP(playerUnit.currentHP, true);
            state = BattleState.PLAYER_TURN;
            battleUI.ShowBattleOptions(true);
        }
    }

    IEnumerator PlayerTurn() {
        state = BattleState.PLAYER_TURN;
        if (enemyUnit.TakeDamage(playerUnit.baseAttack)) {
            battleUI.SetHP(enemyUnit.currentHP, false);
            battleUI.SetAnnouncerText(enemyUnit.unitName + " has fainted!");
            yield return new WaitForSeconds(2f);
            EndBattle(BattleState.WON);
        } else {
            battleUI.SetAnnouncerText(playerUnit.unitName + " attacked " + enemyUnit.unitName + " for " + playerUnit.baseAttack + " damage");
            battleUI.SetHP(enemyUnit.currentHP, false);
            StartCoroutine(EnemyTurn());
        }
    }

    public void EndBattle(BattleState result) {
        state = result;
        battleUI.SetAnnouncerText("Battle over");
        battleUI.ShowBattleOptions(false);
    }
}
