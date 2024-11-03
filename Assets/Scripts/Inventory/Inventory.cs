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


    public static List<Item> items = new();
    private const int maxItems = 16;

    [YarnCommand("addItem")]
    public void AddItem(string itemName) //will need to use this function for picking up item
    {
        if (items.Count < maxItems)
        {   
            Item item = Resources.Load<Item>("Items/" + itemName);
            items.Add(item);

            Debug.Log("Added " + item.name + " to inventory");
            Debug.Log("Length of items: " + items.Count);

            onItemChangedCallback?.Invoke();
        }
    }

    public void AddItem(Item item)
    {
        if (items.Count < maxItems)
        {
            items.Add(item);

            Debug.Log("Added " + item.name + " to inventory");
            Debug.Log("Length of items: " + items.Count);

            onItemChangedCallback?.Invoke();
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

                onItemChangedCallback?.Invoke();
                break;
            }
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        onItemChangedCallback?.Invoke();
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
