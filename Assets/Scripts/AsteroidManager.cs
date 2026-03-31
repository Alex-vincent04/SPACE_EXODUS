//using Unity.Mathematics;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public int astroidSpacing = 10; //number of asteroids in one axis
    public int gridSpacing = 10;
    public GameObject Asteroid;
    void Start()
    {
        PlaceAsteroids();
    }
    void Update()
    {

    }
    void PlaceAsteroids()
    {
        for (int x = 0; x < astroidSpacing; x++)
        {
            for (int y = 0; y < astroidSpacing; y++)
            {
                for (int z = 0; z < astroidSpacing; z++)
                {
                    InstantiateAsteroid(x, y, z);
                }
            }
        }
    }
    void InstantiateAsteroid(int x, int y, int z)
    {
        Instantiate(Asteroid,
            new Vector3(transform.position.x + (x * gridSpacing) + AsteroidOffset(),
                        transform.position.y + (y * gridSpacing) + AsteroidOffset(),
                        transform.position.z + (z * gridSpacing) + AsteroidOffset()),
            Quaternion.identity,
            transform);
    }

    float AsteroidOffset()
    {
        return UnityEngine.Random.Range(-gridSpacing / 2f, gridSpacing / 2f);
    }
}
