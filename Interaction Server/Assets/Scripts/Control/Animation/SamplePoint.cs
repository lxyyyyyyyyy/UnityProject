using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SamplePoint : MonoBehaviour
{
    public UnityEvent event5;
    private Button btn;
    private List<Vector3> posshere;
    private int sampleNum, r, index = 0;
    private Vector3 sphereCenter;

    void Awake()
    {
        btn = GetComponent<Button>();
        event5.AddListener(samplePoint);
    }

    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            event5.Invoke();
        });

        r = 1;
        posshere = new List<Vector3>(0); 
    }

    void Update()
    {
        sphereCenter = GameObject.Find("Relief").transform.position;
    }

    void samplePoint() //采样
    {
        float sita = UnityEngine.Random.Range(0.0f, 10.0f) / 10;
        float pesi = UnityEngine.Random.Range(0.0f, 10.0f) / 10;
        float x, y, z;
        if (index % 2 == 0)
        {
            x = sphereCenter.x + r * Mathf.Cos(sita) * Mathf.Sin(pesi);
            y = sphereCenter.y + r * Mathf.Sin(sita) * Mathf.Sin(pesi);
            z = sphereCenter.z - r * Mathf.Cos(pesi);
            posshere.Add(new Vector3(x, y, z));
        }
        else
        {
            x = sphereCenter.x - r * Mathf.Cos(sita) * Mathf.Sin(pesi);
            y = sphereCenter.y - r * Mathf.Sin(sita) * Mathf.Sin(pesi);
            z = sphereCenter.z - r * Mathf.Cos(pesi);
            posshere.Add(new Vector3(x, y, z));
        }
        Camera.main.transform.position = new Vector3(x, y, z);
        Camera.main.transform.LookAt(sphereCenter);
    }
}
