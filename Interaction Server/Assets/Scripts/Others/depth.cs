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
        // saveDepthMap(depthTexture, "depth.png");   
    }
    

    // Event function that Unity calls after a Camera has finished rendering, that allows you to modify the Camera's final image.
    /*
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (null != Mat)
        {
            // Copies source texture into destination render texture with a shader.
            // 使用material把这个source渲染到那个destination, Blit(source, destination, material)
            // 使用这个material的意思是使用这个material的shader
            // 这个material没必要赋到物体上
            // Graphics.Blit(source, destination, Mat);
            RenderTexture depthTexture = new RenderTexture(source.width, source.height, 32);
            Graphics.Blit(source, depthTexture, Mat);
            saveDepthMap(depthTexture);
        }
    }
    */
    
    private void saveDepthMap(RenderTexture DepthRenderTexture, string MapName)
    {
        int Width = DepthRenderTexture.width;
        int Height = DepthRenderTexture.height;
        Texture2D texture2D = new Texture2D(Width, Height);
        var previous = RenderTexture.active;
        RenderTexture.active = DepthRenderTexture;
        texture2D.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
        RenderTexture.active = previous;
        texture2D.Apply();
        byte[] Data = texture2D.EncodeToPNG();
        FileStream file = File.Open(MapName, FileMode.Create, FileAccess.Write);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(Data);
        file.Close();
    }
    
}