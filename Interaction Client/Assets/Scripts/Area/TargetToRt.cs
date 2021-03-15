using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TargetToRt : MonoBehaviour
{
    private CommandBuffer targetCommandBuffer = null, objectCommandBuffer = null;
    private Renderer targetEBO = null, objectEBO = null;
    public RenderTexture targetTex = null, objectTex = null;
    public GameObject target = null, obj = null;

    // Start is called before the first frame update
    void Start()
    {
        int width = Screen.width, height = Screen.height;
        // target
        targetEBO = target.GetComponentInChildren<Renderer>();

        targetTex = new RenderTexture(width, height, 16);
        targetTex.enableRandomWrite = true;
        targetTex.Create();

        targetCommandBuffer = new CommandBuffer();
        targetCommandBuffer.SetRenderTarget(targetTex);
        targetCommandBuffer.ClearRenderTarget(true, true, Color.black);
        targetCommandBuffer.DrawRenderer(targetEBO, targetEBO.sharedMaterial);

        // object of operation
        objectEBO = obj.GetComponentInChildren<Renderer>();
        objectTex = new RenderTexture(width, height, 16);
        objectTex.enableRandomWrite = true;
        objectTex.Create();

        objectCommandBuffer = new CommandBuffer();
        objectCommandBuffer.SetRenderTarget(objectTex);
        objectCommandBuffer.ClearRenderTarget(true, true, Color.black);
        objectCommandBuffer.DrawRenderer(objectEBO, objectEBO.sharedMaterial);

        // test
        // this.GetComponent<Renderer>().sharedMaterial.mainTexture = objectTex;

        Camera.main.AddCommandBuffer(CameraEvent.AfterForwardOpaque, targetCommandBuffer);
        Camera.main.AddCommandBuffer(CameraEvent.AfterForwardOpaque, objectCommandBuffer);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
