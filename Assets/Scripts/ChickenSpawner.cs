using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    public float Timer = 2;
    GameObject chickenClone;
    public GameObject chicken;
    public bool expired = true;
    public int score = 0;
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
        if (expired)
        {
            expired = false;
            chickenClone = Instantiate(chicken, transform.position, transform.rotation) as GameObject;
            chickenClone.transform.localPosition += new Vector3(5, 0.15f, -5);
            road = GameObject.FindWithTag("Road");
            chickenClone.transform.localScale = road.transform.localScale*(3.5f);
            chickenClone.transform.Rotate(new Vector3(0, -90, 0));
            Timer = 1f;
        }
    }
}
