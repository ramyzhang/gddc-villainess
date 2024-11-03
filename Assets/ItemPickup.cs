using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
	public Item item;	// Item to put in the inventory if picked up

	// Pick up the item

    void OnMouseDown()
    {
        Inventory.instance.AddItem(item);	// Add to inventory

        Destroy(gameObject);	// Destroy item from scene  
    }
}
