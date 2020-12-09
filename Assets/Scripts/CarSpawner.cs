using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public int direction = 1;
    public float Timer = 2;
    GameObject carClone;
    public GameObject car;
    public AudioSource carSound;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            carClone = Instantiate(car, transform.position, transform.rotation) as GameObject;
            
            carClone.transform.parent = this.transform;
            if (direction == 1)
            {
                carClone.transform.localPosition += new Vector3(-2.5f, 1f, 0); 
            }
            else
            {
                carClone.transform.localPosition += new Vector3(-5f, 1.69f, -4); 
            }
            carClone.transform.Rotate(new Vector3(0, -90*direction, 0));
            
            Timer = 2f;
        }
        
    }
}
