using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public UnityEvent event1;
    private Button btn;
    private bool originDominator;
    private GameObject serverCam, clientCam, relief;
    private NetWorkAsServer networkScript;

    void Awake()
    {
        btn = GetComponent<Button>();
        event1.AddListener(ChangeDominator);
    }

    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            event1.Invoke();
        });
        serverCam = GameObject.Find("Main Camera");
        clientCam = GameObject.Find("ClientCamera");
        relief = GameObject.Find("Relief");
        networkScript = GameObject.Find("Relief").GetComponent<NetWorkAsServer>();
        originDominator = relief.GetComponent<Move>().dominator;
    }

    // Update is called once per frame
    void Update()
    {
        if (originDominator && !relief.GetComponent<Move>().dominator)
        {
            originDominator = false;
            RecordDomChangeInfo("Client");
        }
        // originDominator = relief.GetComponent<Move>().dominator;
    }

    void ChangeDominator()
    {
        if (relief.GetComponent<Move>().dominator)
        {
            Debug.Log("You are already the Dominator");
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
        string info = identity + "," +  GameObjInfo(relief) + "," + 
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
