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
    public GameObject score;
    public GameObject scoreTitle;

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
        
        //score = GameObject.Find("txtScoreNumber");
        
        //pushChicken = btnPushChicken.GetComponent<Button>();
        //pushChicken.onClick.AddListener(behaveRoad.);
    }
    
    // Update is called once per frame
    void Update()
    {

        if (chicken==null)
        {
            logger.Log("no chicken");
            chicken = GameObject.FindWithTag("SpawnedObject");
            //chickenAnimate = chicken.GetComponent<Animator>();
            
        }
        else
        {
            //Show scores
            

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
                    txtOnScreen.SetActive(false);
                    score.SetActive(true);
                    scoreTitle.SetActive(true);
                }

            }
            else
            {
                //logger.Log("Chicken to Idle");
                if (chickenFound)
                {
                    chickenAnimate.SetBool("Run", false);
                    chickenAnimate.SetBool("Turn Head", true);

                }
                //recheck the chicken to Test
                if (cdCount > 3)
                {
                    chickenFound = false;
                    txtOnScreen.SetActive(false);
                    txtOnScreen.SetActive(true);
                    btnShowPushChicken.SetActive(false);
                }
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
                if (chickenFound == false)
                {
                    //Show Chicken FOUND only the FIRST TIME
                    logger.Log("Chicken FOUND");
                    txtUI.text = "Chicken FOUND";
                    logger.Log("Chicken first FOUND");

                }

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
        
    }
