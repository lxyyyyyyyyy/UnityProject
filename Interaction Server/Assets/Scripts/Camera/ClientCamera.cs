using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 position;
    private Quaternion rotation;

    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    public void SetPosition(Vector3 _pos)
    {
        position = _pos;
    }

    public void SetRotation(Quaternion _rot)
    {
        rotation = _rot;
    }
}
