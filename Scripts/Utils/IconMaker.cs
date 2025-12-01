using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class IconMaker : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt;
    public Image background;

    public void Start()
    {
        cam = Camera.main;
    }

    public void Create()
    {
        StartCoroutine(CaptureImage());
    }
    
    IEnumerator CaptureImage()
    {
        yield return null;

        Texture2D texture = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

        yield return null;

        var data = texture.EncodeToPNG();
        string name = "Thumbnail";
        string format = ".png";
        string path = Application.persistentDataPath + "/Thumbnails/";

        Debug.Log(path);

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        File.WriteAllBytes(path + name + format, data);

        yield return null;
    }
}
