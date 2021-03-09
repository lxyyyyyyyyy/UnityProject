using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class FileIO : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Writef(List<string> data, string Path = "./Assets/Scripts/Control/Resource/TargetInfo.csv")
    {
        FileInfo fi = new FileInfo(Path);
        if (!fi.Exists)
        {
            fi.Create().Dispose();
        }

        StreamWriter sw = new StreamWriter(Path, false, Encoding.UTF8);
        foreach (string t in data)
        {
            sw.WriteLine(t);
        }
        sw.Close();
    }

    public List<string> Readf(string Path = "./Assets/Scripts/Control/Resource/TargetInfo.csv")
    {
        StreamReader sr = new StreamReader(Path, Encoding.UTF8);

        string line = "";
        List<string> lines = new List<string>();
        
        while ((line = sr.ReadLine()) != null)
        {
            lines.Add(line); 
        }

        sr.Close();
        return lines;
    }
}
