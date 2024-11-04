using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Item", menuName = "Inventory/Heal Item")] 
public class HealItem : Item
{
    public int amount = 10;
}
