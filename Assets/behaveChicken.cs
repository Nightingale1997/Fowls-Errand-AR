using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;

public class behaveChicken : MonoBehaviour
{
    private ARRaycastManager rays;
    public GameObject chickenPrefab;    
    public Camera myCam;
    public float cooldown, cdCount;
    private ARAnchorManager anchorCheck;
    private ARPlaneManager planeCheck;
    private Animator chickenAnimate;

    private static ILogger logger = Debug.unityLogger;
    // Start is called before the first frame update
    void Start()
    {
        cooldown = 2;
        myCam = this.gameObject.transform.Find
            ("AR Camera").gameObject.GetComponent<Camera>();
        rays = this.gameObject.GetComponent<ARRaycastManager>();
        
        //to check if the raycast actually hit a Plane
        anchorCheck = this.gameObject.GetComponent<ARAnchorManager>();
        planeCheck = this.gameObject.GetComponent<ARPlaneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        cdCount += Time.deltaTime;

        if (Input.touchCount==1)
        {
            fusRohDah();
        }
        else if (cdCount>cooldown && Input.touchCount ==2)
        {
            Spawn();
            cdCount=0;
        }


        // This is an attempt to make the chicken flap it's wings when in the moddle of the screen
        Vector3 screenPoint = myCam.WorldToViewportPoint(new Vector3(0.5f, 0.5f));
        RaycastHit[] myHits;
        Ray r;

        r = myCam.ScreenPointToRay(screenPoint);
        myHits = Physics.RaycastAll(r);

        foreach (RaycastHit hit in myHits)
        {
            if (hit.transform.gameObject.tag == "SpawnedObject")
            {
                chickenAnimate.SetBool("Run", true);
            }
            else
            {
                chickenAnimate.SetBool("Run", false);
            }
        }
    }

    public void Spawn()
    {
        GameObject chicken;
        Vector3 screenCenter;
        bool hit;
        ARRaycastHit nearest;
        List<ARRaycastHit> myHits = new List<ARRaycastHit>();
        ARPlane plane;
        ARAnchor anchorPoint;
        screenCenter = myCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        hit = rays.Raycast(screenCenter,
            myHits,
            TrackableType.FeaturePoint | TrackableType.PlaneWithinPolygon);

        logger.Log("Hit: " + hit);

        if (hit==true)
        {
            nearest = myHits[0];
            chicken = Instantiate(chickenPrefab, 
                nearest.pose.position+nearest.pose.up*0.1f,
                nearest.pose.rotation);

            chicken.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            chicken.transform.eulerAngles = new Vector3(0,180,0);
            chicken.tag = "SpawnedObject";
            logger.Log("Spawned at" + chicken.transform.position.x +
                ", " + chicken.transform.position.y + ", " +
                chicken.transform.position.z);


            plane = planeCheck.GetPlane(nearest.trackableId);

            if (plane!=null)
            {
                anchorPoint = anchorCheck.AttachAnchor(plane, nearest.pose);
                logger.Log("Added an anchor to a plane " + nearest);
            }
            else
            {
                anchorPoint = anchorCheck.AddAnchor(nearest.pose);
                logger.Log("Added another anchor "+nearest);
            }
            chicken.transform.parent = anchorPoint.transform;
        }
        
    }

    public void fusRohDah() 
    {
        RaycastHit[] myHits;
        Ray r;

        r = myCam.ScreenPointToRay(Input.GetTouch(0).position);
        myHits = Physics.RaycastAll(r);

        foreach (RaycastHit hit in myHits)
        {
            logger.Log("Detected " + hit.transform.gameObject.name);
            if (hit.transform.gameObject.tag=="SpawnedObject")
            {
                logger.Log("Conjuring the FUS ROH DAH");
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce
                    (r.direction*100);
            }
        }
    }



}
