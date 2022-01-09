using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetCubeController : MonoBehaviour
{
    public GameObject[] ResettingObjects;
    public Vector3 ResetPosition;

    // Start is called before the first frame update
    void Start()
    {
        ResetPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            reset();
        }
    }

    void OnMouseDown()
    {
        reset();
    }

    void reset()
    {
        for (int i = 0; i < ResettingObjects.Length; i++)
        {
            ResettingObjects[i].transform.position = ResetPosition;
        }

    }
}
