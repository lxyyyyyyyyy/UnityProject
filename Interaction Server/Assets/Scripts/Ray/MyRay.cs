using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRay : MonoBehaviour
{
    public Vector3 init_pos, end_pos, direction;
    public Color line_color;

    private int layer;
    public bool detected;

    private LineRenderer line;

    private GameObject target;
    private NetWorkAsServer serverScript;
    private OppositeRay oppositeRayScript;
    private Vector3 origin_init_pos, origin_end_pos;

    void Start()
    {

        LineInit();

        detected = false;
        direction = GameObject.Find("Relief").transform.position - init_pos;

        origin_init_pos = new Vector3(0, 0, 0);
        origin_end_pos = new Vector3(0, 0, 0);
        
        line_color = Color.red;

        layer = LayerMask.NameToLayer("Relief");

        target = GameObject.Find("Relief");
        serverScript = target.GetComponent<NetWorkAsServer>();
        oppositeRayScript = GameObject.Find("Relief/Line2").GetComponent<OppositeRay>();
    }

    // Update is called once per frame
    void Update()
    {
        init_pos = Camera.main.transform.position;
        end_pos = detected ? (target.transform.position + target.transform.forward * 0.1f) : (init_pos + 5 * direction);

        SendInfo();
        Connect();
        SetColor();
        DrawLine();
    }

    void SetColor()
    {
        if (detected && !oppositeRayScript.detected) 
            line_color = Color.blue;

        if (detected && oppositeRayScript.detected) 
            line_color = Color.green;
    }

    void Connect()
    {
        if (detected)
            return;

        RaycastHit hit;
        if (!Physics.Raycast(init_pos, direction, out hit, 100.0f, 1 << layer))
            return;

        detected = true;
        serverScript.SendMessageToClient("Ready");
    }

    void DisConnect()
    {
        // 按钮
        detected = false;
        serverScript.SendMessageToClient("DisConnect");
    }

    void LineInit()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Particles/Additive"));
        line.positionCount = 2; //　设置该线段由几个点组成
        line.startWidth = 0.005f;
        line.endWidth = 0.005f;
    }

    void DrawLine()
    {
        line.startColor = line_color;
        line.endColor = line_color;

        line.SetPosition(0, init_pos);
        line.SetPosition(1, end_pos);
    }
    
    public void SendInfo()
    {
        if (origin_init_pos != init_pos || origin_end_pos != end_pos)
        {
            serverScript.SendMessageToClient("Ray" + Vec3toStr(init_pos) + "," + Vec3toStr(end_pos) + ",");
            origin_init_pos = init_pos; origin_end_pos = end_pos;
        }    
    }

    string Vec3toStr(Vector3 _vec)
    {
        string precision = "0.000";
        return _vec.x.ToString(precision) + "," + _vec.y.ToString(precision) + "," + _vec.z.ToString(precision);
    }
}
