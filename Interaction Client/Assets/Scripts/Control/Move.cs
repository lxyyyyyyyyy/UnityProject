using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // private float acceleration;
    public bool dominator;

    private float Speed;
    private float rotateSpeed;
    private KeyCode[] inputKeys;
    private KeyCode[] rotateInputKeys;
    private Vector3[] directionForkeys;

    private Vector3 position;
    private Quaternion rotation;
    private bool change;

    private NetWorkAsClient networkScript;

    // Start is called before the first frame update
    void Start()
    {
        dominator = true;
        Speed = 0.3f;
        rotateSpeed = 0.3f;

        inputKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.Q, KeyCode.E };
        rotateInputKeys = new KeyCode[] { KeyCode.I, KeyCode.J, KeyCode.L, KeyCode.K, KeyCode.U, KeyCode.O };
        directionForkeys = new Vector3[] { Vector3.forward, Vector3.left, Vector3.right, Vector3.back, Vector3.up, Vector3.down };

        networkScript = GameObject.Find("Relief").GetComponent<NetWorkAsClient>();
    }

    void FixedUpdate()
    {
        if (change)
        {
            transform.position = position;
            transform.rotation = rotation;
            change = false;
        }

        if (!dominator) return;
        if (!GameObject.Find("Main Camera").GetComponent<MyRay>().detected) return;
        if (!GameObject.Find("Main Camera").GetComponent<MyRay>().oppositeDetected) return;

        bool send = false;
        // 平移
        for (int i = 0; i < inputKeys.Length; i++)
        {
            // var key = inputKeys[i];
            if (Input.GetKey(inputKeys[i]))
            {
                transform.Translate(directionForkeys[i] * Time.fixedDeltaTime * Speed, Space.World);
                send = true;
            }
        }

        // 旋转
        for (int i = 0; i < inputKeys.Length; i++)
        {
            if (Input.GetKey(rotateInputKeys[i]))
            {
                transform.Rotate(directionForkeys[i] * rotateSpeed);
                send = true;
            }
        }

        if (send) SendPoint();
    }

    public void SetPosition(Vector3 _position)
    {
        position = _position;
        change = true;
    }

    public void SetRotation(Quaternion _rotation)
    {
        rotation = _rotation;
        change = true;
    }

    void SendPoint()
    {
        networkScript.SendMessageToServer("Target " + Vec3toStr(transform.position) + "," + QuatoStr(transform.rotation) + ",");
    }

    string Vec3toStr(Vector3 _vec)
    {
        string precision = "0.000";
        return _vec.x.ToString(precision) + "," + _vec.y.ToString(precision) + "," + _vec.z.ToString(precision);
    }

    string QuatoStr(Quaternion _q)
    {
        string precision = "0.000";
        return _q.x.ToString(precision) + "," + _q.y.ToString(precision) + "," + _q.z.ToString(precision) + "," + _q.w.ToString(precision);
    }
}