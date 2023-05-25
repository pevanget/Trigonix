using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Text;
using System;
//using System;

public class CodeGenerator : MonoBehaviour
{
    /// <summary>
    /// Requirements:
    /// -Number of elements needed
    /// -Calculate error correction
    /// -ROBUST
    /// - DYNAMIC MASKING? appearance reasons - spread the error
    /// - reconstruction (vgazw fwtografia, einai "diplwmeno" reconstruct
    /// 
    /// </summary>

    [SerializeField] private GameObject _UINumberOfElements;
    [SerializeField] private GameObject _elementTriangle;
    [SerializeField] private int _heightOfTriangle;
    [SerializeField] private int _maxNumberOfCharacters;
    [SerializeField] Vector2 _firstElementPosition;
    [SerializeField] private Button _decodeButton;

    public Encoding ascii = Encoding.ASCII;

    private byte[] _encodedBytesASCII;
    private ASCIIToElement _asciiToElement;
    private TextFieldHandler _textFieldHandler;
    private int[] _linesToTotalElements;
    private int[] _numberOfElementsPerLine; //top to bottom
    private Vector2[] _positionsOfElements;
    private float _sizeElementX;
    private float _sizeElementY;
    private int _attemptsGenerate = 0;
    private GameObject _currentParent = null;
    private int _totalNumberOfElements = 0;
    private TMP_Text _UINumberOfElementsText;
    private int _counter = 0;
    private GameObject _parentObject;
    private string _stringToEncode;
    private CheckString _checkString;
    private int _numberOfSpecialElements = 0;
    private int _totalMaxElements = 900;
    private int _maxLines = 30;
    private int _linesOfTriangle;
    private GameObject[] elements;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();    
    }
    private void Initialize()
    {
        CalculateLinesToTotalElementsMatrix();
        CalculateSizeOfElement();
        _checkString = FindObjectOfType<CheckString>();
        _checkString.SetMaxSizeForString(_maxNumberOfCharacters);
        _asciiToElement = FindObjectOfType<ASCIIToElement>();
        _textFieldHandler = FindObjectOfType<TextFieldHandler>();
        //GetStringToEncode();
        _UINumberOfElementsText = _UINumberOfElements.GetComponent<TMP_Text>();
        if (_elementTriangle == null) Debug.LogError("No element found");
        //CalculateNumberOfElementsPerLine();

        _positionsOfElements = new Vector2[_totalNumberOfElements];
        _UINumberOfElementsText.text = "Total number of elements: " + _totalNumberOfElements;
    }
    private void CalculateLinesToTotalElementsMatrix()
    {
        _linesToTotalElements = new int[_maxLines];

        for (int i = 0; i < _maxLines; i++)
        {
            _linesToTotalElements[i] = (i + 1) * (i + 1);
        }
    }

    private void CalculateSizeOfElement()
    {
        GameObject element = Instantiate(_elementTriangle, _firstElementPosition, Quaternion.identity);
        SpriteRenderer SR = element.GetComponent<SpriteRenderer>();
        _sizeElementX = SR.bounds.size.x;
        _sizeElementY = SR.bounds.size.y;
        Destroy(element);
    }

    public void StartEncode()
    {
        GetStringToEncode();
        if (_checkString.CheckStringValid(_stringToEncode))
        {
            Debug.Log("Finished checking string. Encoding...");
        }
        else return;

        int totalElementsNeeded = CalculateNumberOfErrorCorrectionElementsNeeded(_stringToEncode) + _stringToEncode.Length + _numberOfSpecialElements;
        bool isNotTooLargeCode = CheckNumberOfElements(totalElementsNeeded); //should change, seems unnecessary
        if (!isNotTooLargeCode) return;
        //Debug.Log(totalElementsNeeded);
        _linesOfTriangle = CalculateNumberOfLinesNeeded(totalElementsNeeded);
        //Debug.Log(_linesOfTriangle);
        ///////////EDW
        _heightOfTriangle = _linesOfTriangle;
        AdjustCamera(_heightOfTriangle);
        CalculateNumberOfElementsPerLine();

        _positionsOfElements = new Vector2[_totalNumberOfElements];
        ResetCode();
        _encodedBytesASCII = ascii.GetBytes(_stringToEncode);

        TestGenerateCode();
        ActivateDecodeButton();
        ///
    }

    private void AdjustCamera(int heightOfTriangle)
    {
        AdjustMyCamera adjustMyCamera = FindObjectOfType<AdjustMyCamera>();
        heightOfTriangle++;
        if (heightOfTriangle > 9)
        {
            adjustMyCamera._myCam.orthographicSize = Mathf.Floor((float) ((float) (heightOfTriangle) / 5f)) * 3 + 1;
        }
        else
        {
            adjustMyCamera._myCam.orthographicSize = 5;
        }
        if (heightOfTriangle > 8)
        {
            heightOfTriangle -= 8;
            heightOfTriangle /= 2;
            Vector3 positionToMove = new Vector3(0, -heightOfTriangle * _sizeElementY, -10);
            
            adjustMyCamera.AdjustCamera(positionToMove);
            Debug.Log(heightOfTriangle + " " + positionToMove + " " + _sizeElementY);
        }
        else
        {
            heightOfTriangle = 8 - heightOfTriangle;
            heightOfTriangle /= 2;
            Vector3 positionToMove = new Vector3(0, heightOfTriangle * _sizeElementY, -10);
            
            adjustMyCamera.AdjustCamera(positionToMove);
            //Debug.Log(heightOfTriangle + " " + positionToMove + " " + _sizeElementY);
        }
        
        

    }

    private void ActivateDecodeButton()
    {
        _decodeButton.interactable = true;
    }

    private int CalculateNumberOfLinesNeeded(int elementsInCode)
    {
        //int linesNeeded = 0;
        for (int i = 0; i < _maxLines; i++)
        {
            if (elementsInCode <= _linesToTotalElements[i]) return i+1;
        }
        Debug.LogWarning("Something bad happened");
        return -1;
    }
    private bool CheckNumberOfElements(int elementsNeeded)
    {
        if (elementsNeeded <= _totalMaxElements)
        {
            Debug.Log("Size of Code within limits (is this necessary?)");
            return true;
        }
        else
        {
            Debug.LogWarning("Size of Code is too large. Generate Code Aborted");
            return false;
        }
    }

    private int CalculateNumberOfErrorCorrectionElementsNeeded(string stringToEncode)
    {
        return 0;
    }
    void CalculateNumberOfElementsPerLine()
    {
        _numberOfElementsPerLine = new int[_heightOfTriangle];
        _totalNumberOfElements = 0;
        for (int i = 0; i < _heightOfTriangle; i++)
        {
            _numberOfElementsPerLine[i] = 1 + 2 * i;
            _totalNumberOfElements += _numberOfElementsPerLine[i];
        }
        _UINumberOfElementsText.text = "Total number of elements: " + _totalNumberOfElements;
    }

    // Update is called once per frame
    void Update()
    {
        //if (StartTestGenerateCode) //make it UI button
        //{
        //    StartTestGenerateCode = false;
        //    ResetCode();
        //     TestGenerateCode();

        //}
        //if (_heightOfTriangle > 30) _heightOfTriangle = 30;
    }

    private void ResetCode()
    {
        if (_currentParent != null) Destroy(_currentParent);
        //_currentParent = null;
    }

    //public void GetCodePhrase(string stringToEncode)
    //{

    //}

    public Transform[] GetElementsTransforms()
    {
        Debug.Log("check an doulevei");
        if (_currentParent == null)
        { Debug.LogWarning("No elements were found");
            return null;
        }
        else
        {
            Transform[] elemTransf = new Transform[_currentParent.transform.childCount];
            int count = 0;
            foreach (Transform child in _currentParent.transform)
            {
                elemTransf[count] = child;
                count++;
            }
            return elemTransf;
        }
        
    }

    private void EncodeString(string stringToEncode)
    {
        
    }

    private void GenerateFirstElement()
    {
        _positionsOfElements[_counter] = _firstElementPosition;
        GameObject firstElement = Instantiate(_elementTriangle, _firstElementPosition, Quaternion.identity, _parentObject.transform);
        SpriteRenderer SR = firstElement.GetComponent<SpriteRenderer>();
        _sizeElementX = SR.bounds.size.x;
        _sizeElementY = SR.bounds.size.y;
        //Debug.Log(_sizeElementX + ", " + _sizeElementY);
        SR.color = _asciiToElement.PaintElement(_encodedBytesASCII[_counter]);
        //SR.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        _counter++;
    }

    private void GetStringToEncode() => _stringToEncode = _textFieldHandler.GetStringToEncode();
    public void TestGenerateCode()
    {
        _attemptsGenerate++;
        CalculateNumberOfElementsPerLine();
        _parentObject = new GameObject("Parent Object no. " + _attemptsGenerate);
        _currentParent = _parentObject;
        _counter = 0;
        for (int i = 0; i < _heightOfTriangle; i++)
        {
            for (int j = 0; j < _numberOfElementsPerLine[i]; j++)
            {

                if (i == 0)
                {
                    GenerateFirstElement();
                }
                else
                {
                    //Debug.Log(_counter);
                    //Debug.Log((int)((_numberOfElementsPerLine[i]) / 2));
                    //_positionsOfElements[counter].x = (j - ((int)((_numberOfElementsPerLine[i]) / 2))) + _sizeElementX * j / 2;
                    _positionsOfElements[_counter].x = _firstElementPosition.x - i * _sizeElementX / 2 + _sizeElementX * j / 2;
                    //_positionsOfElements[counter].x = (j - (int)(_numberOfElementsPerLine[i] / 2)) + _sizeElementX * j;
                    _positionsOfElements[_counter].y = _firstElementPosition.y - i * _sizeElementY;
                    GameObject element = Instantiate(_elementTriangle, _positionsOfElements[_counter], Quaternion.identity, _parentObject.transform);
                    if (j % 2 == 1)
                    {
                        element.transform.RotateAround(element.transform.position, transform.forward, 180f);
                    }

                    SpriteRenderer SR = element.GetComponent<SpriteRenderer>();
                    if (_counter < _stringToEncode.Length) SR.color = _asciiToElement.PaintElement(_encodedBytesASCII[_counter]);
                    else SR.color = Color.gray;
                        //SR.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));


                        _counter++;
                    if (_counter >= _stringToEncode.Length)
                    {
                        //Debug.Log("phinished");
                        
                    }

                }
            }
        }
    }
}
