using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AutoMove : MonoBehaviour
{
    private FileIO fileIoScript;
    private List<string> trajectory;
    private int index = 0, Speed = 1, count = 0;

    private Move moveScript;
    private NetWorkAsServer networkScript;

    void Start()
    {
        fileIoScript = GetComponent<FileIO>();
        trajectory = fileIoScript.Readf();

        networkScript = GameObject.Find("Relief").GetComponent<NetWorkAsServer>();
        moveScript = GameObject.Find("Relief").GetComponent<Move>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (++count % Speed == 0 && networkScript.connected())
            MoveTo();
    }

    void MoveTo()
    {
        if (index >= trajectory.Count) return;

        string[] info = trajectory[index++].Split(',');

        // 物体移动
        moveScript.SetPosition(new Vector3(Convert.ToSingle(info[0]), Convert.ToSingle(info[1]), Convert.ToSingle(info[2])), true);
        moveScript.SetRotation(new Quaternion(Convert.ToSingle(info[3]), Convert.ToSingle(info[4]), Convert.ToSingle(info[5]), Convert.ToSingle(info[6])), true);
        // 主相机移动
        Camera.main.transform.position = new Vector3(Convert.ToSingle(info[7]), Convert.ToSingle(info[8]), Convert.ToSingle(info[9]));
        Camera.main.transform.rotation = new Quaternion(Convert.ToSingle(info[10]), Convert.ToSingle(info[11]), Convert.ToSingle(info[12]), Convert.ToSingle(info[13]));
        // client相机移动
        Vector3 op_pos = new Vector3(Convert.ToSingle(info[14]), Convert.ToSingle(info[15]), Convert.ToSingle(info[16]));
        Quaternion op_rot = new Quaternion(Convert.ToSingle(info[17]), Convert.ToSingle(info[18]), Convert.ToSingle(info[19]), Convert.ToSingle(info[20]));
        networkScript.SendMessageToClient("OpCamera" + Vec3toStr(op_pos) + "," + QuatoStr(op_rot) + ",");
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
