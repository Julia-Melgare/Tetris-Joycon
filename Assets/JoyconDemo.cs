using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyconDemo : MonoBehaviour
{

    private Joycon j;

    // Values made available via Unity
    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public Quaternion orientation;

    private bool _isMoving;

    void Start()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);
        // get the public Joycon object attached to the JoyconManager in scene
        j = JoyconManager.Instance.j;
    }

    IEnumerator TurnLeft()
    {
        _isMoving = true;

        Debug.Log("Rotate left");

        yield return new WaitForSeconds(0.5f);

        _isMoving = false;
    }

    IEnumerator TurnRight()
    {
        _isMoving = true;

        Debug.Log("Rotate right");

        yield return new WaitForSeconds(0.5f);

        _isMoving = false;
    }

    IEnumerator MoveLeft()
    {
        _isMoving = true;

        Debug.Log("Move left");

        yield return new WaitForSeconds(0.5f);

        _isMoving = false;
    }

    IEnumerator MoveRight()
    {
        _isMoving = true;

        Debug.Log("Move right");

        yield return new WaitForSeconds(0.5f);

        _isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        // make sure the Joycon only gets checked if attached
        if (j != null && j.state > Joycon.state_.ATTACHED)
        {
            // Gyro values: x, y, z axis values (in radians per second)
            gyro = j.GetGyro();

            // Accel values:  x, y, z axis values (in Gs)
            accel = j.GetAccel();

            stick = j.GetStick();

            orientation = j.GetVector();
            gameObject.transform.rotation = orientation;

            //Test motion detection
            //x-rotation -> turn tetrimino
            //z-rotation -> move tetrimino
            //tilt forward (rotation.Y and rotation.Z negative) -> make tetrimino go faster

            if (gyro.x <= -20 && !_isMoving)
            {
                Debug.Log(gyro.x);
                StartCoroutine(TurnLeft());               
            }


            if (gyro.x >= 20 && !_isMoving)
            {
                Debug.Log(gyro.x);
                StartCoroutine(TurnRight());
            }

            if (gyro.z <= -5 && !_isMoving)
            {
                Debug.Log(gyro.z);
                StartCoroutine(MoveLeft());
            }


            if (gyro.z >= 5 && !_isMoving)
            {
                Debug.Log(gyro.z);
                StartCoroutine(MoveRight());
            }

            if (orientation.w < 0.44)
            {
                Debug.Log("Tetrimino falling faster");
            }


            // GetButtonDown checks if a button has been pressed (not held)
            if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 pressed");
                // GetStick returns a 2-element vector with x/y joystick components
                Debug.Log(string.Format("Stick x: {0:N} Stick y: {1:N}", j.GetStick()[0], j.GetStick()[1]));

                // Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
                j.Recenter();
            }
            // GetButtonDown checks if a button has been released
            if (j.GetButtonUp(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 released");
            }
            // GetButtonDown checks if a button is currently down (pressed or held)
            if (j.GetButton(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 held");
            }

            if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                Debug.Log("Rumble");

                // Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:
                // https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

                j.SetRumble(160, 320, 0.6f, 200);

                // The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
                // (Useful for dynamically changing rumble values.)
                // Then call SetRumble(0,0,0) when you want to turn it off.
            }

            if (j.GetButtonDown(Joycon.Button.DPAD_LEFT))
            {
                j.Recenter();
            }

        }
    }
}
