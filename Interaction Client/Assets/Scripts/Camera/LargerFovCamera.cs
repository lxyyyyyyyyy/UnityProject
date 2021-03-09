using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LargerFovCamera : MonoBehaviour
{
    private CommandBuffer targetCommandBuffer = null, objectCommandBuffer = null;
    private Renderer targetEBO = null, objectEBO = null;
    public RenderTexture completeTargetTex = null, completeObjTex = null;
    public GameObject completeTarget = null, completeObj = null;

    // Start is called before the first frame update
    void Start()
    {
        int width = Screen.width, height = Screen.height;
        // target
        targetEBO = completeTarget.GetComponentInChildren<Renderer>();

        completeTargetTex = new RenderTexture(width, height, 16);
        completeTargetTex.enableRandomWrite = true;
        completeTargetTex.Create();


        targetCommandBuffer = new CommandBuffer();
        targetCommandBuffer.SetRenderTarget(completeTargetTex);
        targetCommandBuffer.ClearRenderTarget(true, true, Color.black);
        targetCommandBuffer.DrawRenderer(targetEBO, targetEBO.sharedMaterial);

        // object of operation
        objectEBO = completeObj.GetComponentInChildren<Renderer>();
        completeObjTex = new RenderTexture(width, height, 16);
        completeObjTex.enableRandomWrite = true;
        completeObjTex.Create();

        objectCommandBuffer = new CommandBuffer();
        objectCommandBuffer.SetRenderTarget(completeObjTex);
        objectCommandBuffer.ClearRenderTarget(true, true, Color.black);
        objectCommandBuffer.DrawRenderer(objectEBO, objectEBO.sharedMaterial);

        GameObject.Find("LargerFovCamera").GetComponent<Camera>().AddCommandBuffer(CameraEvent.AfterForwardOpaque, targetCommandBuffer);
        GameObject.Find("LargerFovCamera").GetComponent<Camera>().AddCommandBuffer(CameraEvent.AfterForwardOpaque, objectCommandBuffer);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position;
        transform.rotation = Camera.main.transform.rotation;
    }
}
