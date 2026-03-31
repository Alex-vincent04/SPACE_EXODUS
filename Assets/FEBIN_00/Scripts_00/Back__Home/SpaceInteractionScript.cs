using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipInteraction : MonoBehaviour
{
    public GameObject interactionUI; // UI prompt (e.g., "Press E to enter spaceship")
    public int fuelRequired = 20; // Fuel needed to proceed

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
            AttemptToEnterSpaceship();
        }
    }

    private void AttemptToEnterSpaceship()
    {
        if (InventoryManager.Instance.HasEnoughFuel(fuelRequired))
        {
            Debug.Log("Enough fuel! Proceeding to next scene...");
            SceneManager.LoadScene(7); // Change "NextScene" to your actual scene name
        }
        else
        {
            Debug.Log("Not enough fuel to take off!");
        }
    }
}
