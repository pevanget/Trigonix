using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GenerateScreenshot : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Camera _thisCamera;
    [SerializeField] private Camera _mainCamera;
    private RenderTexture _rendText;
    //private RenderTexture tex;
    void Start()
    {
        //_camera = GetComponent<Camera>();
        _rendText = _thisCamera.targetTexture;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Screenshot()
    {
        //_rendText = _camera.targetTexture;
        //_camera.enabled = true;
        _thisCamera.orthographicSize = _mainCamera.orthographicSize;
        
        transform.position = _mainCamera.transform.position;
        //Debug.Log(transform.position);
        //Debug.Log( _mainCamera.transform.position);
        Texture2D myTexture = toTexture2D(_rendText);
        byte[] bytes = myTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../Screenshots/SavedScreen.png", bytes);
        Debug.Log("Screenshot saved!");
        //_camera.enabled = false;

    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(1920 , 1080, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
