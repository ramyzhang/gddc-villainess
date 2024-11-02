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

    public List<Item> items = new List<Item>();

    [YarnCommand("AddItem")]
    public void AddItem(string itemName) //will need to use this function for picking up item
    {
        if (items.Count < 16)
        {   
            Item item = Resources.Load<Item>("Items/" + itemName);
            items.Add(item);
        }
    }

    [YarnCommand("RemoveItem")]
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

    public bool CheckForItem(string itemName)
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
