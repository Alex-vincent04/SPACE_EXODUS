using UnityEngine;
using UnityEngine.SceneManagement;

public class Planet1wayF3 : MonoBehaviour
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
        if(other.CompareTag("Home"))
        {
            SceneManager.LoadScene(8);
        }
    }
}
