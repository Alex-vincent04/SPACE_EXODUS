using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float laserOnTime = 1f;
    public float maxDistance = 300f;
    public float fireDelay;
    LineRenderer lineRenderer;
    Light laserLight;
    bool canFire;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        laserLight = GetComponent<Light>();
    }

    private void Start()
    {
        lineRenderer.enabled = false;
        laserLight.enabled = false;
        canFire = true;
    }

    //private void Update()
    //{
    //    FireLaser(transform.forward * maxDistance);
    //}

    //private void Update()
    //{
    //    Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.forward) * maxDistance,Color.green);
    //}

    Vector3 CastRay()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward) * maxDistance;
        if(Physics.Raycast(transform.position, fwd, out hit))
        {
            Debug.Log("hit" + hit.transform.name);

            Explosion temp = hit.transform.GetComponent<Explosion>();
            
            if(temp != null)
                temp.OnImpact(hit.point);

            return hit.point;
        }
            Debug.Log("Miss");
            return transform.position + (transform.forward * maxDistance);
    }

    public void FireLaser()
    {
        Vector3 pos = CastRay();
        FireLaser(pos);
    }

    public void FireLaser(Vector3 targetPosition)
    {
        if (canFire)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, targetPosition);
            lineRenderer.enabled = true;
            laserLight.enabled = true;
            canFire = false;
            Invoke("TurnOffLaser", laserOnTime);
            Invoke("CanFire", fireDelay);
        }
    }

    void TurnOffLaser()
    {
        lineRenderer.enabled = false;
        laserLight.enabled = false;
        //canFire = true;
    }

    public float Distance
    {
        get { return maxDistance; }
    }

    void CanFire()
    {
        canFire = true;
    }
}
 