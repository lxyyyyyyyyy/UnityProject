using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryStore : MonoBehaviour
{
    private FileIO fileIoScript;
    private List<string> trajectory;
    private GameObject clientCamera, Relief;
    private Vector3 origin_pos, origin_cam_pos, origin_opCam_pos;
    private Quaternion origin_rot, origin_cam_rot, origin_opCam_rot;

    // Start is called before the first frame update
    void Start()
    {
        clientCamera = GameObject.Find("ClientCamera");
        Relief = GameObject.Find("Relief");
        fileIoScript = GetComponent<FileIO>();
        trajectory = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        StorePoint();
    }

    public void StoreTrajectory()
    {
        fileIoScript.Writef(trajectory);
    }

    void StorePoint()
    {
        if (Relief.transform.position != origin_pos || Relief.transform.rotation != origin_rot ||
            Camera.main.transform.position != origin_cam_pos || Camera.main.transform.rotation != origin_cam_rot ||
            clientCamera.transform.position != origin_opCam_pos || clientCamera.transform.rotation != origin_opCam_rot)
        {
            origin_pos = Relief.transform.position;
            origin_rot = Relief.transform.rotation;
            origin_cam_pos = Camera.main.transform.position;
            origin_cam_rot = Camera.main.transform.rotation;
            origin_opCam_pos = clientCamera.transform.position;
            origin_opCam_rot = clientCamera.transform.rotation;

            trajectory.Add(Vec3toStr(origin_pos) + "," + QuatoStr(origin_rot) + "," + 
                Vec3toStr(origin_cam_pos) + "," + QuatoStr(origin_cam_rot) + "," + 
                Vec3toStr(origin_opCam_pos) + "," + QuatoStr(origin_opCam_rot));
        }
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
