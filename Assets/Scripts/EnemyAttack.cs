using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;
    public Laser Laser;

    Vector3 hitPosition;


    private void Update()
    {
        InFront();
        HaveLineOfSight();
        if(InFront() && HaveLineOfSight())
        {
            FireLaser();
        }
    }
    bool InFront()
    {
        Vector3 distToTarget = transform.position - target.position;
        float angle=Vector3.Angle(target.position, distToTarget);

        if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270) 
        {
            Debug.DrawLine(transform.position, target.position, Color.green);
            return true;
        }
       // Debug.DrawLine(transform.position, target.position, Color.red);
        return false;
    }

    bool HaveLineOfSight()
    {
        RaycastHit hit;
        Vector3 direction = target.position - transform.position;

        if(Physics.Raycast(Laser.transform.position, direction, out hit, Laser.Distance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.DrawRay(Laser.transform.position, direction, Color.blue);
                hitPosition = hit.transform.position;
                return true;
            }
        }
        return false;
    }

    void FireLaser()
    {
        Debug.Log("");
        Laser.FireLaser(hitPosition);
    }
}
