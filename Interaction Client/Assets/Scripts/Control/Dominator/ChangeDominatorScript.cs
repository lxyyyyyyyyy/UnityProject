using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDominatorScript : MonoBehaviour
{
    private float serverScore, clientScore;
    private Queue<bool> domList;
    private int filterBound = 60;
    
    void Start()
    {
        domList = new Queue<bool>(0);
    }

    // Update is called once per frame
    void Update()
    {
        serverScore = GameObject.Find("Main Camera").GetComponent<ViewQuality>().opScore;
        clientScore = GameObject.Find("Main Camera").GetComponent<ViewQuality>().score;
        if (domList.Count == 100)
            domList.Dequeue();
        domList.Enqueue(clientScore > serverScore);
        
        autoChangeDominator();
    }

    void autoChangeDominator()
    {
        if (clientScore > serverScore && Filter())
            ChangeDominator();
    }

    bool Filter()
    {
        int res = 0;
        foreach (var temp in domList)
        {
            if (temp) res += 1;
        }
        return res > filterBound;
    }

    public void ChangeDominator()
    {
        if (GameObject.Find("Relief").GetComponent<Move>().dominator)
        {
            // Debug.Log("You are already the Dominator");
            return;
        }
        GameObject.Find("Relief").GetComponent<Move>().dominator = true;
        GameObject.Find("Relief").GetComponent<NetWorkAsClient>().SendMessageToServer("Dominator");
    }
}
