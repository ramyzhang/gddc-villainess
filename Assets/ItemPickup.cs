using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    // TODO: refactor this to use the GameTrigger system
	public Item item;	// Item to put in the inventory if picked up

    void OnMouseDown()
    {
        Inventory.instance.AddItem(item);	// Add to inventory

        Destroy(gameObject);	// Destroy item from scene  
    }
}
