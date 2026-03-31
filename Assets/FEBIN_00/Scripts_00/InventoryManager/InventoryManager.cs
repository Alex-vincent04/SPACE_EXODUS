using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure it persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add an item to the inventory
    public void AddItem(string itemName, int amount = 1)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory[itemName] = amount;
        }
    }

    // Check if player has enough orbs (both Red and Blue combined)
    public bool HasEnoughOrbs(int requiredOrbs)
    {
        int totalOrbs = GetOrbCount();
        return totalOrbs >= requiredOrbs;
    }

    // Remove a specified number of orbs
    public void RemoveOrbs(int amount)
    {
        int redOrbs = GetRedOrbCount();
        int blueOrbs = GetBlueOrbCount();
        int totalOrbs = redOrbs + blueOrbs;

        if (totalOrbs >= amount)
        {
            while (amount > 0)
            {
                if (redOrbs > 0)
                {
                    int toRemove = Mathf.Min(amount, redOrbs);
                    inventory["RedOrb"] -= toRemove;
                    amount -= toRemove;
                    redOrbs -= toRemove;
                }
                else if (blueOrbs > 0)
                {
                    int toRemove = Mathf.Min(amount, blueOrbs);
                    inventory["BlueOrb"] -= toRemove;
                    amount -= toRemove;
                    blueOrbs -= toRemove;
                }
            }
        }
    }

    // Get total number of orbs (Red + Blue)
    public int GetOrbCount()
    {
        return GetRedOrbCount() + GetBlueOrbCount();
    }

    public int GetRedOrbCount()
    {
        return inventory.ContainsKey("RedOrb") ? inventory["RedOrb"] : 0;
    }

    public int GetBlueOrbCount()
    {
        return inventory.ContainsKey("BlueOrb") ? inventory["BlueOrb"] : 0;
    }

    // Fuel system
    public bool HasEnoughFuel(int requiredFuel)
    {
        return inventory.ContainsKey("Fuel") && inventory["Fuel"] >= requiredFuel;
    }

    public void AddFuel(int amount)
    {
        AddItem("Fuel", amount);
    }

    public void RemoveFuel(int amount)
    {
        if (HasEnoughFuel(amount))
        {
            inventory["Fuel"] -= amount;
        }
    }

    public int GetFuelCount()
    {
        return inventory.ContainsKey("Fuel") ? inventory["Fuel"] : 0;
    }

    // Print inventory for debugging
    public void PrintInventory()
    {
        Debug.Log("Inventory Contents:");
        foreach (var item in inventory)
        {
            Debug.Log(item.Key + ": " + item.Value);
        }
    }
}
