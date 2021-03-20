using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominatorMark : MonoBehaviour
{
    private Move moveScript;
    private MyRay myRayScript;
    private OppositeRay opRayScript;
    // Start is called before the first frame update
    void Start()
    {
        moveScript = GameObject.Find("Relief").GetComponent<Move>();
        myRayScript = GameObject.Find("Main Camera").GetComponent<MyRay>();
        opRayScript = GameObject.Find("Relief/Line2").GetComponent<OppositeRay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveScript.dominator)
        {
            transform.position = (9 * myRayScript.init_pos + myRayScript.end_pos) / 10;
            // transform.position = myRayScript.init_pos;
        }
        else
        {
            transform.position = (9 * opRayScript.GetInitPos() + opRayScript.GetEndPos()) / 10;
        }
    }
}
