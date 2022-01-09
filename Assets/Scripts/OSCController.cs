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
    // To Do:
    // Record accelZ as long as it stays positive.
    // End when touch has ended
    private bool recordAcceleration;
    private float tempAcceleration;
    private Vector3 throwLine;
    private Vector3 throwLocation;
    private float throwTimer;


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
        // receiver.Bind("/" + oscDeviceUUID + "/accel", onAcceleration);
        receiver.Bind("/" + oscDeviceUUID + "/quaternion", onQuaternion);
        throwLine = new Vector3(0, 0, 18f);
        Debug.Log(throwLine.z);
    }

    // OSC-Functions
    public void onTouch(OSCMessage message)
    {
        touchPhase = !touchPhase;
        touchX = (float)message.Values[0].DoubleValue;
        touchY = -(float)message.Values[1].DoubleValue;
        // Debug.Log("touch");
    }

    void Update()
    {


        // Check if Released
        if (touchPhase)
        {
            // Debug.Log("Grab");
            Ball.GetComponent<CubeController>().hold();

            float HandQuaternionX = quaternionX;
            float HandQuaternionY = quaternionY;
            float HandQuaternionZ = quaternionZ;
            float HandQuaternionW = quaternionW;
            Quaternion newQuaternion = new Quaternion(quaternionX, quaternionY, quaternionZ, quaternionW); ;
            Ball.transform.rotation = newQuaternion;

            Ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;


            float mappedGravityX = map(gravityX, -1f, 1f, 2.5f, 20f);

            if (mappedGravityX >= tempAcceleration)
            {
                recordAcceleration = true;
                throwTimer += Time.deltaTime;

            }
            else
            {
                recordAcceleration = false;
                throwLocation = Hand.transform.position;
                // if(throwTimer != 0) Debug.Log(throwTimer);
                throwTimer = 0;
            }
            tempAcceleration = mappedGravityX;
            // Debug.Log(throwTimer);

            if (recordAcceleration)
            {

            }

            float HandPositionX = Hand.transform.position.x;
            // accelY is too unstable
            float HandPositionY = Hand.transform.position.y;
            float HandPositionZ = -mappedGravityX;
            Vector3 HandPosition = new Vector3(HandPositionX, HandPositionY, HandPositionZ);
            Hand.transform.position = HandPosition;

            // Debug.Log(Hand.transform.position.z);
            if (Hand.transform.position.z <= -throwLine.z)
            {
                Debug.Log("throw");
                float distanceZ = throwLine.z - -throwLocation.z;
                touchPhase = false;
                float throwPower = 2;
                float throwForce = Mathf.Pow((distanceZ / throwTimer),throwPower) * 0.1f;
                Debug.Log(throwForce);
                Ball.GetComponent<Rigidbody>().AddForce(new Vector3(Hand.transform.position.x, Hand.transform.position.y, Hand.transform.position.z * throwForce));
                Ball.GetComponent<CubeController>().release();
            }

            
        }
        else
        {
            touchPhaseCount = 0;
            Ball.GetComponent<CubeController>().release();
            // Debug.Log("Release");
        }

    }



    protected void onAcceleration(OSCMessage message)
    {
        accelX = (float)message.Values[0].FloatValue;
        accelY = (float)message.Values[1].FloatValue;
        accelZ = (float)message.Values[2].FloatValue;
        // Debug.Log("AccelX: " + accelX + " | AccelY: " + accelY + " | AccelZ: " + accelZ);


    }

    public void onGravity(OSCMessage message)
    {
        gravityX = (float)message.Values[0].FloatValue;
        gravityY = -(float)message.Values[1].FloatValue;
        gravityZ = (float)message.Values[2].FloatValue;
        // Debug.Log("gravityX: " + gravityX);

        // Round to prevent unprecision from ZigSim
        gravityX = Mathf.Round(gravityX * 100f) / 100f;
    }

    public void onQuaternion(OSCMessage message)
    {
        // Debug.Log(message);
        quaternionX = (float)message.Values[0].FloatValue;
        quaternionY = (float)message.Values[1].FloatValue;
        quaternionZ = (float)message.Values[2].FloatValue;
        quaternionW = (float)message.Values[3].FloatValue;
    }

    // Helper Classes
    // Maps a value from one arbitrary range to another arbitrary range
    public static float map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
    {
        return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
    }
}
