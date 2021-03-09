using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeRay : MonoBehaviour
{
    private LineRenderer line;
    // private Vector3[] pos;
    private Vector3 init_pos;
    private Vector3 end_pos;
    private Color line_color;

    void Awake()
    {
        // pos = new Vector3[2]; 
    }

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        // 设置材料的属性
        line.material = new Material(Shader.Find("Particles/Additive"));
        line.positionCount = 2; //　设置该线段由几个点组成

        // 设置线段起点宽度和终点宽度
        line.startWidth = 0.005f;
        line.endWidth = 0.005f;

        line_color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetPosition(0, init_pos);
        line.SetPosition(1, end_pos);

        line.startColor = line_color;
        line.endColor = line_color;
    }

    public void SetInitPos(Vector3 _init_pos)
    {
        init_pos = _init_pos;
    }

    public void SetEndPos(Vector3 _end_pos)
    {
        end_pos = _end_pos;
    }

    public void SetColor(Color _line_color)
    {
        line_color = _line_color;
    }
}
