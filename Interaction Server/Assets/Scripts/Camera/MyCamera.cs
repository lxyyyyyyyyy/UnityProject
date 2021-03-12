using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 origin_position;
    private Quaternion origin_rotation;
    private NetWorkAsServer serverScript;

    void Start()
    {
        origin_position = new Vector3(0, 0, 0);
        origin_rotation = new Quaternion(0, 0, 0, 0);
        serverScript = GameObject.Find("Relief").GetComponent<NetWorkAsServer>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (transform.position != origin_position || transform.rotation != origin_rotation)
        {
            origin_position = transform.position;
            origin_rotation = transform.rotation;
            SendInfo();
        }
    }

    public void SendInfo()
    {
        serverScript.SendMessageToClient("Camera" + Vec3toStr(origin_position) + "," + QuatoStr(origin_rotation) + ",");
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
