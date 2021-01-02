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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (expired)
        {
            expired = false;
            chickenClone = Instantiate(chicken, new Vector3(38, 13, -11), transform.rotation) as GameObject;
            chickenClone.transform.Rotate(new Vector3(0, -90, 0));
            Timer = 1f;
        }
    }
}
