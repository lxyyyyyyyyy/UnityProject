using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showScore_client : MonoBehaviour
{
    public string str;
    Text text;

    void Start()
    {
        text = GameObject.Find("ClientScore").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string high_precision = "0.000";
        string precision = "0.00";
        ViewQuality viewQualityScript = GameObject.Find("Main Camera").GetComponent<ViewQuality>();
        str = "Score: " + viewQualityScript.score.ToString(high_precision);
        str += " S: " + viewQualityScript.S.ToString(precision);
        str += " S_over: " + viewQualityScript.S_over.ToString(precision);
        str += " D: " + viewQualityScript.D.ToString(precision);
        str += " Oc: " + viewQualityScript.Oc.ToString(precision);
        str += " Or: " + viewQualityScript.Or.ToString(precision);
        text.text = str;
    }
}
