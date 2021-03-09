using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;
using System;

public class TestC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*
        string msg = "#Ray252.873,232.215,-62.969,249.353,228.664,-63.041252.873,232.215,-62.967,249.353,228.664,-63.039";
        string oneDimensional = @"-?\d{1,}\.\d{3}";
        string threeDimensional = oneDimensional + "," + oneDimensional + "," + oneDimensional;
        Regex posPattern = new Regex(oneDimensional);
        Regex regPattern = new Regex(@"#Ray");
        if (regPattern.IsMatch(msg))
        {
            foreach (Match match in posPattern.Matches(msg))
            {
                Debug.Log(match.Value);
            }
        }
        */
        string msg = "S38,D2.481,Oc0,Or0.717";
        Regex ViewFactorPattern = new Regex(@"S(.+),D(.+),Oc(.+),Or(.+)");
        Regex ViewScorePattern = new Regex(@"ViewScore(.+)");
        if(ViewFactorPattern.IsMatch(msg))
        {
            Match _match = ViewScorePattern.Match(msg);
            Debug.Log(_match.Groups[1].Value);
            Debug.Log(_match.Groups[2].Value);
            Debug.Log(_match.Groups[3].Value);
            Debug.Log(_match.Groups[4].Value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
