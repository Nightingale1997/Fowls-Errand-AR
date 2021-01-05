using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.UI;

public class behaveChicken : MonoBehaviour
{
    private ARRaycastManager rays;
    public Camera myCam;
    public float cooldown, cdCount;
    private Animator chickenAnimate;
    bool chickenFound = false;
    //OneChicken
    private GameObject chicken;

    private static ILogger logger = Debug.unityLogger;
    //UI
    public GameObject txtOnScreen;
    public GameObject btnShowPushChicken;

    //public GameObject btnPushChicken;
    public Vector3 directionRay;
    private Text txtUI;

    //private Button pushChicken; 
    void Start()
    {
        cooldown = 2;
        myCam = this.gameObject.transform.Find
            ("AR Camera").gameObject.GetComponent<Camera>();
        rays = this.gameObject.GetComponent<ARRaycastManager>();

         //set UI 
        txtUI = txtOnScreen.GetComponent<Text>();

        //pushChicken = btnPushChicken.GetComponent<Button>();
        //pushChicken.onClick.AddListener(behaveRoad.);
    }

    //GameObject[] chickSearch;
    
    // Update is called once per frame
    void Update()
    {
        /*
        if (chickSearch == null || chickSearch.Length==0)
        {
            logger.Log("searching chicks");
            chickSearch = GameObject.FindGameObjectsWithTag("SpawnedObject");
            //chickSearch = GameObject.FindWithTag("SpawnedObject");
            logger.Log("array " + chickSearch.Length);
            if (chickSearch!=null && chickSearch.Length>0)
            {
                chickenAnimate = chickSearch[0].GetComponent<Animator>();
            }            
        }*/
        if (chicken==null)
        {
            logger.Log("no chicken");
            chicken = GameObject.FindWithTag("SpawnedObject");
            //chickenAnimate = chicken.GetComponent<Animator>();
            
        }
        else
        {
            cdCount += Time.deltaTime;

            if (Input.touchCount == 1 && cdCount > cooldown)
            {
                //fusRohDah();
                
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
                chickenAnimate.SetBool("Run", false);
                //recheck the chicken to Test
                if (cdCount>5)
                {
                    chickenFound = false;
                }
            }

        }      

    }

    public void findChicken()
    {
        logger.Log("raycasting to find chicken");
        //bool hitChicken;
        Vector3 screenPoint = myCam.WorldToViewportPoint(new Vector3(0.5f, 0.5f, 0f));
        //Vector3 screenPoint = Camera.main.WorldToViewportPoint(new Vector3(0.5f, 0.5f, 0f));

        //RaycastHit[] myHits;

        RaycastHit hit2;
        Ray r;
        r = myCam.ScreenPointToRay(screenPoint);

        //STILL CASTS THE R3AY IN DIRECTION OF THE USER
        if (Physics.Raycast(r,out hit2))
        {
            if (hit2.transform.gameObject.tag == "SpawnedObject")
            {                
                txtUI.text = "Chicken FOUND";
                btnShowPushChicken.SetActive(true);
                //txtGuides.SetActive(true);
                logger.Log("Chicken about to be found 2");
                /*
                //GameObject chickTest = GameObject.FindWithTag("SpawnedObject");
                chicken = hit2.transform.gameObject;
                chicken.GetComponent<Rigidbody>().velocity = new Vector3(0f, 10.2f, 0f);
                chickenAnimate = chicken.GetComponent<Animator>();
                */

                hit2.transform.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 1.2f, 0f);
                logger.Log("Chicken about to be found 1");
                //hit2.GetComponent<Rigidbody>().velocity = new Vector3(0f, 10.2f, 0f);
                logger.Log("Chicken about to be found 0");
                chickenAnimate = hit2.transform.gameObject.GetComponent<Animator>();
                //chickenAnimate = chickTest.GetComponent<Animator>();
                
                //chickSearch[0].GetComponent<Rigidbody>().velocity = new Vector3(0f, 1.2f, 0f); ;
                
                chickenAnimate.SetBool("Run", true);
                chickenFound = true;
                directionRay = r.direction;
                logger.Log("Chicken FOUND");
            }
            else
            {
                logger.Log("only " + hit2.transform.gameObject.name);

            }
        }
        else
        {
            logger.Log("No luck with Hit2");
        }

        /*
        myHits = Physics.RaycastAll(r);
        //hitChicken = rays.Raycast(screenPoint, myHits, TrackableType.FeaturePoint);
        foreach (RaycastHit hit in myHits)
        {            
            if (hit.transform.gameObject.tag == "SpawnedObject")
            {
                logger.Log("Chicken FOUND");
                txtUI.text = "Chicken FOUND";
                //txtGuides.SetActive(true);

                chickSearch[0].GetComponent<Rigidbody>().velocity= new Vector3(0f, 1.2f, 0f); ;
                chickenAnimate.SetBool("Run", true);
                chickenFound = true;
            }
            else 
            {                               
                logger.Log("only "+ hit.transform.gameObject.name);           
                
            }            
        }
        //when not looking at anything
        */
    }
  
    /*
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
                    (r.direction * 100);
                //hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(r.direction*100);
            }
        }
    }
    */
  
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
