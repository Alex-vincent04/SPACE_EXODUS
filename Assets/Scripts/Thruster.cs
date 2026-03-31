using UnityEngine;

public class Thruster : MonoBehaviour
{
    TrailRenderer trailRenderer;
    Light thrusterLight;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        thrusterLight = GetComponent<Light>();
    }

    public void Start()
    {
        //trailRenderer.enabled = false;  
        //thrusterLight.enabled = false;
        thrusterLight.intensity = 0;
    }

    //public void Activate(bool activate = true)
    //{
    //    if (activate)
    //    {
    //        trailRenderer.enabled = true;
    //        thrusterLight.enabled = true;
    //    }
    //    else
    //    {
    //        trailRenderer.enabled = false;
    //        thrusterLight.enabled = false;
    //    }
    //}

    public void Intensity(float inten)
    {
        thrusterLight.intensity = inten * 10f;
    }
}
