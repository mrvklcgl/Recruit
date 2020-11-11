using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Dummiesman;
using System.IO;

public class ObjManager : MonoBehaviour
{
    public Motto.Domain.CatalogProduct productOfObject;
    private string filePath;
    private string serviceUrl;

    private string savedPath;

    public string ServiceUrl { get {return serviceUrl;} set { serviceUrl = value;}}
    public string SavedPath { get {return savedPath;} set { savedPath = value;}}

    private GameObject progressBar;
    public GameObject canvasBarPrefab;

    void OnTriggerEnter(Collider col)
    {
        progressBar = Instantiate(canvasBarPrefab, new Vector3(0,0,0), UnityEngine.Quaternion.identity);
        StartCoroutine(DownloadFile());
    }

    IEnumerator GetTexture()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(productOfObject.thumbnailFileUrl);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    public void SetTextureToImage()
    {
        StartCoroutine(GetTexture());
    }

    public void LoadObj()
    {
        //string filePath = @"I:\random\cylinder.obj";
        if (!File.Exists(filePath))
        {
            Debug.LogError("Please set FilePath in ObjFromFile.cs to a valid path.");
            return;
        }
 
        //load
        var loadedObj = new OBJLoader().Load(filePath);
        string ScriptName = "ObjectMoveCommand";
        System.Type ObjScriptType = System.Type.GetType (ScriptName + ",Assembly-CSharp");
        AddScriptToObject(loadedObj, ObjScriptType);
    }

    void AddScriptToObject (GameObject go, System.Type type)
    {
        go.AddComponent(type);
    }

    IEnumerator DownloadFile() 
    {
        var uwr = new UnityWebRequest(productOfObject.fileUrl, UnityWebRequest.kHttpVerbGET);
        savedPath = Path.Combine(Application.persistentDataPath, productOfObject.name + ".zip");
        uwr.downloadHandler = new DownloadHandlerFile(savedPath);
        UnityWebRequestAsyncOperation operation = uwr.SendWebRequest();
        yield return DownloadProgress(operation);
        if (uwr.isNetworkError || uwr.isHttpError)
            Debug.LogError(uwr.error);
        else
        {
            Debug.Log("File successfully downloaded and saved to " + savedPath);
            Destroy(progressBar);
        }
        
        uwr.Dispose();
    }

    IEnumerator DownloadProgress(UnityWebRequestAsyncOperation operation)
    {
        while (!operation.isDone)
        {
            GameObject progressBar = GameObject.FindWithTag("RadialFill");
            progressBar.GetComponent<RadialFillScript>().FillPercentage(operation.progress);
            yield return null;
        }
    }
}
