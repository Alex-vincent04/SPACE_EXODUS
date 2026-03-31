using UnityEngine;
using UnityEngine.SceneManagement;

public class Planet1wayF2 : MonoBehaviour
{
    public Transform TARGET; // Assign the target (planet) in the Inspector
    public float speed = 5f; // Movement speed

    void Update()
    {
        // Move the player towards the target
        transform.position = Vector3.MoveTowards(transform.position, TARGET.position, speed * Time.deltaTime);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Planet1Way2"))
        {
            SceneManager.LoadScene(6);
        }
    }
}
