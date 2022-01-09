using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class CubeController : MonoBehaviour
{

    float throwForce = 600;
    Vector3 objectPosition;
    float distance;

    public bool canHold = true;
    public GameObject item;
    public GameObject tempParent;
    public bool isHolding = false;

    public int frameCount = 0;

    public Material TouchedMaterial;
    public Material IdleMaterial;


    void Awake()
    {
        // This will cause framerate to drop to 60 frames per second.
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        frameCount++;

        // Check if holding
        if (isHolding == true)
        {
            // item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            // item.transform.SetParent(tempParent.transform);

            if (Input.GetMouseButtonDown(1))
            {
                //throw
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                isHolding = false;
            }
        }

    }

    // void FixedUpdate()
    // {
    //     frameCount++;
    //     
    // }

    public void hold()
    {
        isHolding = true;
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().detectCollisions = true;
        this.gameObject.transform.position = tempParent.transform.position;
        item.GetComponent<Renderer>().material = TouchedMaterial;
    }

    public void release()
    {
        isHolding = false;
        objectPosition = item.transform.position;
        item.GetComponent<Rigidbody>().detectCollisions = true;
        item.transform.SetParent(null);
        item.GetComponent<Rigidbody>().useGravity = true;
        item.transform.position = objectPosition;
        item.GetComponent<Renderer>().material = IdleMaterial;
    }
}
