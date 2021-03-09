using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float Speed;
    private KeyCode[] inputKeys;
    private Vector3[] directionForkeys;
    // Start is called before the first frame update
    void Start()
    {
        Speed = 0.1f;
        // inputKeys = new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.Q, KeyCode.E };
        inputKeys = new KeyCode[] { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N };
        directionForkeys = new Vector3[] { Vector3.forward, Vector3.left, Vector3.right, Vector3.back, Vector3.up, Vector3.down };
    }

    // Update is called once per frame
    void Update()
    {
        // 平移
        for (int i = 0; i < inputKeys.Length; i++)
        {
            var key = inputKeys[i];
            if (Input.GetKey(key))
            {
                transform.Translate(directionForkeys[i] * Time.fixedDeltaTime * Speed, Space.World);
            }
        }
    }
}
