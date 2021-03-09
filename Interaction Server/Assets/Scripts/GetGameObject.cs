using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGameObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //camare2D.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            Debug.Log(LayerMask.NameToLayer("target"));
            Debug.Log(LayerMask.NameToLayer("StandardObject"));
            if (Physics.Raycast(ray, out hit, 100.0f, 0 << LayerMask.NameToLayer("target") | 1 << LayerMask.NameToLayer("StandardObject") ))
            {
                print("hit:" + hit.collider.gameObject.name + Input.mousePosition);
            }
        }
    }
}
