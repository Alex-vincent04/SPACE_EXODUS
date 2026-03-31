using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Collided with: " + other.gameObject.name + " | Tag: " + other.tag); 

    if (other.CompareTag("BlueOrb"))
    {
        Debug.Log("BlueOrb detected!"); // This should appear in Console if collision works

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem("BlueOrb");
            InventoryManager.Instance.PrintInventory();
        }
        Destroy(other.gameObject);
    }
    else if (other.CompareTag("RedOrb"))
    {
        Debug.Log("RedOrb detected!");

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.AddItem("RedOrb");
            InventoryManager.Instance.PrintInventory();
        }
        Destroy(other.gameObject);
    }
}

}
