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
    private float touchPhaseTemp;

    private float accelX;
    private float accelY;
    private float accelZ;

    private float gravityX;
    private float gravityY;
    private float gravityZ;

    public GameObject Ball;
    public GameObject Hand;


    // Start is called before the first frame update
    void Start()
    {
        // TODO
        // Show ZigSim Data (IP, UUID) on the screen, so the user can connect to the game

        OSCReceiver receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = oscPortNumber;
        receiver.Bind("/" + oscDeviceUUID +"/touch0", onTouch);
        receiver.Bind("/" + oscDeviceUUID +"/accel", onAcceleration);
        receiver.Bind("/" + oscDeviceUUID +"/gravity", onGravity);
    }

    // OSC-Functions
    public void onTouch(OSCMessage message)
    {
        touchPhase = true;        
        touchX = (float)message.Values[0].DoubleValue;
        touchY = -(float)message.Values[1].DoubleValue;
        Debug.Log("touchX = " + touchX.ToString("F6"));
    }

    // Update is called once per frame
    void Update()
    {
        // #Update
        // Check if Released by checking if touchX/Y changed
        if(touchPhase)
        {
            Ball.GetComponent<CubeController>().hold();
            Debug.Log("Grab");
        } else {
            Ball.GetComponent<CubeController>().isHolding = false;
            // Debug.Log("Loose");
        }
        touchPhase = false;
    }

     

    protected void onAcceleration(OSCMessage message)
    {
        accelX = (float)message.Values[0].FloatValue;
        accelY = -(float)message.Values[1].FloatValue;
        accelZ = (float)message.Values[2].FloatValue;
        // Debug.Log("AccelX: " + accelX + " | AccelY: " + accelY + " | AccelZ: " + accelZ);
    }

    public void onGravity(OSCMessage message)
    {
        gravityX = (float)message.Values[0].FloatValue;
        gravityY = -(float)message.Values[1].FloatValue;
        gravityZ = (float)message.Values[2].FloatValue;
        // Debug.Log("gravityX: " + gravityX + " | gravityY: " + gravityY + " | gravityZ: " + gravityZ);
    }
}
