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
        
        //targetTex = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
        //targetTex.enableRandomWrite = true;

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
        /*
        Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        RenderTexture.active = targetTex;
        texture2D.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture2D.Apply();
        Debug.Log(texture2D.GetPixel(0,0));
        */
    }
}
