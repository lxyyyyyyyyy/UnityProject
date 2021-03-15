using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Depth : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera m_Camera;
    public RenderTexture depthTexture;
    public Material Mat;

    void Start()
    {
        m_Camera = gameObject.GetComponent<Camera>();
        // 手动设置相机，让它提供场景的深度信息
        // 这样我们就可以在shader中访问_CameraDepthTexture来获取保存的场景的深度信息
        // float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, uv)); 获取某个像素的深度值
        m_Camera.depthTextureMode = DepthTextureMode.Depth;
        depthTexture = new RenderTexture(Screen.width, Screen.height, 32);
        depthTexture.enableRandomWrite = true;
    }


    void OnPostRender()
    {
        RenderTexture source = m_Camera.activeTexture;
        Graphics.Blit(source, depthTexture, Mat);  
    }
}