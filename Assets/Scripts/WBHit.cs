using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WBHit : MonoBehaviour
{
    public Rigidbody chickenThrown;
    public GameObject rope;
    float speed = 2;
    float mass = 6;
    
    public AudioSource WreckHit;

    // Start is called before the first frame update
    void Start()
    {
        //chickenThrown = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //moving the WBall -- ROPE
        rope.transform.Rotate((Mathf.Cos(Time.time)), 0.0f, 0.0f, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Toon Chicken")
        {
            WreckHit.Play(0);
        }

    }
        void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Toon Chicken")
        {

            
            chickenThrown.velocity = new Vector3((-1* speed * mass), speed * mass * Mathf.Sin(rope.transform.rotation.x), chickenThrown.transform.rotation.y);
            //vector= -1*mass*gravity*sin(angle)
            //9 is the initial center position of the 
            Debug.Log(chickenThrown.velocity);
            Debug.Log(rope.transform.rotation.x);
            //chickenThrown.AddForce(5f, 5f, 5f);
            //chickenThrown.constraints = RigidbodyConstraints.None; //let the chicken fall on its sides and face

            //chickenThrown.velocity = new Vector3(5f, 5f, 5f);
            //chickenThrown.AddForce(5f, 5f, 5f);

            //chickenThrown.constraints = RigidbodyConstraints.FreezeRotationX;
            //chickenThrown.constraints = RigidbodyConstraints.FreezeRotationZ;
        }

    }
}
