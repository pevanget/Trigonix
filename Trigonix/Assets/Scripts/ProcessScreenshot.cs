using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
//C:\Users\THIS USER\AppData\LocalLow\DefaultCompany\My project
public class ProcessScreenshot : MonoBehaviour
{

    //PROBLEM: ENCODE: Debug.Log(switchedColors);
    [SerializeField] private DecodeScreenshot _decScreen;
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
        _decScreen.StartDecoding(_tex, _startOfCode, _endOfCode, _pixelStepBelow, _pixelStepRight);
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
        
        Vector2 _startOfBelowStart = new Vector2(-1, -1);
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
        _startOfCode = FindStart();
        _startOfBelowStart = FindBelowStart(_startOfCode);
        _endOfCode = FindEnd(_startOfCode);
        //Debug.Log(_endOfCode);


        if (!succesful) return;

        _pixelStepBelow = (int)(_startOfCode.y - _startOfBelowStart.y);
        Debug.Log(_pixelStepBelow);
        Vector2Int endMiddleOfFirstTriangle = new Vector2Int(_startOfCode.x, _startOfCode.y - _pixelStepBelow);
        _pixelStepRight = FindWidth(endMiddleOfFirstTriangle);
        Debug.Log(_pixelStepRight);

        //Vector2Int centerOfThirdTriangle = new Vector2Int(_startOfCode.x, (int)(_startOfCode.y - _pixelStepBelow * 1.5f));
        //Debug.Log(_startOfCode);
        //Debug.Log(centerOfThirdTriangle);


    }

    private int FindWidth(Vector2Int centerOfBottomEdge)
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

    private Vector2Int FindStart()
    {
        Vector2Int pos = new Vector2Int(-666, -666);
        for (int j = 1079; j > -1; j--)
        {
            for (int i = 0; i < 1920; i++)
            {
                Color col = _texBnW.GetPixel(i, j);
                if (col == Color.black)
                {
                    pos = new Vector2Int(i, j);
                    //Debug.Log(pos);
                    return pos;
                }
            }
        }
        if (pos == new Vector2Int(-666, -666)) Debug.Log("Failed to find start");
        //Debug.Log(pos);
        return pos;
    }

    private Vector2Int FindBelowStart(Vector2Int posStart)
    {
        Vector2Int pos = new Vector2Int(-666, -666);
        int x = posStart.x;
        int y = posStart.y;
        bool switchedColors = false;
        for (int j = y - 1; j > -1; j--)
        {
            if (_texBnW.GetPixel(x, j) == Color.white)
            {
                switchedColors = true;
            }
            if ((_texBnW.GetPixel(x, j) == Color.black) && switchedColors)
            {
                pos = new Vector2Int(x, j);
                //Debug.Log(pos);
                return pos;
            }
        }


        if (pos == new Vector2Int(-666, -666)) Debug.Log("Failed to find below");
        //Debug.Log(pos);
        return pos;
    }

    private Vector2Int FindEnd(Vector2Int startCode)
    {
        int x = startCode.x;
        int y = startCode.y;
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
        Vector2Int end = new Vector2Int (x, counter - 1);
        return end;
    }


}
