using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
//C:\Users\THIS USER\AppData\LocalLow\DefaultCompany\My project
public class ProcessScreenshot : MonoBehaviour
{

    //PROBLEM: ENCODE: Debug.Log(switchedColors);
    //[SerializeField]
    private int _pixelStepBelow = 0;
    private int _pixelStepRight = 0;
    private Vector2Int _startOfCode;
    private Vector2Int _endOfCode;
    private Texture2D _tex;
    private Texture2D _texBnW;
    private string _pathForEditor;
    private string _pathForBuild;
    private byte[] _fileData; //might need to reset
    // Start is called before the first frame update
    void Start()
    {
        _pathForEditor = Application.dataPath + "/../Screenshots";
        _pathForBuild = Application.persistentDataPath;

    }

    public void DecodeLastScreenshot()
    {
        LoadScreenshot();
        ThresholdScreenshot();
        FindSpecs();

        DecodeScreenshot();
    }



    private void DecodeScreenshot()
    {
        //for (int i = 0; i < length; i++)
        //{

        //}
    }

    private void LoadScreenshot()
    {
        //Application.dataPath + "/../Screenshots/SavedScreen.png"
        if (Directory.Exists(_pathForEditor))
        {
            if (File.Exists(_pathForEditor + "/SavedScreen.png"))
            {
                _fileData = File.ReadAllBytes(_pathForEditor + "/SavedScreen.png");
                _tex = new Texture2D(2, 2);
                _tex.LoadImage(_fileData);
                Debug.Log("Screenshot found on editor");
            }
        }
        else if (Directory.Exists(_pathForBuild))
        {
            if (File.Exists(_pathForBuild + Path.AltDirectorySeparatorChar + "SavedScreen.png"))
            {
                _fileData = File.ReadAllBytes(_pathForBuild + Path.AltDirectorySeparatorChar + "SavedScreen.png");
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
        if (Directory.Exists(_pathForEditor)) File.WriteAllBytes(_pathForEditor + "/SavedScreenBlackAndWhite.png", bytes);
        if (Directory.Exists(_pathForBuild)) File.WriteAllBytes(_pathForBuild + Path.AltDirectorySeparatorChar + "SavedScreenBlackAndWhite.png", bytes);

    }

    private void FindSpecs()
    {
        Vector2 startOfCode = new Vector2(-1, -1);
        Vector2 startOfBelowStart = new Vector2(-1, -1);
        Vector2 endOfCode = new Vector2(-1, -1);
        bool succesful = false;
        if (File.Exists(_pathForEditor + "/SavedScreenBlackAndWhite.png"))
        {
            _fileData = File.ReadAllBytes(_pathForEditor + "/SavedScreenBlackAndWhite.png");
            _texBnW = new Texture2D(2, 2);
            _texBnW.LoadImage(_fileData);
            succesful = true;

            Debug.Log("BnW Screenshot found on editor");

        }
        else if (File.Exists(_pathForBuild + "/SavedScreenBlackAndWhite.png"))
        {
            _fileData = File.ReadAllBytes(_pathForBuild + "/SavedScreenBlackAndWhite.png");
            _texBnW = new Texture2D(2, 2);
            _texBnW.LoadImage(_fileData);
            succesful = true;

            Debug.Log("BnW Screenshot found on build");
        }
        else Debug.Log("No BnW screenshot to process was found");
        startOfCode = FindStart();
        startOfBelowStart = FindBelowStart(startOfCode);
        endOfCode = FindEnd(startOfCode);
        Debug.Log(endOfCode);


        if (!succesful) return;

        _pixelStepBelow = (int)(startOfCode.y - startOfBelowStart.y);
        Debug.Log(_pixelStepBelow);
        Vector2 endMiddleOfFirstTriangle = new Vector2(startOfCode.x, startOfCode.y - _pixelStepBelow);
        _pixelStepRight = FindWidth(endMiddleOfFirstTriangle);
        Debug.Log(_pixelStepRight);

        Vector2 centerOfThirdTriangle = new Vector2(startOfCode.x, (int)(startOfCode.y - _pixelStepBelow * 1.5f));
        Debug.Log(startOfCode);
        Debug.Log(centerOfThirdTriangle);


    }

    private int FindWidth(Vector2 centerOfBottomEdge)
    {
        int width = 0;
        bool found = false;
        int x = (int)centerOfBottomEdge.x;
        int y = (int)centerOfBottomEdge.y;

        while (!found)
        {
            x++;
            Color col = _texBnW.GetPixel(x, y);
            if (col == Color.white) found = true;
        }
        width = (x - 1 - (int)centerOfBottomEdge.x) * 2;

        return width;
    }

    private Vector2 FindStart()
    {
        Vector2 pos = new Vector2(-666, -666);
        for (int j = 1079; j > -1; j--)
        {
            for (int i = 0; i < 1920; i++)
            {
                Color col = _texBnW.GetPixel(i, j);
                if (col == Color.black)
                {
                    pos = new Vector2(i, j);
                    Debug.Log(pos);
                    return pos;
                }
            }
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
            }
        }


        if (pos == new Vector2(-666, -666)) Debug.Log("Failed to find below");
        Debug.Log(pos);
        return pos;
    }

    private Vector2 FindEnd(Vector2 startCode)
    {
        int x = (int)startCode.x;
        int y = (int)startCode.y;
        int counter = 0;
        bool found = false;
        while (!found)
        {
            if ((_texBnW.GetPixel(x, counter)) == Color.black)
            {
                found = true;
            }
            counter++;
        }
        Vector2 end = new Vector2(x, counter - 1);
        return end;
    }


}
