using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChicken : MonoBehaviour
{
    public float chickenSpeed = 3f;
    public bool active = false;
    public bool old = false;
    private Animator ChickenRun;
    private Rigidbody chickenBody;
    private GameObject road;
    public float timer = 2f;
    // Start is called before the first frame update
    void Start()
    {
        road = GameObject.FindWithTag("Road");
        ChickenRun = gameObject.GetComponent<Animator>();
        chickenBody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.R))
        {
            if (!old)
            {
                active = true;
                old = true;
            }
        }

        if (active)
        {
            Vector3 chickenMovement = new Vector3(0, 0, 2) * chickenSpeed * Time.deltaTime;
            transform.Translate(chickenMovement, Space.Self);
            transform.Rotate(new Vector3(0, Mathf.Sin(Time.time * 5) * 0.2f, 0));
            ChickenRun.SetBool("Walk", true);
        }
        else if (!active && old)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                Destroy(transform.root.gameObject);
            }
        }
        
    }


    void OnTriggerEnter(Collider other)
    {

        //Debug.Log("test");
        if (other.gameObject.tag == "Despawner")
        {
            if (active)
            {
                Destroy(gameObject);
                ChickenSpawner chickenSpawner = road.GetComponent<ChickenSpawner>();
                chickenSpawner.expired = true;
                chickenSpawner.score++;
                Debug.Log(chickenSpawner.score);
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


    }
}
