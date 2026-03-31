using System.Linq;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
   public Laser[] laser;
void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Space))
        {

            foreach (Laser l in laser)
            {
                //Vector3 pos = transform.position + (transform.forward * l.Distance);
                l.FireLaser();
            }

        }
    }
}
