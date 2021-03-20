using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ViewQuality : MonoBehaviour
{
    public float S, Oc, S_over, S_obj, S_obj_over;
    public float opS, opOc, opS_over;
    public double Alpha, Beta, D, Or, opD, opOr;
    public float score, opScore;
    private GameObject viewPoint, stdTarget, movingTarget;
    private NetWorkAsClient clientScript;
    private CalculAreaUsingDepth areaScript;
    private CalculateCompleteArea completeAreaScript;
    public Material frameMaterial, opFrameMaterial;

    // Start is called before the first frame update
    void Start()
    {
        viewPoint = GameObject.Find("Main Camera");
        stdTarget = GameObject.Find("StandardObject");
        movingTarget = GameObject.Find("Relief/target");
        clientScript = GameObject.Find("Relief").GetComponent<NetWorkAsClient>();
        // areaScript = GameObject.Find("GameObject").GetComponent<CalculateArea>();
        areaScript = GameObject.Find("GameObject").GetComponent<CalculAreaUsingDepth>();
        completeAreaScript = GameObject.Find("GameObject").GetComponent<CalculateCompleteArea>();
    }

    // Update is called once per frame
    void Update()
    {
        frameMaterial.SetFloat("_ServerScore", opScore);
        opFrameMaterial.SetFloat("_ServerScore", opScore);

        ViewScore();
    }

    void ViewScore()
    {
        Alpha = 0.55; Beta = 0.35; D = CalculD(); Or = CalculOr();
        S = areaScript.GetTarArea() / 5000.0f; S_obj = areaScript.GetObjArea() / 5000.0f; Oc = areaScript.GetCoveredArea() / 5000.0f; 
        S_over = completeAreaScript.GetTarOverArea() / 5000.0f; S_obj_over = completeAreaScript.GetObjOverArea() / 5000.0f;
        // double part1 = Alpha * (Math.Atan(S - 2 * Math.PI) + Math.PI / 2) / (S_over + 1);
        double part1 = Alpha * S / (S_over + 1);
        double part2 = 1 / (Math.Log((Oc / S_over + 1.1), 10) + 1);
        double part3 = Beta * Or / Math.Sqrt(Math.Pow(D, 2) + 1);
        double part4 = (1 - Alpha - Beta) * (S_obj / (S_obj_over + 1));

        // Debug.LogFormat("part1:{0}, part2:{1}, part3:{2}", part1, part2, part3);
        score = (float)(part1 * part2 + part3 + part4);

        SendInfo();
        
        frameMaterial.SetFloat("_ClientScore", score);
        opFrameMaterial.SetFloat("_ClientScore", score);
    }

    void SendInfo()
    {
        string precision = "0.000";

        string viewScoreInfo = "ViewScore" + score.ToString(precision);
        clientScript.SendMessageToServer(viewScoreInfo);

        string viewFactorInfo = "Area" + S.ToString(precision);
        viewFactorInfo += ",OverArea" + S_over.ToString(precision);
        viewFactorInfo += ",Dis" + D.ToString(precision);
        viewFactorInfo += ",Oc" + Oc.ToString(precision);
        viewFactorInfo += ",Or" + Or.ToString(precision);
        clientScript.SendMessageToServer(viewFactorInfo);
    }

    /*
     * Return: The distance between the stdTarget and the viewPoint
     */
    double CalculD()
    {
        Vector3 deltaPos = viewPoint.transform.position - stdTarget.transform.position;
        return Math.Sqrt(Math.Pow(deltaPos.x, 2) + Math.Pow(deltaPos.y, 2) + Math.Pow(deltaPos.z, 2));
    }

    /*
     * Return: The area of the stdTarget at viewPoint
     */
    int CalculS()
    {
        int area = 0;
        for (int x = 0; x < Screen.width; x++)
        {
            for (int y = 0; y < Screen.height; y++)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));  //camare2D.ScreenPointToRay (Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == stdTarget.name)
                {
                    ++area;
                }
            }
        }
        Debug.LogFormat("area: {0}", area);
        return area / 1000;
    }

    /*
     * Return: The Angle between the forward vector of the viewPoint and the vertical vector of the stdTarget
     */
    double CalculOr()
    {
        return SinVal(stdTarget.transform.forward, viewPoint.transform.forward);
    }

    /*
     * Return: The area of the stdTarget covered by the movingTarget in the current viewPoint
     */
    int CalculOc()
    {
        int covered_area = 0, front_layermask = LayerMask.NameToLayer(movingTarget.name), behind_layermask = LayerMask.NameToLayer(stdTarget.name);
        for (int x = 0; x < Screen.width; x++)
        {
            for (int y = 0; y < Screen.height; y++)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y, 0));  //camare2D.ScreenPointToRay (Input.mousePosition);
                RaycastHit hit;
                bool front_hit = (Physics.Raycast(ray, out hit) && hit.collider.gameObject.name == movingTarget.name);
                bool behind_hit = (Physics.Raycast(ray, out hit, 100.0f, 0 << front_layermask | 1 << behind_layermask) && hit.collider.gameObject.name == stdTarget.name);
                if (front_hit && behind_hit)
                {
                    ++covered_area;
                }
            }
        }
        // Debug.LogFormat("covered_area: {0}", covered_area);
        return covered_area / 1000;
    }

    double CosVal(Vector3 v1, Vector3 v2)
    {
        double v1_len = Math.Sqrt(v1.x * v1.x + v1.y * v1.y + v1.z * v1.z);
        double v2_len = Math.Sqrt(v2.x * v2.x + v2.y * v2.y + v2.z * v2.z);
        return Vector3.Dot(v1, v2) / (v1_len * v2_len);
    }

    double SinVal(Vector3 v1, Vector3 v2)
    {
        return Math.Sqrt(1 - Math.Pow(CosVal(v1, v2), 2));
    }

    public void SetOpScore(float _opScore)
    {
        opScore = _opScore;
    }

    public void SetOpFactor(float _opS, float _opS_over, float _opD, float _opOc, float _opOr)
    {
        opS = _opS; opS_over = _opS_over; opD = _opD; opOc = _opOc; opOr = _opOr;
    }
}
