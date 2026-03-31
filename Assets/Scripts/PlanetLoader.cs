using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetLoader : MonoBehaviour
{
    public int planetLayer = 9;
    public int planetLayer2 = 11;
     // Assign your planet's layer in the Inspector or set it manually
    public static Vector3 LastShipPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == planetLayer)
        {
            LastShipPosition = other.transform.position;
            
            // Log the position before switching the scene
            Debug.Log($"Last Ship Position: X={LastShipPosition.x}, Y={LastShipPosition.y}, Z={LastShipPosition.z}");

            SceneManager.LoadScene(2);
        }
        if (other.gameObject.layer == planetLayer2)
        {
            LastShipPosition = other.transform.position;
            
            // Log the position before switching the scene
            Debug.Log($"Last Ship Position: X={LastShipPosition.x}, Y={LastShipPosition.y}, Z={LastShipPosition.z}");

            SceneManager.LoadScene(5);
        }
        
    }
}

