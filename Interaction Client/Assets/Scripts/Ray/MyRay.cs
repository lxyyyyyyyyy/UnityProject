using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRay : MonoBehaviour
{
    public Vector3 init_pos, end_pos, direction;
    public Color line_color;
    public bool detected, oppositeDetected;
    private GameObject target;
    private NetWorkAsClient clientScript;
    private OppositeRay oppositeRayScript;
    private Vector3 origin_init_pos, origin_end_pos;

    void Start()
    {
        detected = false; 
        oppositeDetected = false;
        direction = gameObject.transform.forward;
        init_pos = gameObject.transform.position + new Vector3(0, -0.1f, 0);
        end_pos = init_pos + 5 * direction;
        origin_init_pos = new Vector3(0, 0, 0);
        origin_end_pos = new Vector3(0, 0, 0);
        line_color = Color.red;
        target = GameObject.Find("Relief");
        clientScript = target.GetComponent<NetWorkAsClient>();
        oppositeRayScript = GameObject.Find("Relief/Line2").GetComponent<OppositeRay>();
    }

    // Update is called once per frame
    void Update()
    {
        init_pos = gameObject.transform.position + new Vector3(0, -0.1f, 0);
        end_pos = detected ? (target.transform.position + target.transform.forward * 0.1f) : (init_pos + 5 * direction);
        if (origin_init_pos != init_pos || origin_end_pos != end_pos)
        {
            clientScript.SendMessageToServer("Ray" + Vec3toStr(init_pos) + "," + Vec3toStr(end_pos) + ",");
            origin_init_pos = init_pos; origin_end_pos = end_pos;
        }
            

        if (detected && !oppositeDetected) line_color = Color.blue;
        if (detected && oppositeDetected) line_color = Color.green;
        if (detected)
            return;

        RaycastHit hit;
        if (!Physics.Raycast(init_pos, direction, out hit))
            return;
        if (hit.collider.gameObject.name == "target")
        {
            detected = true;
            // end_pos = hit.point;
            clientScript.SendMessageToServer("Ready");
            if (oppositeDetected)
                oppositeRayScript.SetColor(Color.green);
        }
    }

    string Vec3toStr(Vector3 _vec)
    {
        string precision = "0.000";
        return _vec.x.ToString(precision) + "," + _vec.y.ToString(precision) + "," + _vec.z.ToString(precision);
    }
}
