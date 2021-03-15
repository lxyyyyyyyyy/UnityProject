using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculAreaUsingDepth : MonoBehaviour
{
    public ComputeShader shader;
    private int tarArea, objArea, coveredArea, kernelHandle;
    private int[] initData;
    private Data[] outputData;
    private ComputeBuffer outputbuffer;
    private ComputeBuffer inputbuffer;
    private Depth allDepthScript, tarDepthScript, objDepthScript;

    struct Data
    {
        public int TarArea;
        public int ObjArea;
        public int CoveredArea;
    }

    void Start()
    {
        allDepthScript = Camera.main.GetComponent<Depth>();
        tarDepthScript = GameObject.Find("RenderTarCamera").GetComponent<Depth>();
        objDepthScript = GameObject.Find("RenderObjCamera").GetComponent<Depth>();

        kernelHandle = shader.FindKernel("CSMain");

        initData = new int[1];
        inputbuffer = new ComputeBuffer(initData.Length, 4);

        outputData = new Data[Screen.width * Screen.height];
        outputbuffer = new ComputeBuffer(outputData.Length, 3 * 4);
    }

    void Update()
    {
        // 输入
        shader.SetTexture(kernelHandle, "AllDepthTex", allDepthScript.depthTexture);
        shader.SetTexture(kernelHandle, "SingleTarDepthTex", tarDepthScript.depthTexture);
        shader.SetTexture(kernelHandle, "SingleObjDepthTex", objDepthScript.depthTexture);
        // init
        initData[0] = Screen.width;
        shader.SetBuffer(kernelHandle, "inputData", inputbuffer);
        inputbuffer.SetData(initData);

        // 输出
        shader.SetBuffer(kernelHandle, "outputData", outputbuffer);
        outputbuffer.GetData(outputData);

        tarArea = 0; objArea = 0; coveredArea = 0;
        for (int i = 0; i < outputData.Length; i++)
        {
            tarArea += outputData[i].TarArea;
            objArea += outputData[i].ObjArea;
            coveredArea += outputData[i].CoveredArea;
        }
        Debug.LogFormat("Target Area: {0}, Obj Area: {1}, CoveredArea: {2}", tarArea, objArea, coveredArea);

        // 要创建的线程组的数量
        shader.Dispatch(kernelHandle, Screen.width / 2, Screen.height / 2, 1);
    }

    void OnDisabled()
    {
        outputbuffer.Dispose();
        inputbuffer.Dispose();
    }

    public int GetObjArea() => objArea;

    public int GetTarArea() => tarArea;

    public int GetCoveredArea() => coveredArea;
}
