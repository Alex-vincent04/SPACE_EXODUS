using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float RotationSpeed = 1.25f;
    public float EnemySpeed = 20f;

    private void Update()
    {
        Turn();
        Move();
    }

    void Turn()
    {
        Vector3 pos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos) ;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationSpeed * Time.deltaTime);
    }

    void Move()
    {
        transform.position += transform.forward * EnemySpeed * Time.deltaTime;
    }
}
