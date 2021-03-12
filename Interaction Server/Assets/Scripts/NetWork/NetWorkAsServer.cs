using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System;

public class NetWorkAsServer : MonoBehaviour
{
    // Start is called before the first frame update
    private string IP;
    private int port;
    private Socket serverSocket;
    private List<Socket> clientList;

    private Move targetMoveScript;
    private MyRay myRayScript;
    private MyCamera myCameraScript;
    private OppositeRay oppositeRayScript;
    private ClientCamera clientCameraScript;
    private ViewQuality viewQualityScript;
    private ButtonControl butControlScript;

    void Awake()
    {
        IP = "127.0.0.1";
        port = 10001;
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientList = new List<Socket>();

        targetMoveScript = GameObject.Find("Relief").GetComponent<Move>();
        myRayScript = GameObject.Find("Main Camera").GetComponent<MyRay>();
        myCameraScript = GameObject.Find("Main Camera").GetComponent<MyCamera>();
        oppositeRayScript = GameObject.Find("Relief/Line2").GetComponent<OppositeRay>();
        clientCameraScript = GameObject.Find("ClientCamera").GetComponent<ClientCamera>();
        viewQualityScript = GameObject.Find("Main Camera").GetComponent<ViewQuality>();
        butControlScript = GameObject.Find("Canvas/Button1").GetComponent<ButtonControl>();
    }

    void Start()
    {
        Debug.Log("服务器端已启动!");
        serverSocket.Bind(new IPEndPoint(IPAddress.Parse(IP), port));
        serverSocket.Listen(10); // 设定最多100个排队连接请求   
        Thread myThread = new Thread(ListenClientConnect); // 通过多线程监听客户端连接  
        myThread.Start();
    }

    void OnDestroy()
    {
        for (int i = 0; i < clientList.Count; i++)
        {
            clientList[i].Close();
        }
    }

    void ListenClientConnect()
    {
        while (true)
        {
            Socket clientSocket = serverSocket.Accept();
            //clientSocket.Send(Encoding.UTF8.GetBytes("服务器连接成功"));
            clientList.Add(clientSocket);
            SendSavedMsg(clientSocket);
            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start(clientSocket);
            // 另一用户已连接，设置另一用户射线状态及颜色
            oppositeRayScript.SetState(true);
            oppositeRayScript.SetColor(Color.red);
        }
    }

    void ReceiveMessage(object clientSocket)
    {
        byte[] buffer = new byte[1024];
        Socket myClientSocket = (Socket)clientSocket;
        while (true)
        {
            try
            {
                // 通过clientSocket接收数据  
                int receiveBytes = myClientSocket.Receive(buffer);
                if (receiveBytes == 0)
                {
                    Debug.Log("No Message!");
                    return;
                }
                string recvStr = Encoding.UTF8.GetString(buffer, 0, receiveBytes);
                // Debug.LogFormat ("接收客户端 {0} 的消息：{1}", myClientSocket.RemoteEndPoint.ToString(), recvStr);
                ParseMsg(recvStr);
            }
            catch (Exception ex)
            {
                Debug.Log (ex.Message);
                // myClientSocket.Shutdown(SocketShutdown.Both); // 禁止发送和上传
                // myClientSocket.Close(); // 关闭Socket并释放资源
                // break;
            }
        }
    }

    public void SendMessageToClient(string msg)
    {
        for (int i = 0; i < clientList.Count; i++)
        {
            Socket clientSocket = clientList[i];
            clientSocket.Send(Encoding.UTF8.GetBytes("#" + msg));
            // Debug.Log("向客户发送消息：" + msg);
        }
    }

    void ParseMsg(string msg)
    {
        Regex DominatorPattern = new Regex(@"#Dominator"); 
        Regex OppositeDetectPattern = new Regex(@"#Ready");
        Regex TargetInfoPattern = new Regex(@"#Target(.+)");
        Regex RayInfPattern = new Regex(@"#Ray(.+)");
        Regex CameraInfPattern = new Regex(@"#Camera(.+)");
        Regex ViewScorePattern = new Regex(@"#ViewScore(.+)");
        Regex ViewFactorPattern = new Regex(@"#Area(.+),OverArea(.+),Dis(.+),Oc(.+),Or(.+)");

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
            DealOppositeCameraInfo(CameraInfPattern.Match(msg).Groups[1].Value);
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

    void DealChangeDominator() {
        targetMoveScript.dominator = false;
    } 

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

    void DealOppositeCameraInfo(string msg)
    {
        List<float> info = stringToFloatList(msg);
        clientCameraScript.SetPosition(new Vector3(info[0], info[1], info[2]));
        clientCameraScript.SetRotation(new Quaternion(info[3], info[4], info[5], info[6]));
    }

    void DealViewScore(string msg)
    {
        Regex floatPattern = new Regex(@"(\d+\.\d{3})(.*)");
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

    void SendSavedMsg(Socket clientSocket)
    {
        if (myRayScript.detected)
        {
            clientSocket.Send(Encoding.UTF8.GetBytes("#Ready"));
        }
        myRayScript.SendInfo();
        myCameraScript.SendInfo();
    }

    public bool connected()
    {
        return clientList.Count > 0;
    }
}
