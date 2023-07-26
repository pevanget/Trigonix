using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProcessScreenshot : MonoBehaviour
{

    //PROBLEM: ENCODE: Debug.Log(switchedColors);
    //[SerializeField]
    private Texture2D _tex;
    private Texture2D _texBnW;
    private string _pathForEditorOriginal;
    private string _pathForEditorBnW;
    private string _pathForBuildOriginal;
    private string _pathForBuildBnW;
    //private string _pathBnW; 
    private byte[] _fileData; //might need to reset
    // Start is called before the first frame update
    void Start()
    {
        _pathForEditorOriginal = Application.dataPath + "/../Screenshots";
        _pathForEditorBnW = Application.dataPath + "/../Screenshots";
        _pathForBuildOriginal = Application.persistentDataPath;
        _pathForBuildBnW = Application.persistentDataPath;
    }

    public void DecodeLastScreenshot()
    {
        LoadScreenshot();
        ThresholdScreenshot();
        FindStartAndSize();

        //DecodeScreenshot();
    }

    private void LoadScreenshot()
    {
        //Application.dataPath + "/../Screenshots/SavedScreen.png"
        if (Directory.Exists(_pathForEditorOriginal))
        {
            if (File.Exists(_pathForEditorOriginal + "/SavedScreen.png"))
            {
                _fileData = File.ReadAllBytes(_pathForEditorOriginal + "/SavedScreen.png");
                _tex = new Texture2D(2, 2);
                _tex.LoadImage(_fileData);
                Debug.Log("Screenshot found on editor");
            }
        }
        else if (Directory.Exists(_pathForBuildOriginal))
        {
            if (File.Exists(_pathForBuildOriginal + Path.AltDirectorySeparatorChar + "SavedScreen.png"))
            {
                _fileData = File.ReadAllBytes(_pathForBuildOriginal + Path.AltDirectorySeparatorChar + "SavedScreen.png");
                _tex = new Texture2D(2, 2);
                _tex.LoadImage(_fileData);
                Debug.Log("Screenshot found on build");
            }
        }
        else Debug.Log("No screenshot to process was found");
    }

    private void ThresholdScreenshot()
    {
        for (int i = 0; i < 1920; i++)
        {
            for (int j = 0; j < 1080; j++)
            {
                float col = _tex.GetPixel(i, j).grayscale;
                if (col > 0.01f) _tex.SetPixel(i, j, Color.white); //thresholding
                else _tex.SetPixel(i, j, Color.black);
            }
        }
        byte[] bytes = _tex.EncodeToPNG();
        if (Directory.Exists(_pathForEditorBnW)) File.WriteAllBytes(_pathForEditorBnW + "/SavedScreenBlackAndWhite.png", bytes);
        if (Directory.Exists(_pathForBuildBnW)) File.WriteAllBytes(_pathForBuildBnW + Path.AltDirectorySeparatorChar + "SavedScreenBlackAndWhite.png", bytes);
        //if (Directory.Exists(_pathForBuildBnW)) File.WriteAllBytes(_pathForBuildBnW + Path.AltDirectorySeparatorChar + "SavedScreen.png", bytes); //path for my pc

        //if (Directory.Exists(_pathForEditor)) File.WriteAllBytes(_pathForEditor, bytes); //path for editor
    }

    private void FindStartAndSize()
    {
        if (File.Exists(_pathForEditorBnW))
        {
            _fileData = File.ReadAllBytes(_pathForEditorBnW);
            _texBnW = new Texture2D(2, 2);
            _texBnW.LoadImage(_fileData);

            Vector2 StartOfCode = FindStart();
            Vector2 StartOfBelowStart = FindBelowStart(StartOfCode);
            Debug.Log("BnW Screenshot found on editor");

        }
        else if (File.Exists(_pathForBuildBnW))
        {
            _fileData = File.ReadAllBytes(_pathForBuildBnW);
            _texBnW = new Texture2D(2, 2);
            _texBnW.LoadImage(_fileData);

            Vector2 StartOfCode = FindStart();
            Vector2 StartOfBelowStart = FindBelowStart(StartOfCode);
            Debug.Log("BnW Screenshot found on build");
        }
        else Debug.Log("No BnW screenshot to process was found");
    }


    private Vector2 FindStart()
    {
        Vector2 pos = new Vector2(-666, -666);
        //bool found = false;
        for (int j = 1079; j > -1; j--)
        {
            //Debug.Log(j);
            for (int i = 0; i < 1920; i++)
            {
                Color col = _texBnW.GetPixel(i, j);
                if (col == Color.black)
                {
                    pos = new Vector2(i, j);
                    Debug.Log(pos);
                    return pos;
                    //break;
                }
                //else pos = new Vector2(-200, -200);
            }
            //if (found) break;
        }
        if (pos == new Vector2(-666, -666)) Debug.Log("Failed to find start");
        Debug.Log(pos);
        return pos;
    }

    private Vector2 FindBelowStart(Vector2 posStart)
    {
        Vector2 pos = new Vector2(-666, -666);
        int x = (int)posStart.x;
        int y = (int)posStart.y;
        bool switchedColors = false;
        //bool found = false;
        for (int j = y - 1; j > -1; j--)
        {
            if (_texBnW.GetPixel(x, j) == Color.white)
            {
                switchedColors = true;
            }
            if ((_texBnW.GetPixel(x, j) == Color.black) && switchedColors)
            {
                pos = new Vector2(x, j);
                Debug.Log(pos);
                return pos;
                //found = true;
            }
            //if (found) break;
        }


        if (pos == new Vector2(-666, -666)) Debug.Log("Failed to find below");
        Debug.Log(pos);
        return pos;
    }


}
