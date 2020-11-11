using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ServiceModel;
using Motto.Domain;
using UnityEngine.UI;
using System.IO;

public class ScrollViewManager : MonoBehaviour
{
    public GameObject productImage;
    public GameObject productText;

    public GameObject canvasBarPrefab;
    // Start is called before the first frame update
    void Start()
    {
        TxtReader.Instance.ParseJson("service-informations.txt");
        UnityEngine.Vector3 pos = productImage.transform.position;
        for(int i = 0; i < TxtReader.Instance.Response.Count; i++)
        {
            for(int j = 0; j < TxtReader.Instance.Response[i].products.Count; j++)
            {
                GameObject SpawnedImage = Instantiate(productImage, pos, UnityEngine.Quaternion.identity);
                SpawnedImage.GetComponent<ObjManager>().ServiceUrl = TxtReader.Instance.ServiceUrl;
                SpawnedImage.GetComponent<ObjManager>().canvasBarPrefab = canvasBarPrefab;
                SpawnedImage.GetComponent<ObjManager>().productOfObject = TxtReader.Instance.Response[i].products[j];
                pos = SpawnedImage.transform.position + new UnityEngine.Vector3(0,0,-45);
                SpawnedImage.GetComponent<ObjManager>().SetTextureToImage();
                productText.GetComponent<Text>().text = TxtReader.Instance.Response[i].products[j].name;
                
            }
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
