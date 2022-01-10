using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class OSCController : MonoBehaviour
{
    public int oscPortNumber = 10000;
    public string oscDeviceUUID;

    private float touchX;
    private float touchY;
    private bool touchPhase;

    public int frameCount;

    public float touchPhaseCount;

    // OSC Variables
    private float accelX;
    private float accelY;
    private float accelZ;

    private float gravityX;
    private float gravityY;
    private float gravityZ;

    private float quaternionX;
    private float quaternionY;
    private float quaternionZ;
    private float quaternionW;

    public GameObject Ball;
    public GameObject Hand;

    // Throw Variables
    private float tempAcceleration;
    private Vector3 throwLine;
    private Vector3 throwLocation;
    private float throwTimer;
    private float distanceXTemp;
    private float distanceXSum;


    void Awake()
    {
        // This will cause framerate to drop to 60 frames per second.
        Application.targetFrameRate = 60;

    }


    // Start is called before the first frame update
    void Start()
    {
        // TODO
        // Show ZigSim Data (IP, UUID) on the screen, so the user can connect to the game

        OSCReceiver receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = oscPortNumber;
        receiver.Bind("/" + oscDeviceUUID + "/gravity", onGravity);
        receiver.Bind("/" + oscDeviceUUID + "/touch0", onTouch);
        receiver.Bind("/" + oscDeviceUUID + "/quaternion", onQuaternion);
        throwLine = new Vector3(0, 0, 18f);
        Debug.Log(throwLine.z);
    }

    void Update()
    {
        // Check if ball is grabbed
        if (touchPhase)
        {
            // Debug.Log("Grab");
            Ball.GetComponent<CubeController>().hold();

            // Set quaternion for rotation
            float HandQuaternionX = quaternionX;
            float HandQuaternionY = quaternionY;
            float HandQuaternionZ = quaternionZ;
            float HandQuaternionW = quaternionW;
            Quaternion newQuaternion = new Quaternion(quaternionX, quaternionY, quaternionZ, quaternionW); ;
            Ball.transform.rotation = newQuaternion;
            Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


            //// Map OSC-Values
            // We use the gravity values because they are more stable and precise than the acceleration values from ZigSim.
            // gravityX will be mapped to Unity Z (forwards, backwards).
            // gravityY will be mapped to Unity X (left,right).
            // Unity Y with ZigSim is too imprecise unfortunately. At least with what we tried.
            float mappedGravityX = map(gravityX, -1f, 1f, 2.5f, 20f);
            float mappedGravityY = map(gravityY, -1f, 1f, 0, 5);
            float mappedGravityZ = map(-gravityZ, -1f, 1f, -5, 5);

            // Check if moving forward. If yes -> track that movement (distance & time), we need it for the throwing force calculation.
            if (mappedGravityX >= tempAcceleration)
            {
                throwTimer += Time.deltaTime;

                // Track the sum of distance in Unity X direction.
                float distanceXDelta;
                if(Mathf.Abs(mappedGravityY) > Mathf.Abs(distanceXTemp)) 
                {
                    distanceXDelta = Mathf.Abs(mappedGravityY) - Mathf.Abs(distanceXTemp);
                } else 
                {
                    distanceXDelta = Mathf.Abs(distanceXTemp) - Mathf.Abs(mappedGravityY);
                }
                distanceXSum += distanceXDelta;
            }
            else
            {
                // Else -> reset values.
                throwLocation = Hand.transform.position;
                throwTimer = 0;
                distanceXSum = 0;
            }
            tempAcceleration = mappedGravityX;
            distanceXTemp = mappedGravityY;

            float HandPositionX = mappedGravityY;            
            float HandPositionY = Hand.transform.position.y;
            float HandPositionZ = -mappedGravityX;
            Vector3 HandPosition = new Vector3(HandPositionX, HandPositionY, HandPositionZ);
            Hand.transform.position = HandPosition;

            if (Hand.transform.position.z <= (-throwLine.z + distanceXSum))
            {
                Debug.Log("throw");
                float distance = (throwLine.z + distanceXSum) - -throwLocation.z;
                touchPhase = false;
                float throwPower = 1.5f;
                float throwForce = Mathf.Pow((distance / throwTimer),throwPower) * 0.3f;
                Debug.Log("Throwing Force: " + throwForce);
                Debug.Log("HandRotationX: " + Hand.transform.eulerAngles.x);
                Debug.Log("ThrowX: " + Hand.transform.eulerAngles.x * throwForce * 4);
                Ball.GetComponent<Rigidbody>().AddForce(new Vector3(Hand.transform.position.x * throwForce * 2, Hand.transform.position.y, Hand.transform.position.z * throwForce));
                Ball.GetComponent<CubeController>().release();
            }

            
        }
        else
        {
            // Debug.Log("Release");
            touchPhaseCount = 0;
            Ball.GetComponent<CubeController>().release();
        }

    }

    //// OSC-Receiver-Functions
    public void onTouch(OSCMessage message)
    {
        // Debug.Log("touch");
        touchPhase = !touchPhase;
        touchX = (float)message.Values[0].DoubleValue;
        touchY = -(float)message.Values[1].DoubleValue;
    }


    public void onGravity(OSCMessage message)
    {
        gravityX = (float)message.Values[0].FloatValue;
        gravityY = -(float)message.Values[1].FloatValue;
        gravityZ = (float)message.Values[2].FloatValue;
        // Debug.Log("gravityX: " + gravityX);

        // Round to prevent imprecision from ZigSim
        gravityX = Mathf.Round(gravityX * 100f) / 100f;
        gravityY = Mathf.Round(gravityY * 100f) / 100f;
        gravityZ = Mathf.Round(gravityZ * 100f) / 100f;
    }

    public void onQuaternion(OSCMessage message)
    {
        // Debug.Log(message);
        quaternionX = (float)message.Values[0].FloatValue;
        quaternionY = (float)message.Values[1].FloatValue;
        quaternionZ = (float)message.Values[2].FloatValue;
        quaternionW = (float)message.Values[3].FloatValue;
    }

    //// Helper Classes

    // Maps a value from one arbitrary range to another arbitrary range
    public static float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
}
