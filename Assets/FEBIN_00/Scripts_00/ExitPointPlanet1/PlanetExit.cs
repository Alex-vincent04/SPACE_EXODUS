using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetExit : MonoBehaviour
{
    public string spaceSceneName = "SolarSystem_00"; // Change this to your actual space scene name
    public static Vector3 lastShipPosition; // Store ship's last position

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Press "E" to exit the planet
        {
            lastShipPosition = transform.position; // Save the current position
            SceneManager.LoadScene(spaceSceneName); // Load the space scene
        }
    }
}
