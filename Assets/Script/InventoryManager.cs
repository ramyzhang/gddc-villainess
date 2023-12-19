using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class InventoryManager : MonoBehaviour
{
    // Static instance variable to store the singleton instance
    private static InventoryManager _instance;

    /**
    Public property to access the instance.
    Access the InventoryManager singleton instance like so:

        InventoryManager inventoryManager = InventoryManager.Instance;
    **/
    public static InventoryManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        // Ensure there is only one instance of the InventoryManager
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject); // Destroy the duplicate instance
        }
        else
        {
            _instance = this; // Set the instance to this object
            DontDestroyOnLoad(this.gameObject); // Prevent it from being destroyed when loading scenes
        }
    }

    // Inventory variable that stores string names of each inventory item
    private static List<string> inventory = new List<string>();

    [YarnFunction("inventoryIncludes")]
    public static bool inventoryIncludes(string item) {
        Debug.Log($"Item to look for: {item}");
        if (inventory.Contains(item)) {
            Debug.Log("True");
            return true;
        }
        Debug.Log("False");
        return false;
    }

    [YarnCommand("removeInventoryItem")]
    public void removeInventoryItem(string item) {
        Debug.Log($"Item to remove: {item}");
        if (inventory.Contains(item)) {
            Debug.Log("True");
            inventory.Remove(item);
        }
        Debug.Log("False");
    }

    [YarnCommand("addInventoryItem")]
    public void addInventoryItem(string item) {
        Debug.Log($"Item to add: {item}");
        if (!inventory.Contains(item)) {
            Debug.Log("False");
            inventory.Add(item);
        }
        Debug.Log("True");
    }
}
