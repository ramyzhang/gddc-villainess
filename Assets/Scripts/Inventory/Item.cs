using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")] 
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public string displayName = "New Item";
    public string description = "Enter Description Here";
    public Sprite icon = null; //itemImage
    public bool isDefaultItem = false; //item status
}
