using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeRay : MonoBehaviour
{
    private LineRenderer line;
    // private Vector3[] pos;
    private Vector3 init_pos, end_pos;
    private Color line_color;
    private bool state;

    public bool detected;

    private MyRay oppositeRayScript;

    void Awake()
    {
        // pos = new Vector3[2]; 
    }

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Particles/Additive"));
        line.positionCount = 2; //　设置该线段由几个点组成
        line.startWidth = 0.005f;
        line.endWidth = 0.005f;
        line_color = Color.red;

        state = false;

        oppositeRayScript = GameObject.Find("Relief/Line1").GetComponent<MyRay>();
    }

    // Update is called once per frame
    void Update()
    {
        SetColor();
        DrawLine();
    }

    void DrawLine()
    {
        line.SetPosition(0, init_pos);
        line.SetPosition(1, end_pos);

        line.startColor = line_color;
        line.endColor = line_color;

        line.enabled = state;
    }

    public void SetColor()
    {
        if (detected && !oppositeRayScript.detected)
            line_color = Color.blue;

        if (detected && oppositeRayScript.detected)
            line_color = Color.green;
    }

    public void SetInitPos(Vector3 _init_pos)
    {
        init_pos = _init_pos;
    }

    public void SetEndPos(Vector3 _end_pos)
    {
        end_pos = _end_pos;
    }

    public void SetState(bool _state)
    {
        state = _state;
    }

    public Vector3 GetInitPos()
    {
        return init_pos;
    }

    public Vector3 GetEndPos()
    {
        return end_pos;
    }
}
