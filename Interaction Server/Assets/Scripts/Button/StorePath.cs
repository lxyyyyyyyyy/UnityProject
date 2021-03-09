using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class StorePath : MonoBehaviour
{
    public UnityEvent event3;
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        event3.AddListener(GameObject.Find("AnimationObject").GetComponent<TrajectoryStore>().StoreTrajectory);
    }

    // Start is called before the first frame update
    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            event3.Invoke();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
