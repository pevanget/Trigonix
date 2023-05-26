using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class Encoder : MonoBehaviour
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
    /// 

    /// Specs
    /// max 30 lines
    /// max 900 elements

    [SerializeField] Vector2 _firstElementPosition;
    [SerializeField] private GameObject _UINumberOfElements;
    [SerializeField] private GameObject _elementTriangle;
    [SerializeField] private int _maxNumberOfCharacters;
    [SerializeField] private Button _decodeButton;
    [SerializeField] private ASCIIToElement _asciiToElement;
    [SerializeField] private TextFieldHandler _textFieldHandler;
    [SerializeField] private CheckString _checkString;
    [SerializeField] private AdjustMyCamera _adjustMyCamera;

    public Encoding ascii = Encoding.ASCII;

    private int _linesOfTriangle;
    private int[] _linesToTotalElements;
    private int _attemptsGenerate = 0;
    private int _elementsCounter = 0;
    private Vector2[] _positionsOfElements;
    private float _sizeElementX, _sizeElementY;
    private int[] _numberOfElementsPerLine; //top to bottom
    private GameObject _currentParent = null;
    private int _totalNumberOfElements = 0;
    private GameObject _parentObject;
    private string _stringToEncode;
    private byte[] _encodedBytesASCII;
    private int _numberOfSpecialElements = 0;
    private int _totalMaxElements = 900;
    private int _maxLines = 30;

    private TMP_Text _UINumberOfElementsText;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        CalculateLinesToTotalElementsMatrix();
        CalculateSizeOfElement();
        _checkString.SetMaxSizeForString(_maxNumberOfCharacters);

        _UINumberOfElementsText = _UINumberOfElements.GetComponent<TMP_Text>();
        if (_elementTriangle == null) Debug.LogError("No element found");

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
            Debug.Log("Finished checking string.");
        }
        else
        {
            Debug.Log("String is invalid.");
            return;
        }
        int totalElementsNeeded = CalculateNumberOfErrorCorrectionElementsNeeded(_stringToEncode) + _stringToEncode.Length + _numberOfSpecialElements;
        bool isNotTooLargeCode = CheckNumberOfElements(totalElementsNeeded);
        if (!isNotTooLargeCode) return;
        //Debug.Log(totalElementsNeeded);
        _linesOfTriangle = CalculateNumberOfLinesNeeded(totalElementsNeeded);

        ///////////EDW
        _adjustMyCamera.AdjustCamera(_linesOfTriangle, _sizeElementY);
        CalculateNumberOfElementsPerLine();

        _positionsOfElements = new Vector2[_totalNumberOfElements];
        ResetCode();
        _encodedBytesASCII = ascii.GetBytes(_stringToEncode);

        Encode();
        ActivateDecodeButton();

    }

    

    private void ActivateDecodeButton()
    {
        _decodeButton.interactable = true;
    }

    private int CalculateNumberOfLinesNeeded(int elementsInCode)
    {
        for (int i = 0; i < _maxLines; i++)
        {
            if (elementsInCode <= _linesToTotalElements[i]) return i + 1;
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
            Debug.LogWarning("Size of Code is too large. Generate Code Aborted"); //make it ui visible OR check string should do that
            return false;
        }
    }

    private int CalculateNumberOfErrorCorrectionElementsNeeded(string stringToEncode)
    {
        return 0;
    }
    private void CalculateNumberOfElementsPerLine()
    {
        _numberOfElementsPerLine = new int[_linesOfTriangle];
        _totalNumberOfElements = 0;
        for (int i = 0; i < _linesOfTriangle; i++)
        {
            _numberOfElementsPerLine[i] = 1 + 2 * i;
            _totalNumberOfElements += _numberOfElementsPerLine[i];
        }
        _UINumberOfElementsText.text = "Total number of elements: " + _totalNumberOfElements;
    }

  

    private void ResetCode()
    {
        if (_currentParent != null) Destroy(_currentParent);
    }


    private void GenerateFirstElement()
    {
        _positionsOfElements[_elementsCounter] = _firstElementPosition;
        GameObject firstElement = Instantiate(_elementTriangle, _firstElementPosition, Quaternion.identity, _parentObject.transform);
        SpriteRenderer SR = firstElement.GetComponent<SpriteRenderer>();
        _sizeElementX = SR.bounds.size.x;
        _sizeElementY = SR.bounds.size.y;
        //Debug.Log(_sizeElementX + ", " + _sizeElementY);
        SR.color = _asciiToElement.PaintElement(_encodedBytesASCII[_elementsCounter]);
        //SR.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        _elementsCounter++;
    }

    private void GetStringToEncode() => _stringToEncode = _textFieldHandler.GetStringToEncode();
    public void Encode()
    {
        _attemptsGenerate++;
        CalculateNumberOfElementsPerLine();
        _parentObject = new GameObject("Parent Object no. " + _attemptsGenerate);
        _currentParent = _parentObject;
        _elementsCounter = 0;
        for (int i = 0; i < _linesOfTriangle; i++)
        {
            for (int j = 0; j < _numberOfElementsPerLine[i]; j++)
            {

                if (i == 0)
                {
                    GenerateFirstElement();
                }
                else
                {

                    _positionsOfElements[_elementsCounter].x = _firstElementPosition.x - i * _sizeElementX / 2 + _sizeElementX * j / 2;
                    //_positionsOfElements[counter].x = (j - (int)(_numberOfElementsPerLine[i] / 2)) + _sizeElementX * j;
                    _positionsOfElements[_elementsCounter].y = _firstElementPosition.y - i * _sizeElementY;
                    GameObject element = Instantiate(_elementTriangle, _positionsOfElements[_elementsCounter], Quaternion.identity, _parentObject.transform);
                    if (j % 2 == 1)
                    {
                        element.transform.RotateAround(element.transform.position, transform.forward, 180f);
                    }

                    SpriteRenderer SR = element.GetComponent<SpriteRenderer>();
                    if (_elementsCounter < _stringToEncode.Length) SR.color = _asciiToElement.PaintElement(_encodedBytesASCII[_elementsCounter]);
                    else SR.color = Color.gray;

                    _elementsCounter++;
                    if (_elementsCounter >= _stringToEncode.Length)
                    {
                        //Debug.Log("phinished");

                    }

                }
            }
        }
    }
    public Transform[] GetElementsTransforms()
    {
        Debug.Log("check an doulevei");
        if (_currentParent == null)
        {
            Debug.LogWarning("No elements were found");
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
}
