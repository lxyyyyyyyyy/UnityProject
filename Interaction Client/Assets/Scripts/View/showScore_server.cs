using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showScore_server : MonoBehaviour
{
    public string str;
    Text text;

    void Start()
    {
        text = GameObject.Find("ServerScore").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string high_precision = "0.000";
        string precision = "0.00";
        ViewQuality viewQualityScript = GameObject.Find("Main Camera").GetComponent<ViewQuality>();
        str = "Score: " + viewQualityScript.opScore.ToString(high_precision);
        str += " S: " + viewQualityScript.opS.ToString(precision);
        str += " S_over: " + viewQualityScript.opS_over.ToString(precision);
        str += " D: " + viewQualityScript.opD.ToString(precision);
        str += " Oc: " + viewQualityScript.opOc.ToString(precision);
        str += " Or: " + viewQualityScript.opOr.ToString(precision);
        text.text = str;
    }
}
