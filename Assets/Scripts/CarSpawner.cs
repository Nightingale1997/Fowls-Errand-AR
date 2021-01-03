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
    private GameObject road;
    // Start is called before the first frame update
    void Start()
    {
        road = GameObject.FindWithTag("Road");

    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            carClone = Instantiate(car, transform.position, transform.rotation) as GameObject;
            road = GameObject.FindWithTag("Road");
            carClone.transform.localScale = road.transform.localScale*100;
            carClone.transform.parent = this.transform;
            if (direction == 1)
            {
                carClone.transform.localPosition += new Vector3(-2.5f, 9f, 0);
            }
            else
            {
                carClone.transform.localPosition += new Vector3(-5f, 16f, -4);

            }
            carClone.transform.Rotate(new Vector3(0, -90*direction, 0));
            Timer = Random.Range(0.5f, 2.5f);

        }
        
    }
}
