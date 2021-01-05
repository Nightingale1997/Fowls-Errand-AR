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
                cdCount = 0;

                if (chickenFound)
                {
                    //CHICKEN MOVE
                    //chickenmove();
                    btnShowPushChicken.SetActive(true);
                }

            }
            else
            {
                //logger.Log("Chicken to Idle");
                if (chickenAnimate != null)
                {
                    chickenAnimate.SetBool("Run", false);

                }
                //recheck the chicken to Test
                if (cdCount>5)
                {
                    chickenFound = false;
                    txtUI.text = "";
                }
            }

        }      

    }

    public void findChicken2()
    {
        //Vector3 screenPoint = myCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 screenPoint = new Vector3(0.5f, 0.5f, 0f);
        RaycastHit[] myHits;
        Ray r;
        r = myCam.ViewportPointToRay(screenPoint);

        myHits = Physics.RaycastAll(r);
        foreach (RaycastHit hit in myHits)
        {
 //           logger.Log("Hitting " + hit.transform.gameObject.name);
            if (hit.transform.gameObject.tag == "SpawnedObject")
            {
                logger.Log("Chicken FOUND");
                txtUI.text = "Chicken FOUND";

                //hit.transform.GetComponent<Rigidbody>().velocity = new Vector3(0f, 1.2f, 0f);
                hit.transform.Rotate(Quaternion.LookRotation(r.direction).eulerAngles);
                logger.Log("dir RAY: " + r.direction.normalized + " Angle Euler: " + Quaternion.Euler(r.direction).eulerAngles + " Angle Look: " + Quaternion.LookRotation(r.direction).eulerAngles);

                //hit.transform.GetComponent<Rigidbody>().AddForce (r.direction * 1000);

            }
            else
            {
                logger.Log("only " + hit.transform.gameObject.name);

            }
        }

    }
    public void findChicken()
    {
        logger.Log("raycasting to find chicken");
        //bool hitChicken;
        Vector3 screenPoint = new Vector3(0.5f, 0.5f, 0f);
        //Vector3 screenPoint = Camera.main.WorldToViewportPoint(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit[] myHits;
        Ray r;
        r = myCam.ViewportPointToRay(screenPoint);

        logger.Log("1");

        myHits = Physics.RaycastAll(r);
        //hitChicken = rays.Raycast(screenPoint, myHits, TrackableType.FeaturePoint);
        foreach (RaycastHit hit in myHits)
        {
            if (hit.transform.gameObject.tag == "SpawnedObject")
            {
                logger.Log("Chicken FOUND");
                txtUI.text = "Chicken FOUND";

                hit.transform.GetComponent<Rigidbody>().velocity = new Vector3(0f, 1.2f, 0f);
                chickenAnimate = hit.transform.GetComponent<Animator>();
                chickenAnimate.SetBool("Run", true);

                chickenFound = true;
            }
            else
            {
                logger.Log("only " + hit.transform.gameObject.name);

            }
        }
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

    

    }
