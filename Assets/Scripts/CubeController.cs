using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class CubeController : MonoBehaviour
{
    // Hello Andrea

    public int oscPortNumber = 10000;
    public string oscDeviceUUID;

    private float movementX;
    private float movementY;

    private float accelX;
    private float accelY;
    private float accelZ;

    private float gravityX;
    private float gravityY;
    private float gravityZ;

    // Start is called before the first frame update
    void Start()
    {
        // TODO
        // Show IP on the screen

        OSCReceiver receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = oscPortNumber;
        receiver.Bind("/" + oscDeviceUUID +"/touch0", onTouch);
        receiver.Bind("/" + oscDeviceUUID +"/accel", onAcceleration);
        receiver.Bind("/" + oscDeviceUUID +"/gravity", onGravity);
        
    }

    public void onTouch(OSCMessage message)
    {
        movementX = (float)message.Values[0].DoubleValue;
        movementY = -(float)message.Values[1].DoubleValue;
        // Debug.Log("movementX = " + movementX.ToString("F6"));
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
        Debug.Log("gravityX: " + gravityX + " | gravityY: " + gravityY + " | gravityZ: " + gravityZ);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
