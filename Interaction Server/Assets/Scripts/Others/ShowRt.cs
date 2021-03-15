using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRt : MonoBehaviour
{
    public RenderTexture renderTexture;
    private Depth GetDepthRtScript;
    // Start is called before the first frame update
    void Start()
    {
        GetDepthRtScript = GameObject.Find("RenderDepthCamera").GetComponent<Depth>();
    }

    // Update is called once per frame
    void Update()
    {   
        this.GetComponent<Renderer>().sharedMaterial.mainTexture = GetDepthRtScript.depthTexture;
    }
}
