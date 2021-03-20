using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDominatorScript : MonoBehaviour
{
    private bool originDominator;
    private GameObject serverCam, clientCam, relief;
    private NetWorkAsServer networkScript;

    private float serverScore, clientScore;
    private Queue<bool> domList;
    private int filterBound = 60;

    void Start()
    {
        serverCam = GameObject.Find("Main Camera");
        clientCam = GameObject.Find("ClientCamera");
        relief = GameObject.Find("Relief");
        
        networkScript = GameObject.Find("Relief").GetComponent<NetWorkAsServer>();
        originDominator = relief.GetComponent<Move>().dominator;

        domList = new Queue<bool>(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (domList.Count == 100)
            domList.Dequeue();
        domList.Enqueue(serverScore > clientScore);

        serverScore = GameObject.Find("Main Camera").GetComponent<ViewQuality>().score;
        clientScore = GameObject.Find("Main Camera").GetComponent<ViewQuality>().opScore;
        autoChangeDominator();

        if (originDominator && !relief.GetComponent<Move>().dominator)
        {
            originDominator = false;
            RecordDomChangeInfo("Client");
        }
    }

    void autoChangeDominator()
    {
        if (serverScore > clientScore && Filter())
            ChangeDominator();
    }

    bool Filter()
    {
        int res = 0;
        foreach (var temp in domList)
        {
            if (temp) res += 1;
        }
        return res > filterBound;
    }

    public void ChangeDominator()
    {
        if (relief.GetComponent<Move>().dominator)
        {
            // Debug.Log("You are already the Dominator");
            return;
        }
        if (!networkScript.connected())
        {
            return;
        }
        originDominator = true;
        relief.GetComponent<Move>().dominator = true;
        relief.GetComponent<NetWorkAsServer>().SendMessageToClient("Dominator");
        RecordDomChangeInfo("Server");
    }

    public void RecordDomChangeInfo(string identity)
    {
        string info = identity + "," + GameObjInfo(relief) + "," +
            GameObjInfo(serverCam) + "," + GameObjInfo(clientCam) + "," +
            serverCam.GetComponent<ViewQuality>().score.ToString("0.000") + "," +
            serverCam.GetComponent<ViewQuality>().opScore.ToString("0.000");
        string filePath = "./Assets/Scripts/Control/Resource/DomChangeInfo.csv";
        GameObject.Find("AnimationObject").GetComponent<FileIO>().Writef(info, true, filePath);
    }

    string GameObjInfo(GameObject g)
    {
        return Vec3toStr(g.transform.position) + "," + QuatoStr(g.transform.rotation);
    }

    string Vec3toStr(Vector3 _vec)
    {
        string precision = "0.000";
        return _vec.x.ToString(precision) + "," + _vec.y.ToString(precision) + "," + _vec.z.ToString(precision);
    }

    string QuatoStr(Quaternion _q)
    {
        string precision = "0.000";
        return _q.x.ToString(precision) + "," + _q.y.ToString(precision) + "," + _q.z.ToString(precision) + "," + _q.w.ToString(precision);
    }
}
