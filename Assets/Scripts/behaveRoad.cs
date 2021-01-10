using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.UI;

public class behaveRoad : MonoBehaviour
{
    // Start is called before the first frame update
    private ARRaycastManager rays;
    public GameObject roadPrefab;
    public GameObject btnSetMap;
    public GameObject txtFindChicken;
    public Camera myCam;
    //private float cooldown, cdCount;
    private ARAnchorManager anchorCheck;
    private ARPlaneManager planeCheck;
    private Animator chickenAnimate;
    private bool roadOn = false;
    private bool signFound = false;


    //Set UI
    

    private Button setMap;

    private static ILogger logger = Debug.unityLogger;
    // Start is called before the first frame update
    void Start()
    {
        //cooldown = 2;
        myCam = this.gameObject.transform.Find
            ("AR Camera").gameObject.GetComponent<Camera>();
        rays = this.gameObject.GetComponent<ARRaycastManager>();

        //to check if the raycast actually hit a Plane
        anchorCheck = this.gameObject.GetComponent<ARAnchorManager>();
        planeCheck = this.gameObject.GetComponent<ARPlaneManager>();

        setMap = btnSetMap.GetComponent<Button>();
        setMap.onClick.AddListener(SpawnRoad);
        btnSetMap.SetActive(false);
        txtFindChicken.GetComponent<Text>().text = "Find a Stop Sign!";
        txtFindChicken.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnRoad()
    {
        if (Input.touchCount == 1 && roadOn == false)
        {
            GameObject road;
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

            if (hit == true)
            {
                nearest = myHits[0];
                road = Instantiate(roadPrefab,
                    nearest.pose.position + nearest.pose.up * 0.1f,
                    nearest.pose.rotation);

                road.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                road.transform.eulerAngles = new Vector3(0, 180, 0);

                logger.Log("Spawned at" + road.transform.position.x +
                    ", " + road.transform.position.y + ", " +
                    road.transform.position.z);


                plane = planeCheck.GetPlane(nearest.trackableId);

                if (plane != null)
                {
                    anchorPoint = anchorCheck.AttachAnchor(plane, nearest.pose);
                    logger.Log("Added an anchor to a plane " + nearest);

                }
                else
                {
                    anchorPoint = anchorCheck.AddAnchor(nearest.pose);
                    logger.Log("Added another anchor " + nearest);
                }
                road.transform.parent = anchorPoint.transform;
                
                
                roadOn = true;
                Destroy(btnSetMap.gameObject);
                txtFindChicken.GetComponent<Text>().text = "Find the Chicken!";
                txtFindChicken.SetActive(true);
                

            }
            else
            {
                // no hit on plane
            }           
            

        }      

    }
}
