using UnityEngine;

public class OrbCombiner : MonoBehaviour
{
    public GameObject interactionUI; // UI prompt (e.g., "Press E to convert orbs to fuel")
    public int orbsRequired = 5; // Number of orbs needed to convert into fuel
    public int fuelAmount = 10; // Fuel gained per conversion

    private bool isPlayerInRange = false;

    private void Start()
    {
        interactionUI.SetActive(false); // Hide UI initially
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactionUI.SetActive(true); // Show UI prompt
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionUI.SetActive(false); // Hide UI prompt
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ConvertOrbsToFuel();
        }
    }

    private void ConvertOrbsToFuel()
    {
        if (InventoryManager.Instance.HasEnoughOrbs(orbsRequired))
        {
            InventoryManager.Instance.RemoveOrbs(orbsRequired);
            Debug.Log("Orbs converted to fuel! Current fuel: " + fuelAmount);
        }
        else
        {
            Debug.Log("Not enough orbs to convert!");
        }
    }
}
