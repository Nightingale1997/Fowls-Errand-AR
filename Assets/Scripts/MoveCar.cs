using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCar : MonoBehaviour
{
    private Rigidbody chickenThrown;
    
    //CAR SPEED !!!!
    public float carSpeed = 0f;
    //float mass = 10;
    public AudioSource impactCar;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 carMovement = new Vector3(10, 0, 0) * carSpeed * Time.deltaTime;
        transform.Translate(carMovement, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("test");
        if (other.gameObject.tag == "Despawner")
        {
            Destroy(gameObject);
            //or gameObject.SetActive(false);
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.name == "Toon Chicken")
        {
            chickenThrown = other.gameObject.GetComponent<Rigidbody>();
            chickenThrown.velocity = transform.rotation * new Vector3(5f, 5f, 5f);
            impactCar.Play(0);
        }

            
        
    
    }
}
