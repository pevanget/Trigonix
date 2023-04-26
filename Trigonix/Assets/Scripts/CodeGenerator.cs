using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
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
    public bool StartTestGenerateCode = false;


    [SerializeField] private GameObject _UINumberOfElements;
    [SerializeField] private GameObject _element;
    [SerializeField] private int _heightOfTriangle;
    [SerializeField] private int _maxNumberOfCharacters;
    [SerializeField] Vector2 _firstElementPosition;

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
    private TESTASCII _testASCII;
    private int _numberOfSpecialElements = 0;
    private int _totalMaxElements = 900;
    private int _maxLines = 30;
    private int _linesOfTriangle;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        _testASCII = FindObjectOfType<TESTASCII>();
        _testASCII.SetMaxSizeForString(_maxNumberOfCharacters);
        _textFieldHandler = FindObjectOfType<TextFieldHandler>();
        GetStringToEncode();
        _UINumberOfElementsText = _UINumberOfElements.GetComponent<TMP_Text>();
        if (_element == null) Debug.LogError("No element found");
        CalculateNumberOfElementsPerLine();

        _positionsOfElements = new Vector2[_totalNumberOfElements];
        _UINumberOfElementsText.text = "Total number of elements: " + _totalNumberOfElements;
        TestGenerateCode();

    }

    private void Initialize()
    {
        CalculateLinesToTotalElementsMatrix();
    }

    private void CalculateLinesToTotalElementsMatrix()
    {
        _linesToTotalElements = new int[_maxLines];

        for (int i = 0; i < _maxLines; i++)
        {
            _linesToTotalElements[i] = (i + 1) * (i + 1);
            //Debug.Log(_linesToTotalElements[i]);
        }
        //Debug.Log(_maxLines);
        //Debug.Log(_linesToTotalElements[_maxLines - 1]);
    }

    public void StartEncode()
    {
        GetStringToEncode();
        if (_testASCII.CheckStringValid(_stringToEncode))
        {
            Debug.Log("Finished checking string. Encoding...");
        }
        else return;

        int totalElementsNeeded = CalculateNumberOfErrorCorrectionElementsNeeded(_stringToEncode) + _stringToEncode.Length + _numberOfSpecialElements;
        bool isNotTooLargeCode = CheckNumberOfElements(totalElementsNeeded);
        if (!isNotTooLargeCode) return;
        Debug.Log(totalElementsNeeded);
        _linesOfTriangle = CalculateNumberOfLinesNeeded(totalElementsNeeded);
        Debug.Log(_linesOfTriangle);
        ///////////EDW
        _heightOfTriangle = _linesOfTriangle;
        ResetCode();
        TestGenerateCode();
        ///
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
            Debug.Log("Size of Code within limits");
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
        if (StartTestGenerateCode) //make it UI button
        {
            StartTestGenerateCode = false;
            ResetCode();
            TestGenerateCode();

        }
        if (_heightOfTriangle > 30) _heightOfTriangle = 30;
    }

    private void ResetCode()
    {
        if (_currentParent != null) Destroy(_currentParent);
        //_currentParent = null;
    }

    //public void GetCodePhrase(string stringToEncode)
    //{

    //}

    private void GenerateFirstElement()
    {
        _positionsOfElements[_counter] = _firstElementPosition;
        GameObject firstElement = Instantiate(_element, _firstElementPosition, Quaternion.identity, _parentObject.transform);
        _counter++;
        SpriteRenderer SR = firstElement.GetComponent<SpriteRenderer>();
        _sizeElementX = SR.bounds.size.x;
        _sizeElementY = SR.bounds.size.y;
        //Debug.Log(_sizeElementX + ", " + _sizeElementY);
        SR.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
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

                    //Debug.Log((int)((_numberOfElementsPerLine[i]) / 2));
                    //_positionsOfElements[counter].x = (j - ((int)((_numberOfElementsPerLine[i]) / 2))) + _sizeElementX * j / 2;
                    _positionsOfElements[_counter].x = _firstElementPosition.x - i * _sizeElementX / 2 + _sizeElementX * j / 2;
                    //_positionsOfElements[counter].x = (j - (int)(_numberOfElementsPerLine[i] / 2)) + _sizeElementX * j;
                    _positionsOfElements[_counter].y = _firstElementPosition.y - i * _sizeElementY;
                    GameObject element = Instantiate(_element, _positionsOfElements[_counter], Quaternion.identity, _parentObject.transform);
                    if (j % 2 == 1)
                    {
                        element.transform.RotateAround(element.transform.position, transform.forward, 180f);
                    }

                    SpriteRenderer SR = element.GetComponent<SpriteRenderer>();
                    SR.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                }
            }
        }
    }
}
