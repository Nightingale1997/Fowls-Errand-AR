using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;

public class behaveChicken : MonoBehaviour
{
    private ARRaycastManager rays;
    public Camera myCam;
    public float cooldown, cdCount;
    private Animator chickenAnimate;
    bool chickenFound = false;
    private static ILogger logger = Debug.unityLogger;

    // Start is called before the first frame update
    void Start()
    {
        cooldown = 2;
        myCam = this.gameObject.transform.Find
            ("AR Camera").gameObject.GetComponent<Camera>();
        rays = this.gameObject.GetComponent<ARRaycastManager>();

    }

    GameObject[] chickSearch;
    // Update is called once per frame
    void Update()
    {
    
        if (chickSearch == null || chickSearch.Length==0)
        {
            logger.Log("searching chicks");
            chickSearch = GameObject.FindGameObjectsWithTag("SpawnedObject");
            logger.Log("array " + chickSearch.Length);
            if (chickSearch!=null && chickSearch.Length>0)
            {
                chickenAnimate = chickSearch[0].GetComponent<Animator>();
            }            
        }
        else
        {
            cdCount += Time.deltaTime;

            if (Input.touchCount == 1 && cdCount > cooldown)
            {
                fusRohDah();
                cdCount = 0;
            }
            if (cdCount > cooldown && chickenFound == false)
            {
                //Spawn();
                logger.Log("Check on Chicken...");                
                findChicken();
                cdCount = -3;
            }
            else
            {
                //logger.Log("Chicken to Idle");
                //chickenAnimate = hit.transform.gameObject.GetComponent<Animator>();
                chickenAnimate.SetBool("Run", false);
                if (cdCount>15)
                {
                    chickenFound = false;
                }
            }

        }      

    }

    public void findChicken()
    {
        logger.Log("raycast for the running animation");
        //bool hitChicken;
        Vector3 screenPoint = myCam.WorldToViewportPoint(new Vector3(0.5f, 0.5f));
        RaycastHit[] myHits;
        Ray r;
        r = myCam.ScreenPointToRay(screenPoint);
        myHits = Physics.RaycastAll(r);

        //hitChicken = rays.Raycast(screenPoint, myHits, TrackableType.FeaturePoint);
        foreach (RaycastHit hit in myHits)
        {            
            if (hit.transform.gameObject.tag == "SpawnedObject")
            {
                logger.Log("Chicken FOUND");

                chickSearch[0].GetComponent<Rigidbody>().velocity= new Vector3(0f, 1.5f, 0f); ;
                chickenAnimate.SetBool("Run", true);
                chickenFound = true;
            }
            else 
            {                               
                logger.Log("only "+ hit.transform.gameObject.name);           
                
            }            
        }
        //when not looking at anything

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
                chickSearch[0].transform.gameObject.GetComponent<Rigidbody>().AddForce
                    (r.direction * 100);
                //hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(r.direction*100);
            }
        }
    }

    /*public void Spawn()
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
  */

}
