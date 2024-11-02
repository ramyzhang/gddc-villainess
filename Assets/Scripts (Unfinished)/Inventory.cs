using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory Instance; //singleton

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        Instance = this;
    }
    #endregion

    public static List<Item> items = new();
    private const int maxItems = 16;

    [YarnCommand("addItem")]
    public void AddItem(string itemName) //will need to use this function for picking up item
    {
        if (items.Count < maxItems)
        {   
            Item item = Resources.Load<Item>("Items/" + itemName);
            items.Add(item);
        }
    }

    [YarnCommand("removeItem")]
    public void RemoveItem(string itemName)
    {
        foreach (Item item in items)
        {
            if (item.name == itemName)
            {
                items.Remove(item);
                break;
            }
        }
    }

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
