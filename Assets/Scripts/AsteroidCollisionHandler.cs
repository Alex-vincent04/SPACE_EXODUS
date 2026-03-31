using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidCollisionHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene(9);
            Debug.Log("Spaceship destroyed by asteroid collision!");
        }
    }
}