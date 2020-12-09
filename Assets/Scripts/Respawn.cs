using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "Water")
        {
            transform.position = new Vector3(14.06f, 0.3f, -34.1f);            
        }
    }
}
