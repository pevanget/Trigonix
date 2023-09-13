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
    [SerializeField] private float _thresholdColor;
    private int _pixelStepBelow = 0;
    private int _pixelStepRight = 0;
    private Vector2Int _startOfCode;
    private Vector2Int _endOfCode;
    private int _linesCount;
    private Texture2D _tex;
    private Texture2D _texBnW;
    private Texture2D _texBnWWithSamplingPoints;
    private string _pathForEditor;
    private string _pathForBuild;
    private byte[] _fileData; //might need to reset

    private List<Vector2Int> _coordinatesOfLinesCentered = new List<Vector2Int>();
    private List<Vector2Int> _coordinatesOfTriangles = new List<Vector2Int>();


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
        FindCoords();

        ValidateCoords();

        //DecodeScreenshot();
    }

    private void ValidateCoords()
    {
        LoadScreenshot();
        for (int i = 0; i < _coordinatesOfTriangles.Count; i++)
        {
            Debug.Log(_coordinatesOfTriangles[i]);
            Debug.Log(_tex.GetPixel(_coordinatesOfTriangles[i].x, _coordinatesOfTriangles[i].y));
        }
        //byte[] bytes = _tex.EncodeToPNG();
        //if (Directory.Exists(Application.dataPath + "/../Screenshots")) File.WriteAllBytes(Application.dataPath + "/../Screenshots/SavedScreenBlueDotsAAA.png", bytes);
        //Debug.Log(_coordinatesOfTriangles.Count);
    }
    private void FindCoords()
    {
        FindCoordsOfLinesCentered();
        FindCoordsInLines();
    }

    private void FindCoordsOfLinesCentered()
    {
        _texBnWWithSamplingPoints = _texBnW;
        _coordinatesOfLinesCentered.Clear();

        Vector2Int pos = new Vector2Int(-666, -666);
        Vector2Int currentPosition = _startOfCode;
        Vector2Int firstWhitePixelInTriangle = new Vector2Int(-1, -1);
        Vector2Int lastWhitePixelInTriangle = new Vector2Int(-1, -1);
        Vector2Int thisLineCoordsCentered = new Vector2Int(-1, -1);
        bool alreadyFoundWhite = false;
        int linesCount = 0;
        if (_endOfCode.x < 0) //is this correct?
        {
            Debug.LogError("No valid screenshot found");
            return;
        }
        while (currentPosition.y > _endOfCode.y)
        {
            if (_texBnW.GetPixel(currentPosition.x, currentPosition.y) == Color.black)
            {
                if (alreadyFoundWhite)
                {
                    lastWhitePixelInTriangle = new Vector2Int(currentPosition.x, currentPosition.y + 1);
                    thisLineCoordsCentered = (firstWhitePixelInTriangle + lastWhitePixelInTriangle) / 2;
                    _coordinatesOfLinesCentered.Add(thisLineCoordsCentered);
                    _texBnWWithSamplingPoints.SetPixel(thisLineCoordsCentered.x, thisLineCoordsCentered.y, Color.blue);
                    alreadyFoundWhite = false;

                    linesCount++;
                }
            }
            else
            {
                if (!alreadyFoundWhite)
                {
                    alreadyFoundWhite = true;
                    firstWhitePixelInTriangle = currentPosition;
                }
            }
            currentPosition = new Vector2Int(currentPosition.x, currentPosition.y - 1);

        }
        Debug.Log(linesCount);
        _linesCount = linesCount;
        byte[] bytes = _texBnWWithSamplingPoints.EncodeToPNG();
        if (Directory.Exists(Application.dataPath + "/../Screenshots")) File.WriteAllBytes(Application.dataPath + "/../Screenshots/SavedScreenBlueDotsA.png", bytes);
    }

    private void FindCoordsInLines()
    {
        List<string> L = new List<string>(new string[10]);
        int totalNumberOfElements = _linesCount * _linesCount;
        _coordinatesOfTriangles = new List<Vector2Int>(new Vector2Int[totalNumberOfElements]);
        for (int currentLine = 0; currentLine < _linesCount; currentLine++)
        {
            FindCoordsInLine(currentLine);
        }
    }

    private void FindCoordsInLine(int line)
    {
        //if (line == 0) return;
        int blackLinesToEncounter = line;
        Vector2Int currentPosition = _coordinatesOfLinesCentered[line];
        Vector2Int firstWhitePixelInTriangle = new Vector2Int(-1, -1);
        Vector2Int lastWhitePixelInTriangle = new Vector2Int(-1, -1);
        Vector2Int thisTriangleCoordsCentered = new Vector2Int(-1, -1);
        int lineIndexStart = line * (line + 1); //0=0 1=2 2=6 3=12 4=20
        //Debug.Log(line);
        //Debug.Log(lineIndexStart);


        //Debug.Log(_coordinatesOfTriangles.Count);
        _coordinatesOfTriangles[lineIndexStart] = currentPosition;
        
        //GO LEFT
        bool alreadyFoundBlack = false;
        bool alreadyFoundStart = false;

        int countBlackLines = 0;
        int countTriangles = 0;
        while (blackLinesToEncounter>countBlackLines)
        {
            if (_texBnW.GetPixel(currentPosition.x, currentPosition.y) == Color.black)
            {
                if (!alreadyFoundBlack)
                {
                    alreadyFoundBlack = true;
                }
                else
                {
                    if (alreadyFoundStart)
                    {
                        lastWhitePixelInTriangle = new Vector2Int(currentPosition.x + 1, currentPosition.y);
                        currentPosition = (firstWhitePixelInTriangle + lastWhitePixelInTriangle)/ 2;
                        countTriangles++;

                        //Debug.Log(lineIndexStart);
                        //Debug.Log(countTriangles);

                        int index = lineIndexStart - countTriangles;
                        //Debug.Log("index " +index);
                        _coordinatesOfTriangles[index] = currentPosition;
                        //Debug.Log(_coordinatesOfTriangles[index]);


                        alreadyFoundStart = false;
                        alreadyFoundBlack = false;


                        _texBnWWithSamplingPoints.SetPixel(currentPosition.x, currentPosition.y, Color.blue);
                        countBlackLines++;
                    }
                }
            }
            else
            {
                if((!alreadyFoundStart)&&(alreadyFoundBlack))
                {
                    firstWhitePixelInTriangle = currentPosition;
                    alreadyFoundStart = true;
                }
            }
            currentPosition = new Vector2Int(currentPosition.x-1, currentPosition.y);
        }
        byte[] bytes = _texBnWWithSamplingPoints.EncodeToPNG();
        if (Directory.Exists(Application.dataPath + "/../Screenshots")) File.WriteAllBytes(Application.dataPath + "/../Screenshots/SavedScreenBlueDotsB.png", bytes);

        //GO RIGHT
         alreadyFoundBlack = false;
         alreadyFoundStart = false;
        currentPosition = _coordinatesOfLinesCentered[line];
        countBlackLines = 0;
         countTriangles = 0;
        while (blackLinesToEncounter > countBlackLines)
        {
            if (_texBnW.GetPixel(currentPosition.x, currentPosition.y) == Color.black)
            {
                if (!alreadyFoundBlack)
                {
                    alreadyFoundBlack = true;
                }
                else
                {
                    if (alreadyFoundStart)
                    {
                        lastWhitePixelInTriangle = new Vector2Int(currentPosition.x - 1, currentPosition.y);
                        currentPosition = (firstWhitePixelInTriangle + lastWhitePixelInTriangle) / 2;
                        countTriangles++;
                        int index = lineIndexStart + countTriangles;

                        _coordinatesOfTriangles[index] = currentPosition;

                        alreadyFoundStart = false;
                        alreadyFoundBlack = false;


                        _texBnWWithSamplingPoints.SetPixel(currentPosition.x, currentPosition.y, Color.blue);
                        countBlackLines++;
                    }
                }
            }
            else
            {
                if ((!alreadyFoundStart) && (alreadyFoundBlack))
                {
                    firstWhitePixelInTriangle = currentPosition;
                    alreadyFoundStart = true;
                }
            }
            currentPosition = new Vector2Int(currentPosition.x + 1, currentPosition.y);
        }
        bytes = _texBnWWithSamplingPoints.EncodeToPNG();
        if (Directory.Exists(Application.dataPath + "/../Screenshots")) File.WriteAllBytes(Application.dataPath + "/../Screenshots/SavedScreenBlueDotsB.png", bytes);

    }

    private void GoLeft()
    {

    }

    //private void DecodeScreenshot()
    //{
    //    _decScreen.StartDecoding(_tex, _startOfCode, _endOfCode, _pixelStepBelow, _pixelStepRight);
    //}

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
        Texture2D tempTex = _tex;
        for (int i = 0; i < 1920; i++)
        {
            for (int j = 0; j < 1080; j++)
            {
                float col = tempTex.GetPixel(i, j).grayscale;
                if (col > _thresholdColor) tempTex.SetPixel(i, j, Color.white); //thresholding
                else tempTex.SetPixel(i, j, Color.black);
            }
        }
        byte[] bytes = tempTex.EncodeToPNG();
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

        if (!succesful) return;

        _startOfCode = FindStart();
        _startOfBelowStart = FindBelowStart(_startOfCode);
        _endOfCode = FindEnd(_startOfCode);
        //Debug.Log(_endOfCode);



        _pixelStepBelow = (int)(_startOfCode.y - _startOfBelowStart.y);
        Debug.Log("height triangle " + _pixelStepBelow);
        Vector2Int endMiddleOfFirstTriangle = new Vector2Int(_startOfCode.x, _startOfCode.y - _pixelStepBelow);
        _pixelStepRight = FindWidth(endMiddleOfFirstTriangle);
        Debug.Log("width triangle " + _pixelStepRight);

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
                //this needs something
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
        Vector2Int end = new Vector2Int(x, counter - 1);
        return end;
    }


}
