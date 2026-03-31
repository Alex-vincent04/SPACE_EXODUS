using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 defaultDistance = new Vector3(0f, 2f, -10f);
    public float distanceDamp = 10f;
    //public float rotationDamp = 10f;
    public Vector3 velocity = Vector3.one;

    void LateUpdate()
    {
        SmoothFollow();

        //Vector3 toPos = target.position + (target.rotation * defaultDistance);
        //Vector3 curPos = Vector3.Lerp(transform.position, toPos, distanceDamp * Time.deltaTime);
        //transform.position = curPos;

        //Quaternion toRot = Quaternion.LookRotation(target.position - transform.position, target.up);
        //Quaternion curRot = Quaternion.Slerp(transform.rotation, toRot, rotationDamp * Time.deltaTime);
        //transform.rotation = curRot;
    }

    void SmoothFollow()
    {
        Vector3 toPos = target.position + (target.rotation * defaultDistance);
        Vector3 curPos = Vector3.SmoothDamp(transform.position, toPos, ref velocity, distanceDamp);
        transform.position = curPos;

        transform.LookAt(target,target.up);
    }
}
