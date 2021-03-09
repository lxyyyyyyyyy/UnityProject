using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer line;
    // private Vector3[] pos;
    private MyRay myRayScript;

    void Start()
    {
        myRayScript = GameObject.Find("Main Camera").GetComponent<MyRay>();
        line = gameObject.GetComponent<LineRenderer>();

        // 设置材料的属性
        line.material = new Material(Shader.Find("Particles/Additive"));
        line.positionCount = 2; //　设置该线段由几个点组成

        // 设置线段起点宽度和终点宽度
        line.startWidth = 0.005f;
        line.endWidth = 0.005f;
    }

    private void Update()
    {
        line.startColor = myRayScript.line_color;
        line.endColor = myRayScript.line_color;

        line.SetPosition(0, myRayScript.init_pos);
        line.SetPosition(1, myRayScript.end_pos);
    }

}