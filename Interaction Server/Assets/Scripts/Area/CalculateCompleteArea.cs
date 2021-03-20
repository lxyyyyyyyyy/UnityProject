using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateCompleteArea : MonoBehaviour
{
    public ComputeShader shader;
    private int kernelHandle, overTarArea, overObjArea;
    private int[] initData;
    private Data[] outputData;
    private ComputeBuffer outputbuffer;
    private ComputeBuffer inputbuffer;
    private LargerFovCamera largerFovCamScript;

    struct Data
    {
        public int TarArea;
        public int ObjArea;
        public int CoveredArea;
    }

    void Start()
    {
        largerFovCamScript = GameObject.Find("LargerFovCamera").GetComponent<LargerFovCamera>();

        kernelHandle = shader.FindKernel("CSMain");

        initData = new int[1];
        inputbuffer = new ComputeBuffer(initData.Length, 4);

        outputData = new Data[Screen.width * Screen.height];
        outputbuffer = new ComputeBuffer(outputData.Length, 3 * 4);
    }

    void Update()
    {
        // 输入
        shader.SetTexture(kernelHandle, "TargetTex", largerFovCamScript.completeTargetTex);
        shader.SetTexture(kernelHandle, "ObjectTex", largerFovCamScript.completeObjTex);
        // init
        initData[0] = Screen.width;
        shader.SetBuffer(kernelHandle, "inputData", inputbuffer);
        inputbuffer.SetData(initData);

        // 输出
        shader.SetBuffer(kernelHandle, "outputData", outputbuffer);
        outputbuffer.GetData(outputData);

        overTarArea = 0; overObjArea = 0;
        for (int i = 0; i < outputData.Length; i++)
        {
            overTarArea += outputData[i].TarArea;
            overTarArea += outputData[i].CoveredArea;
            overObjArea += outputData[i].ObjArea;
        }
        overObjArea *= 3;
        overTarArea *= 3;

        Debug.LogFormat("over tar area: {0}, over obj area: {1}, ", overTarArea, overObjArea);

        // 要创建的线程组的数量
        shader.Dispatch(kernelHandle, Screen.width / 2, Screen.height / 2, 1);
    }

    void OnDisabled()
    {
        outputbuffer.Dispose();
        inputbuffer.Dispose();
    }

    public int GetTarOverArea()
    {
        return overTarArea;
    }

    public int GetObjOverArea()
    {
        return overObjArea;
    }
}
