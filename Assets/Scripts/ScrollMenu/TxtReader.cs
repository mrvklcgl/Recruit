using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Motto.Domain;
using System.Text.RegularExpressions;

public class TxtReader 
{
    private static TxtReader instance = null;
    private List<CatalogCategory> response = new List<CatalogCategory>();
    private string serviceUrl;

    public List<CatalogCategory> Response {get{return response;} set { response = value; }}
    public string ServiceUrl { get {return serviceUrl;} set { serviceUrl = value;}}
    

    public static TxtReader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TxtReader();
            }
            return instance;
        }
    }

    private TxtReader()
    {
        
    }
    public void ParseJson(string path)
    {
        string [] lines = File.ReadAllLines(path);
        int i = 0;
        for(i = 0; i < lines.Length; i++)
        {
            var item = lines[i];
            if (item.Contains("Service:"))
            {
                int startPos = item.IndexOf("Service:" + 8);
                serviceUrl = item.Substring(startPos, item.Length - startPos);
            }
            if (item.Contains("Response:"))
            {
                break;
            }
        }
        string fileLinesToJson = "";
        for(int j = i + 1; j < lines.Length; j++)
        {
            fileLinesToJson += lines[j];
            fileLinesToJson = Regex.Unescape(fileLinesToJson);
        }

        response.Add(JsonUtility.FromJson<CatalogCategory>(fileLinesToJson));
    }
}
