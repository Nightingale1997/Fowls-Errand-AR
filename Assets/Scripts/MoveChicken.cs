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
    public float runTimer = 6f;
    public Camera myCam;
    private ARRaycastManager rays;
    //UI
    //public GameObject btnPushChicken;
    private Button pushChicken;
    //public GameObject txtOnScreen;
    private Text txtUI;
    private Text txtScore;
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
        txtScore = GameObject.Find("txtScoreNumber").GetComponent<Text>();
        logger.Log(" Cross-UI Successful");

        
        
    }


    public Vector3 calculateRayDir()
    {
        Vector3 screenPoint = new Vector3(0.5f, 0.5f, 0f);
        RaycastHit hit2;
        Ray r;
  
        myCam = GameObject.Find("AR Camera").gameObject.GetComponent<Camera>();
        rays = GameObject.FindObjectOfType<ARRaycastManager>();

        r = myCam.ViewportPointToRay(screenPoint);
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
                //txtUI.text = "TO CROSSING";
            }
        }
        float targetAngle = transform.eulerAngles.y;
        Vector3 direction = calculateRayDir();
        targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (active)
        {
            logger.Log("before crossing");
            

            if (direction.magnitude >= 0.1f)
            {



                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                ChickenRun.SetBool("Run", true);
            }
            chickenBody.velocity = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * chickenSpeed;
            //chickenBody.constraints = RigidbodyConstraints.None;

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Destroy(transform.root.gameObject);
                logger.Log("chicken destroyed");
                active = false;
                ChickenSpawner chickenSpawner = road.GetComponent<ChickenSpawner>();
                chickenSpawner.expired = true;
            }
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
        else if (!active && !old)
        {
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
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
                txtScore = GameObject.Find("txtScoreNumber").GetComponent<Text>();
                txtScore.text = " "+chickenSpawner.score+"";
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
                chickenSpawner.score = 0;
                txtScore = GameObject.Find("txtScoreNumber").GetComponent<Text>();
                txtScore.text = " " + chickenSpawner.score + "";
                chickenSpawner.expired = true;
            }
            

            ChickenRun.SetBool("Turn Head", false);
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



    }
}
