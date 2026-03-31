using UnityEngine;
using UnityEngine.SceneManagement;

public class BackHome : MonoBehaviour
{
    public int fuelRequired = 20; // Required fuel amount

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (InventoryManager.Instance != null && InventoryManager.Instance.HasEnoughFuel(fuelRequired))
            {
                Debug.Log("Enough fuel! Proceeding to next scene...");
                SceneManager.LoadScene("NextScene"); // Replace "NextScene" with your actual scene name
            }
            else
            {
                Debug.Log("Not enough fuel to take off!");
            }
        }
    }
}
