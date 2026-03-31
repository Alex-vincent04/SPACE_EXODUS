using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosion;


    public void OnImpact(Vector3 pos)
    {
        GameObject go = Instantiate(explosion, pos, Quaternion.identity, transform) as GameObject;
        Destroy(go, 6f);
    }
}
