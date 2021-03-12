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
    private GameObject relief;
    private ClientCamera cliCamScript;

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
        relief = GameObject.Find("Relief");
        cliCamScript = GameObject.Find("ClientCamera").GetComponent<ClientCamera>(); 
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
        relief.transform.position = new Vector3(Convert.ToSingle(info[1]), Convert.ToSingle(info[2]), Convert.ToSingle(info[3]));
        relief.transform.rotation = new Quaternion(Convert.ToSingle(info[4]), Convert.ToSingle(info[5]), Convert.ToSingle(info[6]), Convert.ToSingle(info[7]));
        Camera.main.transform.position = new Vector3(Convert.ToSingle(info[8]), Convert.ToSingle(info[9]), Convert.ToSingle(info[10]));
        Camera.main.transform.rotation = new Quaternion(Convert.ToSingle(info[11]), Convert.ToSingle(info[12]), Convert.ToSingle(info[13]), Convert.ToSingle(info[14]));
        cliCamScript.SetPosition(new Vector3(Convert.ToSingle(info[15]), Convert.ToSingle(info[16]), Convert.ToSingle(info[17])));
        cliCamScript.SetRotation(new Quaternion(Convert.ToSingle(info[18]), Convert.ToSingle(info[19]), Convert.ToSingle(info[20]), Convert.ToSingle(info[21])));
    }
}
