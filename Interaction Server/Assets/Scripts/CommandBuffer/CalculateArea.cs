using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateArea : MonoBehaviour
{
    public ComputeShader shader;
    private int area = 0, coveredArea = 0, kernelHandle;
    private int[] initData;
    private Data[] outputData;
    private ComputeBuffer outputbuffer;
    private ComputeBuffer inputbuffer;
    private TargetToRt targetToRtScript;

    struct Data
    {
        public int Area;
        public int CoveredArea;
    }

    void Start()
    {
        targetToRtScript = GameObject.Find("GameObject").GetComponent<TargetToRt>();

        kernelHandle = shader.FindKernel("CSMain");

        initData = new int[1];
        inputbuffer = new ComputeBuffer(initData.Length, 4);

        outputData = new Data[Screen.width * Screen.height];
        outputbuffer = new ComputeBuffer(outputData.Length, 2 * 4);
    }

    void Update()
    {
        // 输入
        shader.SetTexture(kernelHandle, "TargetTex", targetToRtScript.targetTex);
        shader.SetTexture(kernelHandle, "ObjectTex", targetToRtScript.objectTex);
        // init
        initData[0] = Screen.width;
        shader.SetBuffer(kernelHandle, "inputData", inputbuffer);
        inputbuffer.SetData(initData);

        // 输出
        shader.SetBuffer(kernelHandle, "outputData", outputbuffer);
        outputbuffer.GetData(outputData);

        area = 0; coveredArea = 0;
        for (int i = 0; i < outputData.Length; i++)
        {
            area += outputData[i].Area; coveredArea += outputData[i].CoveredArea;
        }
        // Debug.LogFormat("Area: {0}, Covered Area: {1}", area, coveredArea);

        // 要创建的线程组的数量
        shader.Dispatch(kernelHandle, Screen.width / 2, Screen.height / 2, 1);        
    }

    void OnDisabled()
    {
        outputbuffer.Dispose();
        inputbuffer.Dispose();
    }

    public int GetArea()
    {
        return area;
    }

    public int GetCoveredArea()
    {
        return coveredArea;
    }
}
