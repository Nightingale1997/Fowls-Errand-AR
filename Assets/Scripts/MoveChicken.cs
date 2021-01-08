using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.UI;

public class MoveChicken : MonoBehaviour
{
    public float chickenSpeed = 25.0f;
    public bool active = false;
    public bool old = false;
    private Animator ChickenRun;
    private Rigidbody chickenBody;
    private GameObject road;
    public float timer = 2f;
    public Camera myCam;
    private ARRaycastManager rays;
    //UI
    //public GameObject btnPushChicken;
    private Button pushChicken;
    //public GameObject txtOnScreen;
    private Text txtUI;

    //GameObject[] chickSearch;

    private static ILogger logger = Debug.unityLogger;
    // Start is called before the first frame update
    void Start()
    {
        road = GameObject.FindWithTag("Road");
        ChickenRun = gameObject.GetComponent<Animator>();
        chickenBody = gameObject.GetComponent<Rigidbody>();

        
        logger.Log("Cross Start");
        pushChicken = GameObject.Find("btnPushChicken").GetComponent<Button>(); //btnPushChicken.GetComponent<Button>();
        //pushChicken.onClick.AddListener(pushPressed);
        txtUI = GameObject.Find("txtOnScreen").GetComponent<Text>();
        logger.Log(" Cross-UI Successful");

        
        
    }


    public Vector3 calculateRayDir()
    {
        Vector3 screenPoint = new Vector3(0.5f, 0.5f, 0f);
        RaycastHit hit2;
        Ray r;
        logger.Log("R ray succ");

        myCam = GameObject.Find("AR Camera").gameObject.GetComponent<Camera>();
        rays = GameObject.FindObjectOfType<ARRaycastManager>();

        r = myCam.ViewportPointToRay(screenPoint);
        logger.Log("R Cam succ");
        logger.Log("R: " + r);

        Physics.Raycast(r, out hit2);
        
        if (hit2.transform.gameObject)
            return r.direction;        
        else
            return new Vector3(0f,0f,0f);
        
    }
    void Update()
    {
        
        
        if (Input.touchCount == 2)
        {            
            if (!old)
            {
                logger.Log("about to Cross");                
                active = true;
                old = true;
                txtUI.text = "TO CROSSING";
            }
        }
        

        if (active)
        {
            logger.Log("before crossing");
            float targetAngle = transform.eulerAngles.y;
            Vector3 direction = calculateRayDir();
            if (direction.magnitude >= 0.1f)
            {

                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                ChickenRun.SetBool("Run", true);
            }
            chickenBody.velocity = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * chickenSpeed;
            //chickenBody.constraints = RigidbodyConstraints.None;


            //PAST CODE
            //Vector3 chickenMovement = new Vector3(0, 0, 2 ) * chickenSpeed * Time.deltaTime;
            //transform.Translate(chickenMovement, Space.Self);

            //logger.Log("RB: "+ transform.GetComponent<Rigidbody>());
            //transform.GetComponent<Rigidbody>().AddForce(direction * 100);            
            //transform.Rotate(Quaternion.LookRotation(direction).eulerAngles);
            //              logger.Log("dir RAY: " + calculateRayDir().normalized + " Angle: " + Quaternion.LookRotation(calculateRayDir()).eulerAngles);
            //ChickenRun.SetBool("Walk", true);
            logger.Log(" crossed");
        }
        else if (!active && old)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Destroy(transform.root.gameObject);
                logger.Log("chicken destroyed");
            }
        }
        
    }



    void OnTriggerEnter(Collider other)
    {

        logger.Log("into collider");
        if (other.gameObject.tag == "Despawner")
        {
            logger.Log("there's spawner");
            if (active)
            {
                Destroy(gameObject);
                //logger.Log("destroyed");
                ChickenSpawner chickenSpawner = road.GetComponent<ChickenSpawner>();
                chickenSpawner.expired = true;
                chickenSpawner.score++;                
                txtUI.text = "CHICKEN SCORE:" + chickenSpawner.score+"";
                logger.Log("SCORE IS:"+ chickenSpawner.score);
            }
            
        }
    }

    void OnCollisionEnter(Collision c)
    {
     

        // force is how forcefully we will push the player away from the enemy.
        float force = 1000;

        // If the object we hit is the enemy
        if (c.gameObject.tag == "Car")
        {

            if (active)
            {
                ChickenSpawner chickenSpawner = road.GetComponent<ChickenSpawner>();
                chickenSpawner.expired = true;
            }
            

            ChickenRun.SetBool("Walk", false);
            ChickenRun.SetBool("Run", true);
            active = false;
            chickenBody.useGravity = true;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = c.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            GetComponent<Rigidbody>().AddForce(dir * force);


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
                //logger.Log("Detected " + hit.transform.gameObject.name);
                if (hit.transform.gameObject.tag == "SpawnedObject")
                {
                    logger.Log("Conjuring the FUS ROH DAH");
                    Vector3 chickenMovement = new Vector3(0, 0, r.direction.x) * chickenSpeed * Time.deltaTime;
                    transform.Translate(chickenMovement, Space.Self);
                    transform.Rotate(new Vector3(0, Mathf.Sin(Time.time * 5) * 0.2f, 0));
                    ChickenRun.SetBool("Walk", true);

                    //chickSearch[0].transform.gameObject.GetComponent<Rigidbody>().AddForce(r.direction * 100);
                    //hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(r.direction*100);
                }
            }
        }
        */

    }
}
