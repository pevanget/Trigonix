using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using System;

public class DecodeScreenshot : MonoBehaviour
{
    [SerializeField] private Decoder _decoder;
    [SerializeField] private Masker _masker;
    [SerializeField] private TMP_InputField _decodedTextDisplayed;

    private Texture2D _tex;
    private Vector2Int _currentPosition;
    private string _pathForEditor;
    private string _pathForBuild;
    private byte[] _fileData; //might need to reset
    private string _decodedString;

    private void Start()
    {

        _pathForEditor = Application.dataPath + "/../Screenshots";
        _pathForBuild = Application.persistentDataPath;
    }


    public void StartDecoding(List<Vector2Int> coordsOfTriangles)
    {
        //Debug.Log(coordsOfTriangles.Count);
        _decodedString = null;
        LoadScreenshot();
        int maskID = _decoder.FindMask(_tex.GetPixel(coordsOfTriangles[0].x, coordsOfTriangles[0].y));
        Debug.Log(maskID);
        for (int i = 1; i < coordsOfTriangles.Count; i++)
        {
            Color colorToUnmask = _tex.GetPixel(coordsOfTriangles[i].x, coordsOfTriangles[i].y);
            //Debug.Log(colorToUnmask);
            Color colorUnmasked = _masker.GetMask(maskID).UnmaskElement(i, colorToUnmask);
            //Debug.Log(colorUnmasked);
            char a =_decoder.DecodeColor(colorUnmasked);
            //Debug.Log(a);
            _decodedString += a;
            //Debug.Log(a);
            //Debug.Log(coordsOfTriangles[i]);
            //Debug.Log(_tex.GetPixel(coordsOfTriangles[i].x, coordsOfTriangles[i].y));
        }
        _decodedTextDisplayed.text = _decodedString;
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






    //katevaineis olokliro
    //deksia miso
    //absolute coord calculation to avoid consecutive stacking errors

    //public void StartDecoding(Texture2D tex, Vector2Int startOfCode, Vector2Int endOfCode, int height, int width)
    //{
    //    _tex = tex;

    //    //height++;
    //    //SaveSpecs(tex, startOfCode, endOfCode, height, width);
    //    startOfCode = new Vector2Int(startOfCode.x, startOfCode.y - height / 2);
    //    _currentPosition = startOfCode;
    //    Debug.Log(_currentPosition);
    //    int counter = 0;
    //    while (_currentPosition.y > endOfCode.y)
    //    {
    //        if (counter==0)
    //        {
    //            //DecodeFirstElement
    //            _tex.SetPixel(_currentPosition.x, _currentPosition.y, Color.blue);
    //        }
    //        else
    //        {
    //            _currentPosition = CalculateCoords(counter, startOfCode, height, width);
    //            if (_currentPosition.y > endOfCode.y) _tex.SetPixel(_currentPosition.x, _currentPosition.y, Color.blue);
    //        }
    //        counter++;

    //    }
    //    byte[] bytes = _tex.EncodeToPNG();
    //    if (Directory.Exists(Application.dataPath + "/../Screenshots")) File.WriteAllBytes(Application.dataPath + "/../Screenshots/SavedScreenBlueDots.png", bytes);
    //}




    //private Vector2Int CalculateCoords(int count, Vector2Int startPos, int h, int w)
    //{
    //    Vector2Int val;
    //    int line = Mathf.FloorToInt(Mathf.Sqrt(count));
    //    int yCoord = startPos.y - line * h;
    //    int posInLine = count - line * line;
    //    int relativePosToCenterLine = posInLine - line;
    //    int xCoord = startPos.x + relativePosToCenterLine * w / 2;
    //    val = new Vector2Int(xCoord, yCoord);


    //    Debug.Log("element number " + count);
    //    Debug.Log("line number " + line);
    //    Debug.Log("posInLine " + posInLine);
    //    Debug.Log("relativePosToCenterLine " + relativePosToCenterLine);
    //    Debug.Log(val);


    //    return val;
    //}
}
