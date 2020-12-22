using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;


public class LightScript : MonoBehaviour
{
    private Light myLight;
    public ARCameraManager manager;

    // Start is called before the first frame update
    void Start()
    {
        myLight = this.GetComponent<Light>();
        RenderSettings.ambientMode = AmbientMode.Skybox;

    }

    void OnEnable()
    {
        manager.frameReceived += handleChange;
    }
    void OnDisable()
    {
        manager.frameReceived -= handleChange;
    }

    void handleChange(ARCameraFrameEventArgs args)
    {
        myLight.intensity = 
            args.lightEstimation.averageBrightness.Value;
        
        myLight.colorTemperature =
            args.lightEstimation.averageColorTemperature.Value;
        
        myLight.color = 
            args.lightEstimation.colorCorrection.Value;
        
        myLight.transform.rotation =
            Quaternion.LookRotation(args.lightEstimation.mainLightDirection.Value);
        
        myLight.intensity = 
            args.lightEstimation.mainLightIntensityLumens.Value;

        RenderSettings.ambientProbe = 
            args.lightEstimation.ambientSphericalHarmonics.Value;

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
