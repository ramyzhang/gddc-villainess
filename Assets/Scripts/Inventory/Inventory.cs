using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance; //singleton

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

    public delegate void OnNewItem(Item item);
    public OnNewItem onNewItemCallback;

    public static List<Item> items = new();
    // public List<Item> items = new();
    private const int maxItems = 16;


    [YarnCommand("addItem")]
    public void AddItem(string itemName) //will need to use this function for picking up item
    {
        Item item = Resources.Load<Item>("Constants/Items/" + itemName);
        AddItem(item);  // Call the base implementation
    }

    public void AddItem(Item item)
    {
        if (items.Count < maxItems)
        {
            items.Add(item);

            onItemChangedCallback?.Invoke();
            onNewItemCallback?.Invoke(item);
        }
    }

    [YarnCommand("removeItem")]
    public void RemoveItem(string itemName)
    {
        Item tempItem = null;
        foreach (Item item in items)
        {
            if (item.name == itemName)
            {
                tempItem = item;
                break;
            }
        }

        RemoveItem(tempItem);
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        onItemChangedCallback?.Invoke();
    }

    // Comment this out and change the "items" variable to a non-static one to test the Inventory with the Inspector
    
    [YarnFunction("checkForItem")]
    public static bool CheckForItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.name == itemName)
            {
                return true;
            }
        }

        return false;
    }
}
