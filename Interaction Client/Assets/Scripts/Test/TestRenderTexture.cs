using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TestRenderTexture : MonoBehaviour
{
    private CommandBuffer commandBuffer = null;
    private RenderTexture renderTexture = null;
    private Renderer targetRenderer = null;
    public GameObject targetObject = null;

    // Start is called before the first frame update
    void Start()
    {
        targetRenderer = targetObject.GetComponentInChildren<Renderer>();
        // 申请RT
        renderTexture = new RenderTexture(512, 512, 16);
        // renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default, 4);
        // 设置Command Buffer渲染目标为申请的RT
        commandBuffer = new CommandBuffer();
        commandBuffer.SetRenderTarget(renderTexture);
        //初始颜色设置为黑色
        commandBuffer.ClearRenderTarget(true, true, Color.black);

        //绘制目标对象
        Material mat = targetRenderer.sharedMaterial;
        commandBuffer.DrawRenderer(targetRenderer, mat);

        this.GetComponent<Renderer>().sharedMaterial.mainTexture = renderTexture;
        //直接加入相机的CommandBuffer事件队列中
        Camera.main.AddCommandBuffer(CameraEvent.AfterForwardOpaque, commandBuffer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
