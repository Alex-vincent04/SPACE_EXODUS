using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float minScale = .8f;
    public float maxScale = 1.2f;
    public float minRotation = .8f;
    public float maxRotation = 1.2f;
    public float rotationSpeed = 5f;


    Vector3 randomRotation;
    private void Start()
    {

        Vector3 scale = Vector3.one;
        scale.x = Random.Range(minScale, maxScale);
        scale.y = Random.Range(minScale, maxScale);
        scale.z = Random.Range(minScale, maxScale);

        transform.localScale = scale;

        randomRotation.x = Random.Range(minRotation, maxRotation);
        randomRotation.y = Random.Range(minRotation, maxRotation);
        randomRotation.z = Random.Range(minRotation, maxRotation);

    }
    private void Update()
    {
        transform.Rotate(randomRotation * rotationSpeed * Time.deltaTime);
    }
}


