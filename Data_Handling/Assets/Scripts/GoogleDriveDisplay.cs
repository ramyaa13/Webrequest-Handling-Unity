using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

//JSON element Data
public struct Data
{
    public string Name;
    public string ImageURL;
    public string VideoURL;
}
public class GoogleDriveDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI NameText;
    [SerializeField]
    private RawImage Image;
    [SerializeField]
    private VideoPlayer videoPlayer;

    private string JsonLink = "https://drive.google.com/uc?export=download&id=1ImX854d0ipVk_JOCkCJJWxsO4j21o_Fq";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DisplayImageFromGoogleDrive()
    {
        StartCoroutine(GetData(JsonLink));
    }
    IEnumerator GetData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.Send();

        if(request.isNetworkError)
        {
            Debug.LogError("Errorr");
        }
        else
        {
            Data data = JsonUtility.FromJson<Data>(request.downloadHandler.text);

            NameText.text = data.Name;

            StartCoroutine(GetImage(data.ImageURL));
            StreamVideo(data.VideoURL);
        }
        request.Dispose();
    }

    IEnumerator GetImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.Send();

        if (request.isNetworkError)
        {
            Debug.LogError("Errorr");
        }
        else
        {
            Image.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        request.Dispose();
    }

    public void StreamVideo(string url)
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = url;
        videoPlayer.Play();
    }
    
}
