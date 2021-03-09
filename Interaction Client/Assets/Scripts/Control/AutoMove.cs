using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    private Vector3 position;
    private Quaternion rotation;
    private bool change = false;

    // Start is called before the first frame update
    void Start()
    {
        if (change)
        {
            transform.position = position;
            transform.rotation = rotation;
            change = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(Vector3 _position)
    {
        position = _position;
        change = true;
    }

    public void SetRotation(Quaternion _rotation)
    {
        rotation = _rotation;
        change = true;
    }
}
