using UnityEngine;
using UnityEngine.UI;

public class OrbUIManager : MonoBehaviour
{
    public Text orbCountText; // Assign in Inspector
    private int totalOrbs;

    void Start()
    {
        void Start()
{
    if (InventoryManager.Instance == null)
    {
        Debug.LogError("InventoryManager instance is missing!");
        return;
    }
    UpdateOrbCount();
}

        UpdateOrbCount();
    }

    public void UpdateOrbCount()
    {
        totalOrbs = InventoryManager.Instance.GetOrbCount(); // Get orbs from InventoryManager
        orbCountText.text = "Orbs: " + totalOrbs;
    }
}
