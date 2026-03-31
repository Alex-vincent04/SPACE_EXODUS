using UnityEngine;

public class Orbital : MonoBehaviour
{
    public GameObject Sun;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OrbitAround();
    }
    void OrbitAround()
    {
        transform.RotateAround(Sun.transform.position, Vector3.up, speed * Time.deltaTime);
    }
}
