using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class ScreenshotTest : MonoBehaviour
{
    private RenderTexture renderTexture;
    [SerializeField] private GameObject a;

   
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
        //AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGBA32, ReadbackCompleted);
    }

    public void Screenshot()
 
    {

        renderTexture = new RenderTexture(Screen.width, Screen.height, 0);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);

        a.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", renderTexture);
    }





    //void ReadbackCompleted(AsyncGPUReadbackRequest request)
    //{
    //    // Render texture no longer needed, it has been read back.
    //    DestroyImmediate(renderTexture);

    //    using (var imageBytes = request.GetData<byte>())
    //    {
    //        // do something with the pixel data.
    //    }
    //}
}