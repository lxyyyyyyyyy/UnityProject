using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class NextDomChangeFrame : MonoBehaviour
{
    public UnityEvent event4;
    private Button btn;
    private string Path = "./Assets/Scripts/Control/Resource/DomChangeInfo.csv";
    private List<string> info_list;
    private int index = 0;
    private Move moveScript;
    private NetWorkAsServer networkScript;

    void Awake()
    {
        btn = GetComponent<Button>();
        event4.AddListener(NextFrame);
    }

    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            event4.Invoke();
        });

        networkScript = GameObject.Find("Relief").GetComponent<NetWorkAsServer>();
        moveScript = GameObject.Find("Relief").GetComponent<Move>();

        info_list = GameObject.Find("AnimationObject").GetComponent<FileIO>().Readf(Path);
    }

    void Update()
    {
        
    }

    void NextFrame()
    {
        if (index >= info_list.Count)
            return;
        string[] info = info_list[index++].Split(',');

        string name = info[0];
        GameObject.Find("Canvas/Text").GetComponent<Text>().text = name;
        // 物体移动
        moveScript.SetPosition(new Vector3(Convert.ToSingle(info[1]), Convert.ToSingle(info[2]), Convert.ToSingle(info[3])), true);
        moveScript.SetRotation(new Quaternion(Convert.ToSingle(info[4]), Convert.ToSingle(info[5]), Convert.ToSingle(info[6]), Convert.ToSingle(info[7])), true);
        // 主相机移动
        Camera.main.transform.position = new Vector3(Convert.ToSingle(info[8]), Convert.ToSingle(info[9]), Convert.ToSingle(info[10]));
        Camera.main.transform.rotation = new Quaternion(Convert.ToSingle(info[11]), Convert.ToSingle(info[12]), Convert.ToSingle(info[13]), Convert.ToSingle(info[14]));
        // client相机移动
        Vector3 op_pos = new Vector3(Convert.ToSingle(info[15]), Convert.ToSingle(info[16]), Convert.ToSingle(info[17]));
        Quaternion op_rot = new Quaternion(Convert.ToSingle(info[18]), Convert.ToSingle(info[19]), Convert.ToSingle(info[20]), Convert.ToSingle(info[21]));
        networkScript.SendMessageToClient("OpCamera" + Vec3toStr(op_pos) + "," + QuatoStr(op_rot) + ",");
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
