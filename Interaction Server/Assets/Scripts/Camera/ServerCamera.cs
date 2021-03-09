using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GameObject.Find("Main Camera").GetComponent<Transform>().position;
        transform.rotation = GameObject.Find("Main Camera").GetComponent<Transform>().rotation;
    }
}
