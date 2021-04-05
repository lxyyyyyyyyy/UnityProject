using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System;

public class NetWorkAsClient : MonoBehaviour
{
    // Start is called before the first frame update
    private string serverIP;
    private int serverPort;
    private Socket clientSocket;

    private Queue<string> msg_queue;

    private Move targetMoveScript;
    private MyRay myRayScript;
    private OppositeRay oppositeRayScript;
    private ServerCamera serverCameraScript;
    private ViewQuality viewQualityScript;
    private AutoMove autoMoveScript;

    void Awake()
    {
        serverIP = "127.0.0.1";
        serverPort = 10002;
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        msg_queue = new Queue<string>();

        targetMoveScript = GameObject.Find("Relief").GetComponent<Move>();
        myRayScript = GameObject.Find("Main Camera").GetComponent<MyRay>();
        oppositeRayScript = GameObject.Find("Relief/Line2").GetComponent<OppositeRay>();
        serverCameraScript = GameObject.Find("ServerCamera").GetComponent<ServerCamera>();
        viewQualityScript = GameObject.Find("Main Camera").GetComponent<ViewQuality>();
        autoMoveScript = GameObject.Find("Main Camera").GetComponent<AutoMove>();
    }

    void Start()
    {
        Debug.Log("客户端已启动!");
        try
        {
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(serverIP), serverPort)); //配置服务器IP与端口  
            Debug.Log("连接服务器成功");

            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start();
        }
        catch
        {
            Debug.Log("连接服务器失败");
            return;
        }
        SendMessageToServer("hello");
    }

    void Update()
    {
        while (msg_queue.Count > 0)
        {
            string msg = msg_queue.Dequeue();
            ParseMsg(msg);
        }
    }

    void OnDestroy()
    {
        clientSocket.Close();
    }

    public void SendMessageToServer(string msg)
    {
        // 向服务器发送数据，需要发送中文则需要使用Encoding.UTF8.GetBytes()，否则会乱码
        clientSocket.Send(Encoding.UTF8.GetBytes("#" + msg));
        // Debug.Log("向服务器发送消息：" + msg);
    }

    void ReceiveMessage()
    {
        byte[] buffer = new byte[1024];
        while (true)
        {
            // int bytes = clientSocket.Receive(recvBytes, recvBytes.Length, 0);  第二个参数是偏移量，第三个参数是SocketFlag(enum) 
            int receiveBytes = clientSocket.Receive(buffer);
            if (receiveBytes == 0)
            {
                Debug.Log("No Message!");
                return;
            }
            string recvStr = Encoding.UTF8.GetString(buffer, 0, receiveBytes);
            // Debug.LogFormat("接收服务器的消息：{0}", recvStr);
            string[] singleStr = recvStr.Split('#');
            foreach(string s in singleStr)
            {
                if (s.Length == 0) continue;
                msg_queue.Enqueue(s);
            }
        } 
    }

    void ParseMsg(string msg)
    {
        Regex DominatorPattern = new Regex(@"Dominator");
        Regex OppositeDetectPattern = new Regex(@"Ready");
        Regex TargetInfoPattern = new Regex(@"Target(.+)");
        Regex RayInfPattern = new Regex(@"Ray(.+)");
        Regex CameraInfPattern = new Regex(@"OpCamera(.+)");
        Regex OpCameraInfPattern = new Regex(@"Camera(.+)");
        Regex ViewScorePattern = new Regex(@"ViewScore(.+)");
        Regex ViewFactorPattern = new Regex(@"Area(.+),OverArea(.+),Dis(.+),Oc(.+),Or(.+)");

        if (DominatorPattern.IsMatch(msg))
        {
            DealChangeDominator();
        }
        if (OppositeDetectPattern.IsMatch(msg))
        {
            DealOppositeDetectTarget();
        }
        if (TargetInfoPattern.IsMatch(msg))
        {
            DealTargetInfo(TargetInfoPattern.Match(msg).Groups[1].Value);
        }
        if (RayInfPattern.IsMatch(msg))
        {
            DealOppositeRayInfo(RayInfPattern.Match(msg).Groups[1].Value);
        }
        if (CameraInfPattern.IsMatch(msg))
        {
            Debug.Log(msg);
            DealCameraInfo(CameraInfPattern.Match(msg).Groups[1].Value);
        }
        if (OpCameraInfPattern.IsMatch(msg))
        {
            DealOppositeCameraInfo(OpCameraInfPattern.Match(msg).Groups[1].Value);
        }
        if (ViewScorePattern.IsMatch(msg))
        {
            DealViewScore(ViewScorePattern.Match(msg).Groups[1].Value);
        }
        if (ViewFactorPattern.IsMatch(msg))
        {
            Match _match = ViewFactorPattern.Match(msg);
            DealViewScoreFactor(_match.Groups[1].Value, _match.Groups[2].Value, _match.Groups[3].Value,
                _match.Groups[4].Value, _match.Groups[5].Value);
        }
    }

    void DealChangeDominator() => targetMoveScript.dominator ^= true;

    void DealOppositeDetectTarget()
    {
        myRayScript.oppositeDetected = true;
        // 设置另一用户射线颜色
        if (myRayScript.detected)
            oppositeRayScript.SetColor(Color.green);
        else
            oppositeRayScript.SetColor(Color.blue);
    }

    void DealTargetInfo(string msg)
    {
        List<float> info = stringToFloatList(msg);
        targetMoveScript.SetPosition(new Vector3(info[0], info[1], info[2]));
        targetMoveScript.SetRotation(new Quaternion(info[3], info[4], info[5], info[6]));
    }

    void DealOppositeRayInfo(string msg)
    {
        List<float> info = stringToFloatList(msg);
        oppositeRayScript.SetInitPos(new Vector3(info[0], info[1], info[2]));
        oppositeRayScript.SetEndPos(new Vector3(info[3], info[4], info[5]));
    }

    void DealCameraInfo(string msg)
    {
        List<float> info = stringToFloatList(msg);
        autoMoveScript.SetPosition(new Vector3(info[0], info[1], info[2]));
        autoMoveScript.SetRotation(new Quaternion(info[3], info[4], info[5], info[6]));
    }

    void DealOppositeCameraInfo(string msg)
    {
        List<float> info = stringToFloatList(msg);
        serverCameraScript.SetPosition(new Vector3(info[0], info[1], info[2]));
        serverCameraScript.SetRotation(new Quaternion(info[3], info[4], info[5], info[6]));
    }

    void DealViewScore(string msg)
    {
        Regex floatPattern = new Regex(@"(\d+.\d{3})(.*)");
        viewQualityScript.SetOpScore(Convert.ToSingle(floatPattern.Match(msg).Groups[1].Value));
    }

    void DealViewScoreFactor(string S, string overS, string D, string Oc, string Or)
    {
        Regex floatPattern = new Regex(@"(\d+\.\d{3})(.*)");
        viewQualityScript.SetOpFactor(Convert.ToSingle(S), Convert.ToSingle(overS), Convert.ToSingle(D), 
            Convert.ToSingle(Oc), Convert.ToSingle(floatPattern.Match(Or).Groups[1].Value));
    }

    List<float> stringToFloatList(string str)
    {
        string oneDimensional = @"-?\d{1,}\.\d{3}";
        Regex posPattern = new Regex(oneDimensional);

        List<float> info = new List<float>();
        foreach (Match match in posPattern.Matches(str))
        {
            info.Add(Convert.ToSingle(match.Value));
        }
        return info;
    }
}
