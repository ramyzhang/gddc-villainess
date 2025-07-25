using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public Sprite icon;
    public string unitName;
    public int maxHP;
    public int currentHP;
    public int baseAttack;
    public bool isDefending = false;

    void Awake() {
        currentHP = maxHP;
    }

    public bool TakeDamage(int damage) {
        if (isDefending) {
            damage /= 2;
        }
        currentHP -= damage;
        if (currentHP <= 0) {
            currentHP = 0;
            return true;
        } 
        return false;
    }

    public void Heal(int amount) {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
    }

    public void Die() {
        // TODO: Play death animation
        Destroy(gameObject);
    }
}
