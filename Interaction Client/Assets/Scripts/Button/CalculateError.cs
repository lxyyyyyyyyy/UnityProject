using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class CalculateError : MonoBehaviour
{
    public UnityEvent event2;
    private Button btn;
    private Vector3 standardLoc;
    private Vector3 standardRot;
    private Vector3 standardXaxis;
    private Vector3 standardYaxis;
    private Vector3 standardZaxis;
    private GameObject target;
    private float locError, rotError;


    void Awake()
    {
        btn = GetComponent<Button>();
        event2.AddListener(GetError);
    }

    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            event2.Invoke();
        });

        standardLoc = new Vector3(250.913f, 232.436f, -61.923f);
        standardRot = new Vector3(270f, 0.0f, -180.0f);
        standardXaxis = new Vector3(0.0f, 1.0f, 0.0f);
        standardYaxis = new Vector3(0.0f, 0.0f, 1.0f);
        standardZaxis = new Vector3(-1.0f, 0.0f, 0.0f);
        target = GameObject.Find("Relief");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetError()
    {
        Vector3 currentLoc = target.transform.position;
        Vector3 deltaLoc = currentLoc - standardLoc;
        float curLocError = Math.Abs(deltaLoc.x) + Math.Abs(deltaLoc.y) + Math.Abs(deltaLoc.z);

        float xAxisCosVal = CosVal(standardXaxis, target.transform.forward);
        float yAxisCosVal = CosVal(standardYaxis, target.transform.up);
        float zAxisCosVal = CosVal(standardZaxis, target.transform.right);
        float curRotError = 3 - xAxisCosVal - yAxisCosVal - zAxisCosVal;

        Debug.LogFormat("Location Error: {0}", curLocError);
        Debug.LogFormat("Rotate_X Error: {0}", 1 - xAxisCosVal);
        Debug.LogFormat("Rotate_Y Error: {0}", 1 - yAxisCosVal);
        Debug.LogFormat("Rotate_Z Error: {0}", 1 - zAxisCosVal);
        Debug.LogFormat("Rotate Error: {0}", curRotError);
    }

    float CosVal(Vector3 v1, Vector3 v2)
    {
        float v1_len = (float)Math.Sqrt(v1.x * v1.x + v1.y * v1.y + v1.z * v1.z);
        float v2_len = (float)Math.Sqrt(v2.x * v2.x + v2.y * v2.y + v2.z * v2.z);
        return Vector3.Dot(v1, v2) / (v1_len * v2_len);
    }
}
