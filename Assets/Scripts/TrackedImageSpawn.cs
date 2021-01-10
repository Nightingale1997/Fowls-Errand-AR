using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.UI;

public class TrackedImageSpawn : MonoBehaviour
{
    public GameObject btnSetMap;
    public GameObject txtFindChicken;
    private GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Canvas");

        btnSetMap = Canvas.transform.Find("btnSetRoad").gameObject;
        txtFindChicken = GameObject.Find("txtOnScreen");

        txtFindChicken.SetActive(false);
        btnSetMap.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
